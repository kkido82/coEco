using System.Collections.Generic;

namespace CoEco.Core.Ordering.Dto.GatewayResponses.Repositories
{
    public class CreateOrderResponse : BaseGatewayResponse
    {
        public int Id { get; }
        public CreateOrderResponse(int id) : base(true)
        {
            Id = id;
        }

        public CreateOrderResponse(string errorMessage) : base(false, errorMessage)
        {

        }
    }
}
