using DataLayer.WMCModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WMC_App.Models.DAL
{
    public class CustomerAPi
    {
    }
    public class ReturnComplaintAPI
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
        public Nullable<int> Likestatus { get; set; }
        public string UserFullename { get; set; }
        public string UserProfilepic { get; set; }
        public List<tbl_LikesMasterAPi> LikeList { get; set; }
       
      
    }
    public partial class tbl_LikesMasterAPi
    {
        public int pkid { get; set; }
        public Nullable<int> Master_fkid { get; set; }
        public Nullable<int> SubMaster_fkid { get; set; }
        public string User_fkid { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<System.DateTime> LastModifieddatetime { get; set; }
        public string customername { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public string Pic { get; set; }
    }
    public class LikeCustomList
    {
        public int pkid { get; set; }
        public Nullable<int> Master_fkid { get; set; }
        public Nullable<int> SubMaster_fkid { get; set; }
        public string User_fkid { get; set; }
        public Nullable<int> Likes { get; set; }
        public Nullable<System.DateTime> LastModifieddatetime { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }    
    }
    public class returnAPiFlag
    {
        public string status { get; set; }

        public bool flag { get; set; }
    }
    public class LikeMasterReturn
    {
        public string status { get; set; }

        public bool flag { get; set; }
        public Nullable<int> count { get; set; }
    }
    public class ChangePasswordAPI
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "OldPassword")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

    }
    public class RegisterViewModelAPI
    {       
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
              
        [Display(Name = "UserRoles")]
        public string UserRoles { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "MobileNumber")]
        public string MobileNumber { get; set; }

    }
    public class ChangePasswordAfterLoginAPI
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }

    }
    public class ForgotPasswordAPI
    {       
        [EmailAddress]
        public string Email { get; set; }    
        public string Mobile { get; set; }
    }
    public class Deleteuploadfile
    {
        [Required]
        public string filepath { get; set; }
    }
    public class categorSubcategoryList
    {
        public int pkid { get; set; }
        public string Category_Name { get; set; }
        public string Category_Description { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public List<tbl_Tourism_SubCategory> subCatList { get; set; }
    }
    public class TourimList
    {
        public int pkid { get; set; }
        public Nullable<int> Sub_fkid { get; set; }       
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string InchargeName { get; set; }
        public string Longitude { get; set; }
        public string Lotitude { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public Nullable<int> mid { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public Nullable<int> city_fkid { get; set; }          
        public List<tbl_MultipleFileUpload> fileupload {get;set;}         
    }
    public partial class tbl_Ward_masterAPI
    {
        public int pkid { get; set; }
        public Nullable<int> prabhag_fkid { get; set; }
        public string ward_Name { get; set; }
        public string description { get; set; }
        public string address { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
        public Nullable<int> status { get; set; }
        public List<tbl_wardMember_Master> MemberList { get; set; }     
    }
    public partial class tbl_Prabhag_MasterAPI
    {
        public int pkid { get; set; }
        public string Prabhag_Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> adddate { get; set; }
        public Nullable<int> status { get; set; }
        public List<tbl_Ward_masterAPI> WardList { get; set; }
     
    }
    public partial class ReturnCOntactListApi
    {
        public int pkid { get; set; }
        public string CatgoryName { get; set; }
        public string categoryDescription { get; set; }
        public Nullable<int> Lid { get; set; }
        public Nullable<int> md { get; set; }
        public Nullable<System.DateTime> mdate { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
        public List<tbl_Emergency_ContactUs> Contact { get; set; }
    }
}