using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Services.Services
{
    public interface ILoggerService
    {
        void InsertLogMessage(string type,string message, string user);
    }
}
