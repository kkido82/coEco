using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Front.Auth.Services
{
    public class SmsService : ISmsService
    {
        public Task<bool> SendMessage(string phone, string code)
        {
            return Task.FromResult(true);
        }
    }
}
