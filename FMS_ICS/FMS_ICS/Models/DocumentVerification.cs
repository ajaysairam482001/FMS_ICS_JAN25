//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMS_ICS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentVerification
    {
        public int VerificationID { get; set; }
        public int UserID { get; set; }
        public string DocumentStatus { get; set; }
        public string Remarks { get; set; }
        public string DocumentLink { get; set; }
    
        public virtual User User { get; set; }
    }
}
