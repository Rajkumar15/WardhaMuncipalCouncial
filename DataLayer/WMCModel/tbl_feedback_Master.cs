//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer.WMCModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_feedback_Master
    {
        public int pkid { get; set; }
        public Nullable<int> Complaint_fkid { get; set; }
        public Nullable<int> rateAmt { get; set; }
        public string description { get; set; }
        public string user_fkid { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
