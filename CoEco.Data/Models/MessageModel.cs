using System;

namespace CoEco.Data.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
		public int OrderId { get; set; }
		public DateTime Date { get; set; }
        public string Title { get; set; }
    }
}
