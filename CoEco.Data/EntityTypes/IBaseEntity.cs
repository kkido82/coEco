using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoEco.Data.EntityTypes
{
    public interface IBaseEntity
    {
        System.DateTime CreatedOn { get; set; }
        string CreatedBy { get; set; }
        System.DateTime UpdatedOn { get; set; }
        string UpdatedBy { get; set; }
    }
}
