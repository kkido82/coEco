namespace CoEco.Core.Ordering.Domain
{
	public class Order
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public OrderStatusId Status { get; set; }
        public int RequestingMemberId { get; set; }
        public int RequestingUnitId { get; set; }
        public int LendingUnitId { get; set; }
        public string Remarks { get; set; }
        public int Price { get; set; }
	}
}
