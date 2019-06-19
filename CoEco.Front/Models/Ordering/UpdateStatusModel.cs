using CoEco.Core.Ordering.Domain;

namespace CoEco.Front.Models.Ordering
{
    public class UpdateStatusModel
	{
		public int Id { get; set; }
		public OrderStatusId OrderStatusId { get; set; }
	}

}