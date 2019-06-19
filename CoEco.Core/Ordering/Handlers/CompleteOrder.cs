using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Repositories;
using System;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    /// <summary>
    /// the item was returned to the lending unit
    /// </summary>
    public class CompleteOrderRequest : OrderRequestBase, IRequest<OrderCompleted>
    {
        public CompleteOrderRequest(int orderId, int memberId) : base(orderId, memberId)
        {
        }
    }

    public class OrderCompleted
    {
        public OrderCompleted(int orderId, int memberId, UnitItem affectedUnitItem)
        {
            OrderId = orderId;
            MemberId = memberId;
            AffectedUnitItem = affectedUnitItem;
        }

        public int OrderId { get; }
        public int MemberId { get; }
        public UnitItem AffectedUnitItem { get; }
    }

    public class CompleteOrderHandler : OrderHandlerBase, IHandler<CompleteOrderRequest, OrderCompleted>
    {
        public CompleteOrderHandler(IOrderingService service) : base(service)
        {
        }

        protected override AuthData AuthData => new AuthData(Domain.OrderStatusId.Active, OrderSide.Lending, Domain.Permission.CanUpdateInventory);

        public async Task<Result<OrderCompleted>> Handle(CompleteOrderRequest request)
        {
            var ctx = await LoadContext(request);
            if (!IsAuthorized(ctx))
                return GeneralErrors.UnauthorizedError();

            var update = await Update(ctx);
            if (!update.Success)
                return Errors.FailToUpdate(update.ErrorMessage);

            return new OrderCompleted(request.OrderId, request.MemberId, update.UnitItem);
        }

        private async Task<UpdateModel> Update(RequestContext ctx)
        {
            var unitItem = await service.GetUnitItem(ctx.Order.LendingUnitId, ctx.Order.ItemId);
            unitItem.Amount++;
            var res = await service.Update(unitItem);
            if (!res.Success) return new UpdateModel(res.ErrorMessage);
            ctx.Order.Status = OrderStatusId.Completed;
            var updateRes = await service.UpdateOrder(ctx.Order);
            return new UpdateModel(unitItem, ctx.Order.Status);
        }

        class UpdateModel
        {
            public UpdateModel(UnitItem item, OrderStatusId newStatus)
            {
                Success = true;
                UnitItem = item;
                NewStatus = newStatus;
            }

            public UpdateModel(string errorMessage)
            {
                Success = false;
                ErrorMessage = errorMessage;
            }

            public bool Success { get; }
            public UnitItem UnitItem { get; }
            public string ErrorMessage { get; }
            public OrderStatusId NewStatus { get; }
        }
    }
}
