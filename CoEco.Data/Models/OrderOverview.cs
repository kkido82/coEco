using System;

namespace CoEco.Data.Models
{
    public class OrderOverview
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int FromUnitId { get; set; }
        public string FromUnit { get; set; }
        public int ToUnitId { get; set; }
        public string ToUnit { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
