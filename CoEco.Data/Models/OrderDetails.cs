using System;

namespace CoEco.Data.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string FromUnit { get; set; }
        public int FromUnitId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
        public int Cost { get; set; }
        public double Distance { get; set; }
        public string ItemDescription { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonPhone { get; set; }
        public string Remarks { get; set; }
		public int[] Actions { get; internal set; }
	}
}
