using CoEco.Core.Eventing;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Messages;
using CoEco.Core.Ordering.Repositories;
using CoEco.Core.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Core.Tests.Ordering.Messages
{
    [TestFixture]
    public class Messages_Tests
    {
        const int orderId = 1;
        const int requesting_openerMemberId = 1;
        const int requesting_ConfirmMemberId = 2;
        const int lending_UpdateInventoryMemberId = 4;
        const int lending_ConfirmMemberId = 3;

        readonly Member[] members = new Member[]
        {
            new Member(requesting_openerMemberId,1, Permission.CanOpenAnOrder),
            new Member(requesting_ConfirmMemberId,1, Permission.CanConfirmOrder),
            new Member(lending_UpdateInventoryMemberId,2, Permission.CanUpdateInventory),
            new Member(lending_ConfirmMemberId,2, Permission.CanConfirmOrder),
        };

        readonly Order order = new Order
        {
            Id = 1,
            LendingUnitId = 2,
            RequestingMemberId = 1,
            RequestingUnitId = 1
        };

        [Test]
        public async Task When_Order_Opened_Without_Approval_Message_Should_Be_Sent_To_Confirm_Member_On_Requesting_Unit()
        {
            var ev = new OrderOpened(orderId, OrderStatusId.New);
            await Run(ev, requesting_ConfirmMemberId);
        }

        [Test]
        public async Task When_Order_Opened_With_Approval_Message_Should_Be_Sent_To_Confirm_Member_On_Lending_Unit()
        {
            var ev = new OrderOpened(orderId, OrderStatusId.Approved);
            await Run(ev, lending_ConfirmMemberId);
        }


        [Test]
        public async Task When_Canceled_By_Lending_Unit_Message_Should_Be_Sent_To_Opener()
        {
            var ev = new CanceledByLendingUnit(orderId, 1);
            await Run(ev, requesting_openerMemberId);
        }

        [Test]
        public async Task When_Canceled_By_Requesting_Unit_Message_Should_Be_Sent_To_Opener()
        {
            var ev = new CanceledByRequestingUnit(orderId, 1);
            await Run(ev, requesting_openerMemberId);
        }

        [Test]
        public async Task When_Order_Approved_Message_Should_Be_Sent_To_Confirm_Member_On_Lending_Unit()
        {
            var ev = new Approved(orderId, 1);
            await Run(ev, lending_ConfirmMemberId);
        }

        [Test]
        public async Task When_Order_Confirmed_Message_Should_Be_Sent_To_Opener()
        {
            var ev = new OrderConfirmed(orderId, 1, 0);
            await Run(ev, requesting_openerMemberId);
        }

        [Test]
        public async Task When_Order_Activated_Message_Should_Be_Sent_To_Inventory_Member_On_Lending_Unit()
        {
            var ev = new OrderActivated(orderId, 1);
            await Run(ev, lending_UpdateInventoryMemberId);
        }

        [Test]
        public async Task When_Order_Completed_Message_Should_Be_Sent_To_Opener()
        {
            var ev = new OrderCompleted(orderId, 1, new UnitItem());
            await Run(ev, requesting_openerMemberId);
        }


        async Task Run<T>(T ev, int expectedMemberId)
            where T : class
        {
            var orderingService = new Mock<IOrderingService>();
            orderingService
                .Setup(a => a.GetOrder(1))
                .ReturnsAsync(order);

            orderingService
                .Setup(a => a.GetMembers(It.IsAny<int>(), It.IsAny<Permission>()))
                .ReturnsAsync((int unitId, Permission permission) =>
                {
                    return members
                        .Where(m => m.UnitId == unitId && m.HasPermission(permission))
                        .ToArray();
                });

            var messagesToAdd = new List<AddMessageRequest>();
            var messagesService = new Mock<IMessagesService>();
            messagesService.Setup(a => a.AddMessages(It.IsAny<AddMessageRequest[]>()))
                .Callback((AddMessageRequest[] requests) => messagesToAdd.AddRange(requests))
                .ReturnsAsync(true);

            var sender = new MessageSender(orderingService.Object, messagesService.Object);
            var consumer = new MessagesConsumer(sender);

            var tConsumer = consumer as IAsyncConsumer<T>;
            await tConsumer.HandleEventAsync(ev);

            Assert.GreaterOrEqual(1, messagesToAdd.Count);
            var first = messagesToAdd.First();
            Assert.AreEqual(expectedMemberId, first.MemberId);

        }
    }
}
