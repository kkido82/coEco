//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoEco.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ItemToUnit : CoEco.Data.EntityTypes.IBaseEntity
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitID { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Disable { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
