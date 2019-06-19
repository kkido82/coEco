using CoEco.Core.Eventing;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Messages
{
    public class MessagesConsumer : 
        IAsyncConsumer<OrderOpened>,
        IAsyncConsumer<CanceledByRequestingUnit>,
        IAsyncConsumer<CanceledByLendingUnit>,
        IAsyncConsumer<Approved>,
        IAsyncConsumer<OrderConfirmed>,
        IAsyncConsumer<OrderActivated>,
        IAsyncConsumer<OrderCompleted>
    {
        private readonly MessageSender sender;

        public MessagesConsumer(MessageSender sender)
        {
            this.sender = sender;
        }

        public async Task HandleEventAsync(OrderOpened ev)
        {
            
            if (ev.Status == OrderStatusId.New)
            {
                var target = new MemberTarget(OrderSide.Requesting, Permission.CanConfirmOrder);
                await sender.Send(ev.OrderId, target, MessageType.OrderOpened);
            } else if(ev.Status == OrderStatusId.Approved)
            {
                var target = new MemberTarget(OrderSide.Lending, Permission.CanConfirmOrder);
                await sender.Send(ev.OrderId, target, MessageType.OrderApproved);
            }
        }

        public async Task HandleEventAsync(CanceledByLendingUnit ev)
        {
            var target = new OrderOpener();
            await sender.Send(ev.OrderId, target, MessageType.OrderCanceledByLendingUnit);
        }

        public async Task HandleEventAsync(CanceledByRequestingUnit ev)
        {
            var target = new OrderOpener();
            await sender.Send(ev.OrderId, target, MessageType.OrderCanceledByRequestingUnit);
        }

        public async Task HandleEventAsync(Approved ev)
        {
            var target = new MemberTarget(OrderSide.Lending, Permission.CanConfirmOrder);
            await sender.Send(ev.OrderId, target, MessageType.OrderApproved);

        }

        public async Task HandleEventAsync(OrderConfirmed ev)
        {
            var target = new OrderOpener();
            await sender.Send(ev.OrderId, target, MessageType.OrderConfirmed);
        }

        public async Task HandleEventAsync(OrderActivated ev)
        {
            var target = new MemberTarget(OrderSide.Lending, Permission.CanUpdateInventory);
            await sender.Send(ev.OrderId, target, MessageType.OrderActivated);
        }

        public async Task HandleEventAsync(OrderCompleted ev)
        {
            var target = new OrderOpener();
            await sender.Send(ev.OrderId, target, MessageType.OrderCompleted);
        }


    }
}
