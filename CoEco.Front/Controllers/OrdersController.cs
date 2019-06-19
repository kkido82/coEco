using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Handlers;
using CoEco.Data.Services;
using CoEco.Front.Handlers.ProblemReporting;
using CoEco.Front.Helpers;
using CoEco.Front.Models.Ordering;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoEco.Front.Controllers
{
    [RoutePrefix("api/orders"), Authorize]
    public class OrdersController : ApiController
    {
        private readonly Dispatcher dispatcher;
		private readonly AppQueryService queryService;

		public OrdersController(Dispatcher dispatcher, AppQueryService queryService)
        {
            this.dispatcher = dispatcher;
			this.queryService = queryService;
		}

		[HttpGet]
		public async Task<IHttpActionResult> Get(int id)
		{
			var memberId = User.GetMemberId();
			var order = await queryService.GetOrderDetails(memberId, id);
			return Ok(order);
		}

        [HttpGet, Route("new/{itemUnitId}")]
        public async Task<IHttpActionResult> NewOrder(int itemUnitId)
        {
            var memberId = User.GetMemberId();
            var order = await queryService.GetNewOrder(memberId, itemUnitId);
            return Ok(order);
        }

		[HttpGet, Route("lending-from")]
		public async Task<IHttpActionResult> LendingFrom()
		{
			var memberId = User.GetMemberId();
			var orders = await queryService.GetRequestingOrders(memberId);
			return Ok(orders);
		}


		[HttpGet, Route("lending-to")]
		public async Task<IHttpActionResult> LendingTo()
		{
			var memberId = User.GetMemberId();
			var orders = await queryService.GetLendingOrders(memberId);
			return Ok(orders);
		}


		[HttpPost]
        public async Task<IHttpActionResult> Post(CreateOrderModel model)
        {
            var memberId = User.GetMemberId();
            var request = new OpenOrderRequest(memberId, model.FromUnitId, model.ItemId, model.Remarks);
            return await Dispatch(request, async r => {
                return await queryService.GetOrderOverview(r.OrderId);
            });
        }

        [HttpPost, Route("status")]
        public async Task<IHttpActionResult> UpdateStatus(UpdateStatusModel model)
        {
            var memberId = User.GetMemberId();
            var req = CreateUpdateRequest(memberId, model);
            if (req == null)
                return BadRequest();

            return await Dispatch(req, async () => {
                return await queryService.GetOrderOverview(model.Id);
            });
        }

        [HttpPost, Route("problems")]
        public async Task<IHttpActionResult> Problems(ProblemReportModel model)
        {
            var memberId = User.GetMemberId();
            var req = new SendOrderProblemRequest(memberId, model.OrderId, model.Description);
            var res = await dispatcher.Dispatch(req);
            if (res.Success)
                return Ok();
            return BadRequest(res.Error.Description);
        }

        IRequest CreateUpdateRequest(int memberId,  UpdateStatusModel model)
        {
            switch (model.OrderStatusId)
            {
                case Core.Ordering.Domain.OrderStatusId.Approved:
                    return new ApproveRequest(model.Id, memberId);
                case Core.Ordering.Domain.OrderStatusId.Confirmed:
                    return new ConfirmOrderRequest(model.Id, memberId);
                case Core.Ordering.Domain.OrderStatusId.Active:
                    return new ActivateOrderRequest(model.Id, memberId);
                case Core.Ordering.Domain.OrderStatusId.Completed:
                    return new CompleteOrderRequest(model.Id, memberId);
                case Core.Ordering.Domain.OrderStatusId.CanceledByRequestingUnit:
                    return new CancelNewRequest(model.Id, memberId);
                case Core.Ordering.Domain.OrderStatusId.CanceledByLendingUnit:
                    return new CancelApprovedRequest(model.Id, memberId);
                default:
                    return null;
            }
        }


        async Task<IHttpActionResult> Dispatch(IRequest request)
        {
            var res = await dispatcher.Dispatch(request);
            if (res.Success)
                return Ok();

            return BadRequest(res.Error.Description);
        }

        async Task<IHttpActionResult> Dispatch<TResponse>(IRequest<TResponse> request, Func<TResponse, Task<object>> selector)
                where TResponse : class
        {
            var res = await dispatcher.Dispatch(request);
            if (res.Success) {
                var response = await selector(res.Value);
                return Ok(response);
            }

            return BadRequest(res.Error.Description);
        }

        async Task<IHttpActionResult> Dispatch(IRequest request, Func<Task<object>> selector)
        {
            var res = await dispatcher.Dispatch(request);
            if (res.Success)
            {
                var response = await selector();
                return Ok(response);
            }

            return BadRequest(res.Error.Description);
        }

    }
}
