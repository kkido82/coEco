using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    public class CancelApprovedRequest : OrderRequestBase, IRequest<CanceledByLendingUnit>
    {
        public CancelApprovedRequest(int orderId, int memberId) : base(orderId, memberId)
        {
        }
    }

    public class CanceledByLendingUnit
    {
        public CanceledByLendingUnit(int orderId, int memberId)
        {
            OrderId = orderId;
            MemberId = memberId;
        }

        public int OrderId { get; }
        public int MemberId { get; }
    }

    public class CancelApprovedOrderHandler : OrderHandlerBase, IHandler<CancelApprovedRequest, CanceledByLendingUnit>
    {
        public CancelApprovedOrderHandler(IOrderingService service) : base(service)
        {
        }

        protected override AuthData AuthData => new AuthData(OrderStatusId.Approved, OrderSide.Lending, Domain.Permission.CanConfirmOrder);


        public async Task<Result<CanceledByLendingUnit>> Handle(CancelApprovedRequest request)
        {
            var ctx = await LoadContext(request);
            if (!IsAuthorized(ctx))
                return GeneralErrors.UnauthorizedError();

            var update = await Update(ctx);
            if (!update.Success)
                return Errors.FailToUpdate(update.ErrorMessage);

            return new CanceledByLendingUnit(request.OrderId, request.MemberId);
        }

        private async Task<BaseGatewayResponse> Update(RequestContext ctx)
        {
            ctx.Order.Status = OrderStatusId.CanceledByRequestingUnit;
            var updateRes = await service.UpdateOrder(ctx.Order);
            return updateRes;
        }
    }
}
