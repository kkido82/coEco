using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Repositories;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    /// <summary>
    /// the item was recied by requesting unit
    /// </summary>
    public class ActivateOrderRequest : OrderRequestBase, IRequest<OrderActivated>
    {
        public ActivateOrderRequest(int orderId, int memberId) : base(orderId, memberId)
        {
        }
    }

    public class OrderActivated
    {
        public OrderActivated(int orderId, int byMemberId, UnitItem affectedUnitItem = null)
        {
            OrderId = orderId;
            ByMemberId = byMemberId;
            AffectedUnitItem = affectedUnitItem;
        }

        public int OrderId { get; }
        public int ByMemberId { get; }
        public UnitItem AffectedUnitItem { get; }
    }

    public class ActivateHandler : OrderHandlerBase, IHandler<ActivateOrderRequest, OrderActivated>
    {
        public ActivateHandler(IOrderingService service) : base(service)
        {
        }

        protected override AuthData AuthData => new AuthData(OrderStatusId.Confirmed, OrderSide.Requesting, Permission.CanOpenAnOrder);

        public async Task<Result<OrderActivated>> Handle(ActivateOrderRequest request)
        {
            var ctx = await LoadContext(request);
            if (!IsAuthorized(ctx))
                return GeneralErrors.UnauthorizedError();

            var update = await Update(ctx);
            if (!update.Success)
                return Errors.FailToUpdate(update.ErrorMessage);

            return new OrderActivated(request.OrderId, request.MemberId, update.Item);
        }

        /// <summary>
        /// updating unit inventory 
        /// and updatin status
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private async Task<UpdateModel> Update(RequestContext ctx)
        {
            var unitItem = await service.GetUnitItem(ctx.Order.LendingUnitId, ctx.Order.ItemId);
            unitItem.Amount--;
            var res = await service.Update(unitItem);
            if (!res.Success) return new UpdateModel(res.ErrorMessage);
            ctx.Order.Status = OrderStatusId.Active;
            var updateRes = await service.UpdateOrder(ctx.Order);
            if (!updateRes.Success) return new UpdateModel(updateRes.ErrorMessage);

            return new UpdateModel(unitItem);
        }

        class UpdateModel
        {
            public UpdateModel(UnitItem item)
            {
                Success = true;
                Item = item;
            }

            public UpdateModel(string errorMessage)
            {
                Success = false;
                ErrorMessage = errorMessage;
            }

            public bool Success { get; }
            public UnitItem Item { get; }
            public string ErrorMessage { get; }
        }
    }
}

