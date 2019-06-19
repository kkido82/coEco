using CoEco.Core.Services;
using CoEco.Data.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
	[RoutePrefix("api/messages")]
	public class MessagesController : BaseApiController
	{
		private readonly AppQueryService appQuery;
        private readonly IMessagesService messagesService;

        public MessagesController(AppQueryService appQuery, IMessagesService messagesService)
		{
			this.appQuery = appQuery;
            this.messagesService = messagesService;
        }

		[HttpGet]
		public async Task<IHttpActionResult> Get()
		{
			var messages = await appQuery.GetMessages(MemberId);
			return Ok(messages);
		}

        [HttpPost, Route("read/{id}")]
        public async Task<IHttpActionResult> SetMessageRead(int id)
        {
            await messagesService.SetSent(id);
            return Ok();
        }
	}
}