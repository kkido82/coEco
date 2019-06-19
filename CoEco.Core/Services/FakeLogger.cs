using CoEco.Core.Eventing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public class FakeLogger : ILogger
    {
        public void Debug(string msg)
        {
            System.Diagnostics.Debug.Print(msg);
        }

        public void Info(string msg)
        {
            System.Diagnostics.Debug.Print(msg);
        }

        public void Warn(string msg)
        {
            System.Diagnostics.Debug.Print(msg);
        }

        public void Error(string msg, Exception ex)
        {
            System.Diagnostics.Debug.Print(msg);
        }

        public void Fatal(string msg, Exception ex)
        {
            System.Diagnostics.Debug.Print(msg);
        }
    }
}
