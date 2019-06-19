using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public interface IExportableConverter
    {
        Type SupportedType { get; }

        IEnumerable<object> Convert(object source);
    }
}
