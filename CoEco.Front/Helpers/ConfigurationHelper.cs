using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CoEco.Front.Helpers
{
    public class ConfigurationHelper
    {
        public static T Get<T>(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            var type = typeof(T);
            return CanChangeType(value, type)
                ? (T)Convert.ChangeType(value, typeof(T))
                : default(T);

        }

        static bool CanChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                return false;
            }

            if (value == null)
            {
                return false;
            }

            IConvertible convertible = value as IConvertible;

            if (convertible == null)
            {
                return false;
            }

            return typeof(IConvertible).IsAssignableFrom(conversionType);
        }
    }
}