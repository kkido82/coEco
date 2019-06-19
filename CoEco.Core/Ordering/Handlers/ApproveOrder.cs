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
    /// approve order by requesting unit
    /// </summary>
    public class ApproveRequest : OrderRequestBase, IRequest<Approved>
    {
        public ApproveRequest(int orderId, int memberId) : base(orderId, memberId)
        {
        }

    }

    public class Approved
    {
        public Approved(int orderId, int byMemberId)
        {
            OrderId = orderId;
            ByMemberId = byMemberId;
        }
        public int OrderId { get; }
        public int ByMemberId { get; }
    }

    public class AprroveHanlder : OrderHandlerBase, IHandler<ApproveRequest, Approved>
    {
        protected override AuthData AuthData => new AuthData(OrderStatusId.New, OrderSide.Requesting, Permission.CanConfirmOrder);

        public AprroveHanlder(IOrderingService service): base(service)
        {
        }

        public async Task<Result<Approved>> Handle(ApproveRequest request)
        {
            var ctx = await LoadContext(request);
            if (!IsAuthorized(ctx))
                return GeneralErrors.UnauthorizedError();

            var update = await UpdateStatus(ctx);
            if (!update.Success)
                return Errors.FailToUpdate(update.ErrorMessage);

            return new Approved(request.OrderId, request.MemberId);
        }

        public Task<BaseGatewayResponse> UpdateStatus(RequestContext ctx)
        {
            ctx.Order.Status = OrderStatusId.Approved;
            return service.UpdateOrder(ctx.Order);

        }
    }
}
