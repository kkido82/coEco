using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.ExcelUploadFiles.Helper
{
    public class FileHandleDetails
    {
        public string FileReportPath { get; set; }
        public string FileUploadPath { get; set; }
        public string FileUploadMsg { get; set; }
        public bool FileContentOk { get; set; }
        public bool IsErrorsSavedInList { get; set; }
        public string ExcelErrorPath { get; set; }
    }
}