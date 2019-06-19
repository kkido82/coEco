using CoEco.Data.Models;
using System;

namespace CoEco.Front.Models.Ordering
{
    public class ProblemReportMailModel
    {
        public int OrderId { get; set; }
        public string ItemName { get; set; }
        public string ReportingMember { get; set; }
        public int MemberUnitId { get; set; }
        public string Description { get; set; }
        public string FromUnit { get; set; }
        public string ToUnit { get; set; }
        public string CurrentStatus { get; set; }
        public DateTime ReportDate { get; set; }
        public DateTime OrderCreatedDate { get; set; }

    }
}