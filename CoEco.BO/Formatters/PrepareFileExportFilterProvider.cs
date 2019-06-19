using CoEco.BO.Formatters;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CoEco.BO.Formatters
{
    public class PrepareFileExportFilterProvider : IFilterProvider
    {
        private readonly PrepareFileExportAttribute _prepareExcelExportAttribute;
        public PrepareFileExportFilterProvider(PrepareFileExportAttribute prepareExcelExportAttribute)
        {
            _prepareExcelExportAttribute = prepareExcelExportAttribute;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            return new FilterInfo[]
            {
                new FilterInfo(_prepareExcelExportAttribute, FilterScope.Global)
            };
        }
    }
}