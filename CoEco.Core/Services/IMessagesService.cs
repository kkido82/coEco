using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public interface IMessagesService
    {
        Task<bool> AddMessages(AddMessageRequest[] request);
        Task SetSent(int id);
    }

    public class AddMessageRequest
    {
        public AddMessageRequest(string content, int memberId, int orderId)
        {
            Content = content;
            MemberId = memberId;
            OrderId = orderId;
        }

        public string Content { get; }
        public int MemberId { get; }
        public int OrderId { get; }
    }

}
