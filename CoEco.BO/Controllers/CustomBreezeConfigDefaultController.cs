using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CoEco.BO.Controllers
{
    public class CustomBreezeConfigDefaultController: Breeze.ContextProvider.BreezeConfig
    {
        protected override JsonSerializerSettings CreateJsonSerializerSettings()
        {
            var baseSettings = base.CreateJsonSerializerSettings();
            baseSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            return baseSettings;
        }
    }
}