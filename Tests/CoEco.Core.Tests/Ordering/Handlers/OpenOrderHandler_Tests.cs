using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses.Repositories;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CoEco.Core.Tests.Ordering.Handlers
{
    [TestFixture]
    public class OpenOrderHandler_Tests
    {
        [Test]
        public async Task Cannot_Open_Order_Without_Required_Permmision()
        {
            var member = new Member(1, 1);
            var handler = CreateHandler(member);

            var request = new OpenOrderRequest(1, 1, 1);
            var res = await handler.Handle(request);

            Assert.IsFalse(res.Success);
        }

        [Test]
        public async Task Can_Create_Order()
        {
            var member = new Member(1, 1, Permission.CanConfirmOrder);
            var createRes = new CreateOrderResponse(1);
            var handler = CreateHandler(member, createRes);
            var request = new OpenOrderRequest(1, 1, 1);
            var res = await handler.Handle(request);

            Assert.IsTrue(res.Success);
            Assert.AreEqual(1, res.Value.OrderId);

        }

        [Test]
        public async Task Create_Order_With_Corrent_Status()
        {
            var member = new Member(1, 1, Permission.CanConfirmOrder);
            var createRes = new CreateOrderResponse(1);
            var handler = CreateHandler(member, createRes);
            var request = new OpenOrderRequest(1, 1, 1);
            var res = await handler.Handle(request);

            Assert.IsTrue(res.Success);
            Assert.AreEqual(1, res.Value.OrderId);
            Assert.AreEqual(OrderStatusId.Approved , res.Value.Status);

        }

        OpenOrderHandler CreateHandler(Member member, CreateOrderResponse createOrderResponse = null)
        {
            var uow = new Mock<IOrderingService>();
            uow.Setup(a => a.GetMemberById(It.IsAny<int>()))
               .ReturnsAsync(member);

            uow.Setup(a => a.CreateOrder(It.IsAny<Order>())).ReturnsAsync(createOrderResponse);

            var handler = new OpenOrderHandler(uow.Object);
            return handler;
        }
    }
}
