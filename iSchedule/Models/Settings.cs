//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iSchedule.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Settings
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Settings()
        {
            this.AspNetUsers = new HashSet<AspNetUsers>();
        }
    
        public int SettingsID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public Nullable<System.TimeSpan> Scheduletime { get; set; }
        public string MessageTemplate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUsers> AspNetUsers { get; set; }
    }
}
