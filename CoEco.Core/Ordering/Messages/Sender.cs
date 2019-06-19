using CoEco.Core.Infrastructure;
using CoEco.Core.Ordering.Domain;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Core.Ordering.Handlers;
using CoEco.Core.Ordering.Repositories;
using CoEco.Core.Services;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Messages
{
    public abstract class SendTarget { }
    public class OrderOpener : SendTarget { }
    public class MemberTarget : SendTarget
    {
        public MemberTarget(OrderSide orderSide, Permission permission)
        {
            OrderSide = orderSide;
            Permission = permission;
        }

        public OrderSide OrderSide { get; }
        public Permission Permission { get; }
    }

    public enum MessageType
    {
        OrderOpened,
        OrderApproved,
        OrderCanceledByRequestingUnit,
        OrderCanceledByLendingUnit,
        OrderConfirmed,
        OrderActivated,
        OrderCompleted
    }

    public class Sent
    {
        public Sent(int[] memberIds, string content = "")
        {
            MemberIds = memberIds;
            Content = content;
        }

        public int[] MemberIds { get; }
        public string Content { get; }
    }

    public class SendErrors
    {
        public static Error NoMembersFound = new Error("messages_sender_no_members", "no members found");
        public static Error FailedToAddMessages = new Error("messages_failed_to_add", "failed to add messages");
    }

    public class MessageSender
    {
        private readonly IOrderingService orderingService;
        private readonly IMessagesService messagesService;

        public MessageSender(IOrderingService orderingService,
            IMessagesService messagesService)
        {
            this.orderingService = orderingService;
            this.messagesService = messagesService;
        }

        public async Task<Result<Sent>> Send(int orderId, SendTarget target, MessageType messageType)
        {
            try
            {
                var memberIds = await GetMembers(orderId, target);
                if (memberIds.Length == 0)
                    return SendErrors.NoMembersFound;

                var content = GetMessageText(messageType);

                var requests = memberIds.Select(memberId => new AddMessageRequest(content, memberId, orderId)).ToArray();

                var success = await messagesService.AddMessages(requests);
                if (!success)
                    return SendErrors.FailedToAddMessages;

                return new Sent(memberIds, content);
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        static string GetMessageText(MessageType type)
        {
            switch (type)
            {
                case MessageType.OrderOpened:
                    return Messages.OrderOpened;
                case MessageType.OrderApproved:
                    return Messages.OrderApproved;
                case MessageType.OrderCanceledByRequestingUnit:
                    return Messages.OrderCanceledByRequestingUnit;
                case MessageType.OrderCanceledByLendingUnit:
                    return Messages.OrderCanceledByLendingUnit;
                case MessageType.OrderConfirmed:
                    return Messages.OrderConfirmed;
                case MessageType.OrderActivated:
                    return Messages.OrderActivated;
                case MessageType.OrderCompleted:
                    return Messages.OrderCompleted;
                default:
                    return "";
            }
        }

        async Task<int[]> GetMembers(int orderId, SendTarget target)
        {
            var order = await orderingService.GetOrder(orderId);
            switch (target)
            {
                case OrderOpener _: return new[] { order.RequestingMemberId };
                case MemberTarget mt:
                    var unitId = mt.OrderSide == OrderSide.Lending ? order.LendingUnitId : order.RequestingUnitId;
                    var members = await orderingService.GetMembers(unitId, mt.Permission);
                    return members.Select(a => a.Id).ToArray();
            }

            return new int[0];
        }
    }
}
