using System;
using System.ComponentModel.DataAnnotations;

namespace CoEco.Data.EntityTypes
{
    public class BaseEntity : IBaseEntity
    {
        //[Display(Name = "BaseEntityID", ResourceType = typeof(CourseEntityesDisplayNames),Order = 0)]
        public int ID { get; set; }

        //[Display(Name = "CreatedOn", ResourceType = typeof(CourseEntityesDisplayNames), Order = 1001)]
        public DateTime CreatedOn { get; set; }

        //[Display(Name = "CreatedBy", ResourceType = typeof(CourseEntityesDisplayNames), Order = 1002)]
        public string CreatedBy { get; set; }

        //[Display(Name = "UpdatedOn", ResourceType = typeof(CourseEntityesDisplayNames), Order = 1003)]
        public DateTime UpdatedOn { get; set; }

        //[Display(Name = "UpdatedBy", ResourceType = typeof(CourseEntityesDisplayNames), Order = 1004)]
        public string UpdatedBy { get; set; }

        //[Display(Name = "Disabled", ResourceType = typeof(CourseEntityesDisplayNames), Order = 1005)]
        public bool Disabled { get; set; }
    }
}