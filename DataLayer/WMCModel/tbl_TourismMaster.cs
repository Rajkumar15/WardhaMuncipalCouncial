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
    
    public partial class tbl_TourismMaster
    {
        public int pkid { get; set; }
        public Nullable<int> Sub_fkid { get; set; }
        public Nullable<int> city_fkid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string InchargeName { get; set; }
        public string Longitude { get; set; }
        public string Lotitude { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<System.TimeSpan> openingTime { get; set; }
        public Nullable<System.TimeSpan> ClosingTime { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
}
