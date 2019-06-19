using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Services.Models
{
    public class CoecoErrorMemberBulk
    {
        public int ID { get; set; }
        public string TZ { get; set; }
        public string FilesUploadName { get; set; }
        public string ErrorMsg { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Disable { get; set; }
    }
}
