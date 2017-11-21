using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMC_App.Models.DAL
{
    public partial class tbl_Category_Complaintss
    {
        public int pkid { get; set; }
        public string CategoryName { get; set; }
        public Nullable<System.DateTime> LastModifedDate { get; set; }
        public string Description { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_ComplaintMasterss
    {
        public int pkid { get; set; }
        public string User_fkid { get; set; }
        public Nullable<int> Category_fkid { get; set; }
        public string ComplaintName { get; set; }
        public string ComplaintDescription { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> Active { get; set; }
        public string Longitutde { get; set; }
        public string Latitude { get; set; }
        public Nullable<int> City_fkid { get; set; }
        public string ShowIdentity { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
  
    public partial class tbl_NoticeBoardMasterss
    {
        public int pkid { get; set; }
        public string NoticeName { get; set; }
        public string NoticeDescription { get; set; }
        public string Image_Path { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> RequiredDays { get; set; }
        public Nullable<int> Active { get; set; }
        public Nullable<int> City_fkid { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_LikesMasterss
    {
        public int pkid { get; set; }
        public Nullable<int> Master_fkid { get; set; }
        public Nullable<int> SubMaster_fkid { get; set; }
        public string User_fkid { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<System.DateTime> LastModifieddatetime { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public string customername { get; set; }
    }
    public partial class tbl_Tourism_Categoryss
    {
        public int pkid { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_Tourism_SubCategoryss
    {
        public int pkid { get; set; }
        public Nullable<int> Category_fkid { get; set; }
        public string SubCategory_Name { get; set; }
        public string SubCategory_Description { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_TourismMasterss
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
    public partial class tbl_UserDetailsss
    {
        public int pkid { get; set; }
        public string User_fkid { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string ContactNumber { get; set; }
        public string EmailId { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> cid { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public string ProdilePic { get; set; }
    }
    public partial class tbl_NoticeBoardMastersss
    {
        public int pkid { get; set; }
        public string NoticeName { get; set; }
        public string NoticeDescription { get; set; }
        public string Image_Path { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<int> RequiredDays { get; set; }
        public Nullable<int> Active { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public partial class tbl_MultipleFileUploadss
    {
        public int pkid { get; set; }
        public Nullable<int> Master_fkid { get; set; }
        public Nullable<int> SubMaster_fkid { get; set; }
        public string filepath { get; set; }
        public Nullable<System.DateTime> Lastmodifieddatetime { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
    }
    public class FileModel
    {        
        public HttpPostedFileBase[] files { get; set; }
    }
    public partial class tbl_EmergencyContactCategoryss
    {
        public int pkid { get; set; }
        public string CatgoryName { get; set; }
        public string categoryDescription { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> md { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
    }
    public partial class tbl_Emergency_ContactUsss
    {
        public int pkid { get; set; }
        public string DeptName { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Category_fkid { get; set; }
    }
}