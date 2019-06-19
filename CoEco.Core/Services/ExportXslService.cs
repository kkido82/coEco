using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using DataTable = System.Data.DataTable;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CoEco.Core.Services
{
    public class ExportXslService : IExportXslService
    {
        /// <summary>
        /// FUNCTION FOR CONVERT DATA TABLE TO EXCEL
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {
            DataTable table;
            var properties = CreateDataTable(typeof(T), out table);
            FillTableWithValues(data, table, properties);
            return table;

        }

        public DataTable ConvertToDataTable(IList<Object> data, Type itemType)
        {
            DataTable table;
            var properties = CreateDataTable(itemType, out table);
            FillTableWithValues(data, table, properties);
            return table;
        }



        /// <summary>
        /// FUNCTION FOR EXPORT TO EXCEL
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="worksheetName"></param>
        /// <param name="saveAsLocation"></param>
        /// <returns></returns>
        public bool WriteDataTableToExcel(DataTable dataTable, string worksheetName, string saveAsLocation, string reporType)
        {


            try
            {
                using (var pck = new ExcelPackage())
                {
                    AddWorksheetExcel(pck, dataTable, worksheetName);
                    pck.SaveAs(new FileInfo(saveAsLocation));

                }
                return true;
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }


        }



        public bool WriteDataTableToStream(DataTable dataTable, string worksheetName, Stream writeStream, string reportType)
        {
            try
            {
                using (var pck = new ExcelPackage())
                {
                    AddWorksheetExcel(pck, dataTable, worksheetName);
                    pck.SaveAs(writeStream);

                }
                return true;
            }
            catch (Exception ex)
            {
                OnError(ex);
                return false;
            }

        }



        private void AddWorksheetExcel(ExcelPackage pck, DataTable tbl, string workSheetName)
        {
            bool showTime = CheckIfShowTime(workSheetName);
            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add(workSheetName);

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            ws.Cells["A1"].LoadFromDataTable(tbl, true);

            //Format the header for column 1-3
            using (ExcelRange rng = ws.Cells[1, 1, 1, tbl.Columns.Count])
            {
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                rng.Style.Font.Color.SetColor(Color.White);
            }

            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                var type = tbl.Columns[i].DataType;
                if (type == typeof(DateTime))
                {
                    if (showTime)
                        ws.Column(i + 1).Style.Numberformat.Format = "yyyy-mm-dd h:mm";
                    else
                        ws.Column(i + 1).Style.Numberformat.Format = "yyyy-mm-dd";

                }

            }
            ws.Cells[ws.Dimension.Address].AutoFitColumns();



        }

        private bool CheckIfShowTime(string fileName)
        {
            if (fileName.Contains("WorkOrder"))
                return false;
            else return true;
        }

        private static void FillTableWithValues<T>(IList<T> data, DataTable table, PropertyDescriptorCollection properties)
        {
            foreach (object item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    if (!IsEnumerable(prop.PropertyType) && IsNotComplex(prop.PropertyType))
                    {
                        var dN = prop.GetDisplayName();

                        if (table.Columns.Contains(dN))
                        {
                            row[dN] = prop.GetValue(item) ?? DBNull.Value;
                        }
                    }
                table.Rows.Add(row);
            }
        }

        private static PropertyDescriptorCollection CreateDataTable(Type type, out DataTable table)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(type);
            table = new System.Data.DataTable();

            foreach (PropertyDescriptor prop in properties.Sort(new DisplaySortComparer()))
            {

                if (!IsEnumerable(prop.PropertyType) && IsNotComplex(prop.PropertyType) && prop.IsDisplayAttribute())
                {
                    table.Columns.Add(prop.GetDisplayName(), Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            return properties;
        }

        private static bool IsEnumerable(Type type)
        {
            if (type == typeof(string))
                return false;
            return (type.GetInterface("IEnumerable") != null);
        }

        private static bool IsNotComplex(Type type)
        {
            bool isPrimitiveType = type.IsPrimitive || type.IsValueType || (type == typeof(string));
            return isPrimitiveType;
        }


        protected void OnError(Exception exception)
        {
            //Logger.InsertMessagesLog(LogType.Error, exception.Message);
            throw exception;
        }


        public class DisplaySortComparer : IComparer<PropertyDescriptor>, IComparer
        {
            public int Compare(object x, object y)
            {
                return Compare((PropertyDescriptor)x, (PropertyDescriptor)(y));
            }

            public int Compare(PropertyDescriptor x, PropertyDescriptor y)
            {
                return x.GetDisplayOrder() - y.GetDisplayOrder();
            }
        }



    }
}
