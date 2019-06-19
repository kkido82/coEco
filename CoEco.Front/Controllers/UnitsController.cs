using CoEco.Core.Infrastructure;
using CoEco.Data.Services;
using CoEco.Front.Handlers.UnitItems;
using CoEco.Front.Helpers;
using CoEco.Front.Models.UnitItems;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
    [RoutePrefix("api/units"), Authorize]
	public class UnitsController : ApiController
	{
		private readonly AppQueryService appQueryService;
        private readonly Dispatcher dispatcher;

        public UnitsController(
            AppQueryService appQueryService,
            Dispatcher dispatcher)
		{
			this.appQueryService = appQueryService;
            this.dispatcher = dispatcher;
        }
		[Route("balance"), HttpGet]
		public async Task<IHttpActionResult> Balance()
		{
			var memberId = User.GetMemberId();
			var balance = await appQueryService.GetBalance(memberId);
			return Ok(balance);
		}

        [Route("items"), HttpGet]
        public async Task<IHttpActionResult> Items()
        {
            var memberId = User.GetMemberId();
            var res = await appQueryService.GetUnitItems(memberId);
            return Ok(res);
        }

        [Route("items"), HttpPost]
        public async Task<IHttpActionResult> UpdateItem(ManageItemModel model)
        {
            var memberId = User.GetMemberId();
            var req = new ManageItemRequest
            {
                MemberId = memberId,
                Description = model.Description,
                ItemId = model.ItemId,
                Qty = model.Qty
            };
            var res = await dispatcher.Dispatch(req);

            if (res.Success)
                return Ok();

            return BadRequest(res.Error.Description);
        }

	}
}