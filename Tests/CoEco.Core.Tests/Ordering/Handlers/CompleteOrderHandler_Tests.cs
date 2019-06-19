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
    public class CompleteOrderHandler_Tests
    {
        [Test]
        public async Task Can_Complete()
        {
            var res = await Run(new SetupData
            {
                UnitItemAmount = 99,
                MemberPermissions = new Permission[] { Permission.CanUpdateInventory },
                MemberSide = OrderSide.Lending,
                StartingStatus = OrderStatusId.Active,
            });

            Assert.IsTrue(res.Success);
            Assert.AreEqual(100, res.Value.AffectedUnitItem.Amount);
        }

        [Test]
        public async Task Only_Lending_Unit_member_Can_Complete()
        {
            foreach (OrderSide side in Enum.GetValues(typeof(OrderSide)))
            {
                var res = await Run(new SetupData
                {
                    UnitItemAmount = 99,
                    MemberPermissions = new Permission[] { Permission.CanUpdateInventory },
                    MemberSide = side,
                    StartingStatus = OrderStatusId.Active,
                });

                var expected = side == OrderSide.Lending;
                Assert.AreEqual(expected, res.Success);
            }


        }

        [Test]
        public async Task Cannot_Activate_Without_Permission()
        {

            foreach (Permission permission in Enum.GetValues(typeof(Permission)))
            {
                var res = await Run(new SetupData
                {
                    UnitItemAmount = 99,
                    MemberPermissions = new Permission[] { permission },
                    MemberSide = OrderSide.Lending ,
                    StartingStatus = OrderStatusId.Active,
                });

                var expected = permission == Permission.CanUpdateInventory;

                Assert.AreEqual(expected, res.Success);
            }
        }

        [Test]
        public async Task Can_Complete_Only_When_Active()
        {
            foreach (OrderStatusId status in Enum.GetValues(typeof(OrderStatusId)))
            {
                var data = new SetupData
                {
                    UnitItemAmount = 99,
                    MemberPermissions = new Permission[] { Permission.CanUpdateInventory },
                    MemberSide = OrderSide.Lending,
                    StartingStatus = status,
                };

                var res = await Run(data);
                var expected = status == OrderStatusId.Active;
                Assert.AreEqual(expected, res.Success);
            }
        }

        async Task<Result<OrderCompleted>> Run(SetupData data)
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
            orderingService.Setup(a => a.Update(It.IsAny<UnitItem>()))
                .ReturnsAsync(new BaseGatewayResponse(true));
            orderingService.Setup(a => a.GetItem(1)).ReturnsAsync(item);
            orderingService.Setup(a => a.GetUnitItem(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new UnitItem
                {
                    UnitId = order.LendingUnitId,
                    Amount = data.UnitItemAmount,
                    ItemId = order.ItemId
                });
            orderingService.Setup(a => a.TransterMoney(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new BaseGatewayResponse(true));
            var handler = new CompleteOrderHandler(orderingService.Object);

            var request = new CompleteOrderRequest(1, 2);

            return await handler.Handle(request);
        }

        class SetupData
        {
            public OrderStatusId StartingStatus { get; set; }
            public int UnitItemAmount { get; set; }
            public OrderSide MemberSide { get; set; }
            public Permission[] MemberPermissions { get; set; }
        }
    }
}
