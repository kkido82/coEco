using CoEco.Data.Models;
using CoEco.Data.Services;
using CoEco.Front.Helpers;
using CoEco.Front.Models.Items;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
    [RoutePrefix("api/items"), Authorize]
    public class ItemsController : ApiController
    {
        private readonly AppQueryService queryService;

        public ItemsController(AppQueryService queryService)
        {
            this.queryService = queryService;
        }

		[HttpGet]
        public Task<ItemModel[]> Get() => queryService.GetItems();

		[HttpPost, Route("search")]
		public async Task<IHttpActionResult> Search([FromBody]SearchItemModel model)
		{
			var ids = model.Ids;
			var memberId = User.GetMemberId();
			var models = await queryService.SearchUnitItems(memberId, ids);
			return Ok(models);
		}

    }
}