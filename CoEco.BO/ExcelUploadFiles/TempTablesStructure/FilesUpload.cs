using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.BO.ExcelUploadFiles.TempTablesStructure
{
    public class FilesUpload
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public bool Delete { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Disable { get; set; }
    }
}