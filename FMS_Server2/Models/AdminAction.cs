//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMS_Server2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdminAction
    {
        public int ActionID { get; set; }
        public int AdminID { get; set; }
        public string ActionType { get; set; }
        public string ActionDetails { get; set; }
        public Nullable<System.DateTime> ActionDate { get; set; }
    
        public virtual Admin Admin { get; set; }
    }
}
