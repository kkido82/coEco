using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CoEco.Core.Tests.Ordering.Handlers
{
    [TestFixture]
    public class ApproveHandler_Tests
    {
        [Test]
        public async Task CanApprove()
        {
            var order = new Order
            {
                Id = 1,
                ItemId = 1,
                LendingUnitId = 2,
                RequestingMemberId = 1,
                RequestingUnitId = 1,
                Status = OrderStatusId.New
            };
            var member = new Member(2, 1, Permission.CanConfirmOrder);
            var orderingService = new Mock<IOrderingService>();
            orderingService.Setup(a => a.GetOrder(1)).ReturnsAsync(order);
            orderingService.Setup(a => a.GetMemberById(2)).ReturnsAsync(member);
            orderingService.Setup(a => a.UpdateOrder(It.IsAny<Order>()))
                .ReturnsAsync(new Core.Ordering.Dto.GatewayResponses.BaseGatewayResponse(true));
            var handler = new AprroveHanlder(orderingService.Object);

            var request = new ApproveRequest(1, 2);

            var response = await handler.Handle(request);

            Assert.IsTrue(response.Success);
        }

        [Test]
        public async Task Cannot_Approve_Without_Permission()
        {
            var order = new Order
            {
                Id = 1,
                ItemId = 1,
                LendingUnitId = 2,
                RequestingMemberId = 1,
                RequestingUnitId = 1,
                Status = OrderStatusId.New
            };
            var member = new Member(2, 1);
            var orderingService = new Mock<IOrderingService>();
            orderingService.Setup(a => a.GetOrder(1)).ReturnsAsync(order);
            orderingService.Setup(a => a.GetMemberById(2)).ReturnsAsync(member);
            orderingService.Setup(a => a.UpdateOrder(It.IsAny<Order>()))
                .ReturnsAsync(new Core.Ordering.Dto.GatewayResponses.BaseGatewayResponse(true));
            var handler = new AprroveHanlder(orderingService.Object);

            var request = new ApproveRequest(1, 2);

            var response = await handler.Handle(request);

            Assert.IsFalse(response.Success);
        }

        [Test]
        public async Task Only_Requesting_Unit_member_Can_Approve()
        {
            var order = new Order
            {
                Id = 1,
                ItemId = 1,
                LendingUnitId = 2,
                RequestingMemberId = 1,
                RequestingUnitId = 3,
                Status = OrderStatusId.New
            };
            var member = new Member(2, 1,Permission.CanConfirmOrder);
            var orderingService = new Mock<IOrderingService>();
            orderingService.Setup(a => a.GetOrder(1)).ReturnsAsync(order);
            orderingService.Setup(a => a.GetMemberById(2)).ReturnsAsync(member);
            orderingService.Setup(a => a.UpdateOrder(It.IsAny<Order>()))
                .ReturnsAsync(new Core.Ordering.Dto.GatewayResponses.BaseGatewayResponse(true));
            var handler = new AprroveHanlder(orderingService.Object);

            var request = new ApproveRequest(1, 2);

            var response = await handler.Handle(request);

            Assert.IsFalse(response.Success);
        }
    }
}
