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
    
    public partial class Item : CoEco.Data.EntityTypes.IBaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            this.ItemToUnit = new HashSet<ItemToUnit>();
            this.LendingItem = new HashSet<LendingItem>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int Cost { get; set; }
        public int IconStoreID { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Disable { get; set; }
    
        public virtual IconStore IconStore { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemToUnit> ItemToUnit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LendingItem> LendingItem { get; set; }
    }
}
