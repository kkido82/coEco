using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Dto.Events
{
    public class OrderCreated
    {
        private readonly int orderId;

        public OrderCreated(int orderId)
        {
            this.orderId = orderId;
        }
    }
}
