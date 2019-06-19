using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public class ExportableResolver : IExportableResolver
    {
        public static Lazy<ExportableResolver> Instance = new Lazy<ExportableResolver>(() => new ExportableResolver());


        private readonly Dictionary<Type, IExportableConverter> _exportableConvertersDictionary = new Dictionary<Type, IExportableConverter>();

        private ExportableResolver()
        {

        }


        public bool CanConvert(Type type)
        {
            return _exportableConvertersDictionary.ContainsKey(type);
        }

        public IEnumerable<object> Convert(object source)
        {
            var type = source.GetType();
            if (!CanConvert(type))
                throw new NotSupportedException("Dont have converter for " + type.Name);

            return _exportableConvertersDictionary[type].Convert(source);
        }

        public void Register(IExportableConverter converter)
        {
            if (!_exportableConvertersDictionary.ContainsKey(converter.SupportedType))
            {
                _exportableConvertersDictionary.Add(converter.SupportedType, converter);
            }
        }
    }
}
