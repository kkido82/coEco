using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Repositories;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Handlers
{
    /// <summary>
    /// create a new order
    /// </summary>
    public class OpenOrderRequest : IRequest<OrderOpened>
    {
        public OpenOrderRequest(int byMemberId, int fromUnitId,int itemId, string remarks = null)
        {
            ByMemberId = byMemberId;
            FromUnitId = fromUnitId;
            Remarks = remarks;
            ItemId = itemId;
        }

        public int ByMemberId { get; }
        public int FromUnitId { get; }
        public int ItemId { get; set; }
        public string Remarks { get; }
    }

    [DisplayName("OrderWasCreated")]
    public class OrderOpened 
    {
        public int OrderId { get; }
        public OrderStatusId Status { get; }

        public OrderOpened(int orderId, OrderStatusId status)
        {
            OrderId = orderId;
            Status = status;
        }
    }

    public class OpenOrderHandler : IHandler<OpenOrderRequest, OrderOpened>
    {

        static readonly Permission[] canOpenPermission = new Permission[] {
            Permission.CanOpenAnOrder,
            Permission.CanConfirmOrder
        };
        private readonly IOrderingService service;

        public OpenOrderHandler(IOrderingService service)
        {
            this.service = service;
        }

        public async Task<Result<OrderOpened>> Handle(OpenOrderRequest request)
        {
            var member = await service.GetMemberById(request.ByMemberId);
            if (!CanOpenOrder(member))
                return GeneralErrors.UnauthorizedError();

            var status = member.HasPermission(Permission.CanConfirmOrder)
                    ? OrderStatusId.Approved 
                    : OrderStatusId.New;

            var order = new Order
            {
                LendingUnitId = request.FromUnitId,
                Remarks = request.Remarks,
                Status = status,
                RequestingMemberId = member.Id,
                RequestingUnitId = member.UnitId,
                ItemId = request.ItemId
            };

            var createRes = await service.CreateOrder(order);
            if (!createRes.Success)
                return Errors.FailToCreateOrder(createRes.ErrorMessage);

            return new OrderOpened(createRes.Id, status);
        }



        public bool CanOpenOrder(Member member) => canOpenPermission.Any(p => member.HasPermission(p));
    }



}
