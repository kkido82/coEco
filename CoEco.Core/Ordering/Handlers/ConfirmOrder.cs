using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Repositories;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    /// <summary>
    /// confim order by lending unit 
    /// </summary>
    public class ConfirmOrderRequest : OrderRequestBase, IRequest<OrderConfirmed> {
        public ConfirmOrderRequest(int orderId, int memberId):base(orderId, memberId)
        {
        }
    }

    public class OrderConfirmed
    {
        public OrderConfirmed(int orderId, int byMemberId, double transferAmount)
        {
            OrderId = orderId;
            ByMemberId = byMemberId;
            TransferAmount = transferAmount;
        }

        public int OrderId { get; }
        public int ByMemberId { get; }
        public double TransferAmount { get; }
    }

    public class ConfirmHandler : OrderHandlerBase ,IHandler<ConfirmOrderRequest, OrderConfirmed>
    {

        public ConfirmHandler(IOrderingService service) :base(service)
        {
        }

        protected override AuthData AuthData => new AuthData(OrderStatusId.Approved, OrderSide.Lending, Permission.CanConfirmOrder);

        public async Task<Result<OrderConfirmed>> Handle(ConfirmOrderRequest request)
        {
            var ctx = await LoadContext(request);
            if (!IsAuthorized(ctx))
                return GeneralErrors.UnauthorizedError();

            var update = await Update(ctx);
            if (!update.Success)
                return Errors.FailToUpdate(update.ErrorMessage);

            return new OrderConfirmed(request.OrderId, request.MemberId, update.AmountTransferd);
        }

        /// <summary>
        /// transfer money from requesting unit to lending unit
        /// and updating status to Confirmed
        /// </summary>
        /// <returns></returns> 
        async Task<UpdateResult> Update(RequestContext ctx)
        {
            var item = await service.GetItem(ctx.Order.ItemId);
            var res = await service.TransterMoney(ctx.Order.RequestingUnitId, ctx.Order.LendingUnitId, item.Price);
            if (!res.Success) return new UpdateResult(res.ErrorMessage);
            ctx.Order.Price = item.Price;
            ctx.Order.Status = OrderStatusId.Confirmed;
            var updateRes = await service.UpdateOrder(ctx.Order);
            if (updateRes.Success)
                return new UpdateResult(true, item.Price);
            return new UpdateResult(updateRes.ErrorMessage);

        }

        class UpdateResult
        {
            public UpdateResult(bool success, double amountTransferd = 0)
            {
                Success = success;
                AmountTransferd = amountTransferd;
            }
            public UpdateResult(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
            public bool Success { get; set; }
            public double AmountTransferd { get; set; }
            public string ErrorMessage { get; }
        }
    }
}
