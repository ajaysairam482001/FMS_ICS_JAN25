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
    
    public partial class EligibilityCriteria
    {
        public int CriteriaID { get; set; }
        public int CardTypeID { get; set; }
        public Nullable<decimal> MinimumIncome { get; set; }
        public Nullable<int> AgeLimit { get; set; }
    
        public virtual CardType CardType { get; set; }
    }
}
