using CoEco.Core.Services;
using CoEco.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly CoEcoEntities db;

        public MessagesService(CoEcoEntities db)
        {
            this.db = db;
        }
        public async Task<bool> AddMessages(AddMessageRequest[] requests)
        {
            var messages = requests.Select(req => new Message
            {
                Content = req.Content,
                MemberID = req.MemberId,
                OrderID = req.OrderId
            });

            db.Messages.AddRange(messages);

            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task SetSent(int id)
        {
            var message = await db.Messages.FindAsync(id);
            message.OpeningDate = DateTime.Now;
            await db.SaveChangesAsync();
        }
    }
}
