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
    
    public partial class LendingItem : CoEco.Data.EntityTypes.IBaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LendingItem()
        {
            this.Message = new HashSet<Message>();
        }
    
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int UnitRequestsID { get; set; }
        public int UnitLendingID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int MemberID { get; set; }
        public string Remarks { get; set; }
        public int OrderStatusID { get; set; }
        public Nullable<double> RatingLendingUnit { get; set; }
        public Nullable<double> RatingRequestUnit { get; set; }
        public string ProblemDescriptionLendingUnit { get; set; }
        public string ProblemDescriptionRequestUnit { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool Disable { get; set; }
        public int Price { get; set; }
    
        public virtual Item Item { get; set; }
        public virtual Member Members { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Unit UnitRequest { get; set; }
        public virtual Unit UnitLending { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Message> Message { get; set; }
    }
}
