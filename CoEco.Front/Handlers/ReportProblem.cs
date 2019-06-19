using CoEco.Core.Infrastructure;
using CoEco.Core.Interfaces;
using CoEco.Core.Ordering.Dto.Responses;
using CoEco.Data.Models;
using CoEco.Data.Services;
using CoEco.Services.Services;
using System.Linq;
using System.Threading.Tasks;
using CoEco.Data;
using static CoEco.Core.Infrastructure.GeneralErrors;
using CoEco.Front.Models.Ordering;
using CoEco.Front.Helpers;
using System;

namespace CoEco.Front.Handlers.ProblemReporting
{

    public class SendOrderProblemRequest : IRequest<OrderProblemSent>
    {
        public SendOrderProblemRequest(int memberId, int orderId, string description)
        {
            MemberId = memberId;
            OrderId = orderId;
            Description = description;
        }

        public int MemberId { get; }
        public int OrderId { get; }
        public string Description { get; }
    }

    public class OrderProblemSent
    {

    }

    public class SendOrderProblemHandler : IHandler<SendOrderProblemRequest, OrderProblemSent>
    {
        private readonly AppQueryService appQueryService;
        private readonly IMailService mailService;

        public SendOrderProblemHandler(
            AppQueryService appQueryService,
            IMailService mailService)
        {
            this.appQueryService = appQueryService;
            this.mailService = mailService;
        }
        public async Task<Result<OrderProblemSent>> Handle(SendOrderProblemRequest request)
        {
            var member = await appQueryService.GetMember(request.MemberId);
            var order = await appQueryService.GetOrderOverview(request.OrderId);

            var error = Validate(member, order);
            if (error != null)
                return error;


            var solvers = await appQueryService.GetProblemSolvers();
            if (solvers.Length == 0)
            {
                return new OrderProblemSent();
            }

            var mailBody = GenerateMailBody(member, order, request.Description);
            foreach (var solver in solvers)
            {
                var mailRequest = new MailRequest
                {
                    Body = mailBody,
                    From = "dev@ngsoft.com",
                    IsHtml =true,
                    Subject = $"נפתחה בעייה כבור הזמנה {order.Id}",
                    To = solver.Email
                };
                mailService.Send(mailRequest);
            }


            return new OrderProblemSent();
        }

        void SendMail()
        {

        }

        string GenerateMailBody(Member member, OrderOverview order, string desc)
        {
            var vm = new ProblemReportMailModel
            {
                CurrentStatus = order.StatusName,
                Description = desc,
                FromUnit = order.FromUnit,
                ItemName = order.ItemName,
                OrderCreatedDate = order.OrderDate,
                MemberUnitId = member.UnitID,
                OrderId = order.Id,
                ReportDate = DateTime.Now,
                ReportingMember = $"{member.FirstName} {member.LastName}",
                ToUnit = order.ToUnit
            };

            var html = FakeController.RenderViewToString("Mail", "OrderProblem", vm);
            return html;
        }


        Error Validate(Member member, OrderOverview order)
        {
            if (member == null || order == null)
                return NotFound();

            var alllowedUnits = new[] { order.FromUnitId, order.ToUnitId };
            if (!alllowedUnits.Contains(member.UnitID))
                return UnauthorizedError();

            return null;
        }
    }

}