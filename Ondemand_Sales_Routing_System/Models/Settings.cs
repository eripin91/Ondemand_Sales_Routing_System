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
        public int SettingsID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public Nullable<System.TimeSpan> Scheduletime { get; set; }
        public string MessageTemplate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
        public string UserId { get; set; }
    }
}
