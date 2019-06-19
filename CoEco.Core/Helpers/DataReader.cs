using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Core.Helpers
{
    public class DataReader
    {
        public static DataTable CreateDT<T>(IEnumerable<T> items)
        {
            var dt = new DataTable();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
                dt.Columns.Add(new DataColumn(prop.Name));

            foreach (var item in items)
            {
                var row = dt.NewRow();
                foreach (var prop in props)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
