using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.GatewayResponses;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CoEco.Core.Tests.Ordering.Handlers
{
    [TestFixture]
    public class ConfirmHandler_Tests
    {
        [Test]
        public async Task Can_Confirm()
        {
            var res = await Run(new SetupData
            {
                ItemPrice = 100,
                MemberPermissions = new Permission[] { Permission.CanConfirmOrder },
                MemberSide = OrderSide.Lending,
                StartingStatus = OrderStatusId.Approved
            });

            Assert.IsTrue(res.Success);
            Assert.AreEqual(100, res.Value.TransferAmount);
        }

        [Test]
        public async Task Cannot_Approve_Without_Permission()
        {
            var res = await Run(new SetupData
            {
                ItemPrice = 100,
                MemberPermissions = new Permission[] {  },
                MemberSide = OrderSide.Lending,
                StartingStatus = OrderStatusId.Approved
            });
            Assert.IsFalse(res.Success);
        }

        [Test]
        public async Task Only_Requesting_Unit_member_Can_Approve()
        {
            var res = await Run(new SetupData
            {
                ItemPrice = 100,
                MemberPermissions = new Permission[] { Permission.CanConfirmOrder },
                MemberSide = OrderSide.Requesting,
                StartingStatus = OrderStatusId.Approved
            });

            Assert.IsFalse(res.Success);
        }

        [Test]
        public async Task Can_Confirm_Only_When_Approved()
        {
            foreach (OrderStatusId status in Enum.GetValues(typeof(OrderStatusId)))
            {
                var data = new SetupData
                {
                    ItemPrice = 100,
                    MemberPermissions = new Permission[] { Permission.CanConfirmOrder },
                    MemberSide = OrderSide.Lending,
                    StartingStatus = status
                };

                var res = await Run(data);
                var expected = status == OrderStatusId.Approved;
                Assert.AreEqual(expected, res.Success);
            }
        }

        async Task<Result<OrderConfirmed>> Run(SetupData data)
        {
            var order = new Order
            {
                Id = 1,
                ItemId = 1,
                LendingUnitId = 2,
                RequestingMemberId = 1,
                RequestingUnitId = 3,
                Status = data.StartingStatus
            };

            var unitId = data.MemberSide == OrderSide.Lending ? order.LendingUnitId : order.RequestingUnitId;
            var member = new Member(2, unitId, data.MemberPermissions);
            var item = new Item
            {
                Id = 1,
                Price = 100
            };
            var orderingService = new Mock<IOrderingService>();
            orderingService.Setup(a => a.GetOrder(1)).ReturnsAsync(order);
            orderingService.Setup(a => a.GetMemberById(2)).ReturnsAsync(member);
            orderingService.Setup(a => a.UpdateOrder(It.IsAny<Order>()))
                .ReturnsAsync(new BaseGatewayResponse(true));
            orderingService.Setup(a => a.GetItem(1)).ReturnsAsync(item);
            orderingService.Setup(a => a.TransterMoney(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BaseGatewayResponse(true));
            var handler = new ConfirmHandler(orderingService.Object);

            var request = new ConfirmOrderRequest(1, 2);

            return await handler.Handle(request);


        }


        class SetupData
        {
            public OrderStatusId StartingStatus { get; set; }
            public double ItemPrice { get; set; }
            public OrderSide MemberSide { get; set; }
            public Permission[] MemberPermissions { get; set; }
        }

        class CanResult
        {
            public bool Success { get; set; }
            public double AmountTransferd { get; set; }
        }

    }
}
