using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public interface IExportableResolver
    {
        bool CanConvert(Type type);

        IEnumerable<object> Convert(object source);
    }
}
