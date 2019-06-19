using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    public abstract class OrderHandlerBase
    {
        protected abstract AuthData AuthData { get; }
        protected readonly IOrderingService service;

        public OrderHandlerBase(IOrderingService service)
        {
            this.service = service;
        }

        protected async Task<RequestContext> LoadContext(OrderRequestBase requst)
        {
            var ctx = new RequestContext
            {
                Member = await service.GetMemberById(requst.MemberId),
                Order = await service.GetOrder(requst.OrderId)
            };
            return ctx;
        }

        protected bool IsAuthorized(RequestContext ctx)
        {
            if (ctx.Order.Status != AuthData.ReqOrderStatus)
                return false;

            if (!ctx.Member.HasPermission(AuthData.ReqPermission))
                return false;

            var reqUnit = AuthData.IsFromRequestingUnit ? ctx.Order.RequestingUnitId : ctx.Order.LendingUnitId;
            if (ctx.Member.UnitId != reqUnit)
                return false;

            return true;
        }

    }


    public class RequestContext
    {
        public Order Order { get; set; }
        public Member Member { get; set; }
    }

    public class OrderRequestBase
    {
        public OrderRequestBase(int orderId, int memberId)
        {
            OrderId = orderId;
            MemberId = memberId;
        }

        public int OrderId { get; }
        public int MemberId { get; }
    }

    public enum OrderSide
    {
        Requesting,
        Lending
    }
    public class AuthData
    {
        public AuthData(OrderStatusId reqOrderStatus, OrderSide orderSide, Permission reqPermission)
        {
            ReqOrderStatus = reqOrderStatus;
            IsFromRequestingUnit = orderSide == OrderSide.Requesting;
            ReqPermission = reqPermission;
        }

        public OrderStatusId ReqOrderStatus { get; }
        public bool IsFromRequestingUnit { get; }
        public Permission ReqPermission { get; }
    }
}
