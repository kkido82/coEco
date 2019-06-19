using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Ordering.Dto.GatewayResponses
{
    public class BaseGatewayResponse
    {
        public bool Success { get; }
        public string ErrorMessage { get; }

        public BaseGatewayResponse(bool success = false, string errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }
    }
}
