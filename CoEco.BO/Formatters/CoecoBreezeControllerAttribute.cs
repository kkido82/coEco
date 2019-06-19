using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Breeze.WebApi2;
using Microsoft.SqlServer.Server;
using CoEco.BO.Formatters;

namespace CoEco.BO.Formatters
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CoecoBreezeControllerAttribute : Attribute, IControllerConfiguration
    {
        private static readonly object __lock = new object();
        private readonly BreezeControllerAttribute breezeController = new BreezeControllerAttribute();

        private readonly PrepareFileExportAttribute _excelAttribute = new PrepareFileExportAttribute();

        protected virtual IFilterProvider GetPrepareExcelExportFilterProvider(PrepareFileExportAttribute defaultFilter)
        {
            return new PrepareFileExportFilterProvider(defaultFilter);
        }

        public void Initialize(HttpControllerSettings settings, HttpControllerDescriptor descriptor)
        {
            breezeController.Initialize(settings, descriptor);
            lock (__lock)
            {
                settings.Services.Add(typeof(IFilterProvider), (object)this.GetPrepareExcelExportFilterProvider(this._excelAttribute));
                settings.Formatters.Insert(0, new FileMediaFormatter());
            }

        }
    }
}