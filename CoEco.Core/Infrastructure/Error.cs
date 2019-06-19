using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Infrastructure
{
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }

        public Error(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
