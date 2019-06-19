using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Data.Models
{
    public class RequestOrder
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
