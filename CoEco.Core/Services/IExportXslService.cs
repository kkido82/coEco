using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Services
{
    public interface IExportXslService
    {
        /// <summary>
        /// FUNCTION FOR CONVERT DATA TABLE TO EXCEL
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        DataTable ConvertToDataTable<T>(IList<T> data);

        /// <summary>
        /// FUNCTION FOR EXPORT TO EXCEL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <returns></returns>
        bool WriteDataTableToExcel(DataTable dataTable, string worksheetName, string saveAsLocation, string reporType);


        bool WriteDataTableToStream(DataTable dataTable, string worksheetName, Stream writeStream, string reportType);


        DataTable ConvertToDataTable(IList<Object> data, Type itemType);
    }
}
