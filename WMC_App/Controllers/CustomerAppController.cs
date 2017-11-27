using DataLayer.WMCModel;
using WMC_App.Models.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using WMC_App.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using WMC_App.Models.BAL;

namespace WMC_App.Controllers
{
    [Authorize]
    public class CustomerAppController : ApiController
    {
        WMC_WebApplicationEntities db = new WMC_WebApplicationEntities();
        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterViewModel model)
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                if (model.Email != null || model.MobileNumber != null)
                {

                    int co = 0;
                    if (ModelState.IsValid)
                    {
                        if (!string.IsNullOrEmpty(model.Email))
                        {
                            int mobdt = db.tbl_UserDetails.Where(x => x.EmailId == model.Email).Count();
                            if (mobdt > 0)
                            {
                                abc.status = "Email Already Taken";
                                abc.flag = false;
                                return Json(abc);
                            }
                        }
                        if (!string.IsNullOrEmpty(model.MobileNumber))
                        {
                            int mobdt = db.tbl_UserDetails.Where(x => x.ContactNumber == model.MobileNumber).Count();
                            if (mobdt > 0)
                            {
                                abc.status = "Mobile Already Taken";
                                abc.flag = false;
                                return Json(abc);
                            }
                        }
                        string UUNAME = "";
                        if (string.IsNullOrEmpty(model.Email))
                        {
                            UUNAME = model.MobileNumber;
                        }
                        else
                        {
                            UUNAME = model.Email;
                        }
                        if (!string.IsNullOrEmpty(model.Email)) { co = co + db.tbl_UserDetails.Where(x => x.EmailId.ToLower() == model.Email.ToLower()).Count(); }
                        if (!string.IsNullOrEmpty(model.MobileNumber)) { co = co + db.tbl_UserDetails.Where(x => x.ContactNumber.Trim() == model.MobileNumber.Trim()).Count(); }
                        if (co > 0)
                        {
                            abc.status = "Mobile Or Email Already Taken";
                            abc.flag = false;
                            return Json(abc);
                        }
                        var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var user = new ApplicationUser { UserName = UUNAME, Email = model.Email };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            //  await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                            var currentUser = UserManager.FindByName(user.UserName);
                            var roleresult = UserManager.AddToRole(currentUser.Id, model.UserRoles);
                            tbl_UserDetails asd = new tbl_UserDetails();
                            asd.User_fkid = currentUser.Id;
                            asd.UserName = user.UserName;
                            asd.RoleName = model.UserRoles;
                            asd.EmailId = model.Email;
                            asd.FullName = model.FullName;
                            asd.ContactNumber = model.MobileNumber;
                            asd.RegisteredDate = DateTime.Now;
                            asd.LastModifiedDate = DateTime.Now;
                            db.tbl_UserDetails.Add(asd);
                            db.SaveChanges();
                        }
                        if (result.Succeeded == true)
                        {
                            abc.status = "suceess";
                            abc.flag = true;
                        }
                        else
                        {
                            abc.status = "UserName Already Taken";
                            abc.flag = false;
                        }

                    }
                    else
                    {
                        abc.status = "Model State Are Invalid Your Missing Something";
                        abc.flag = false;
                    }
                }
                else
                {
                    abc.status = "Mobile Number Or Email Are required";
                    abc.flag = false;
                }
                return Json(abc);
            }
            catch
            {
                abc.status = "failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> changePassword(ChangePasswordAPI usermodel)
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                var userId = User.Identity.GetUserId();
                var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var token = await UserManager.GeneratePasswordResetTokenAsync(userId);
                var result = await UserManager.ResetPasswordAsync(userId, token, usermodel.NewPassword);
                if (result.Succeeded == true)
                {
                    abc.status = "suceess";
                    abc.flag = true;
                    return Json(abc);
                }
                else
                {
                    abc.status = "failed";
                    abc.flag = false;
                    return Json(abc);
                }
            }
            catch
            {
                abc.status = "failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassword(ForgotPasswordAPI usermodel)
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {

                if (string.IsNullOrEmpty(usermodel.Email))
                {
                    if (!string.IsNullOrEmpty(usermodel.Mobile))
                    {
                        string EMail = db.tbl_UserDetails.Where(x => x.ContactNumber == usermodel.Mobile).FirstOrDefault().UserName;
                        usermodel.Email = EMail;
                    }
                    else
                    {
                        abc.status = "Enter Email Or Mobile Number";
                        abc.flag = false;
                        return Json(abc);
                    }
                }
                else {
                    string EMail = db.tbl_UserDetails.Where(x => x.EmailId == usermodel.Email).FirstOrDefault().UserName;
                    usermodel.Email = EMail;
                }
               
                var UserManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = await UserManager.FindByNameAsync(usermodel.Email);
                var userId = user.Id;
                var token = await UserManager.GeneratePasswordResetTokenAsync(userId);

                var result = await UserManager.ResetPasswordAsync(userId, token, "123456");
                if (result.Succeeded == true)
                {
                    abc.status = "suceess";
                    abc.flag = true;
                    return Json(abc);
                }
                else
                {
                    abc.status = "failed";
                    abc.flag = false;
                    return Json(abc);
                }
            }
            catch
            {
                abc.status = "failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> UserProfile()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                var data = db.tbl_UserDetails.Where(x => x.User_fkid == id).ToList();
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        //List Masters API or Httpget API List
        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> RoleList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new Microsoft.AspNet.Identity.EntityFramework.RoleStore<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new ApplicationDbContext()));
                var data = new System.Web.Mvc.SelectList(roleManager.Roles.ToList(), "Id", "Name");
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> ComplaintListPerUser(int skip, int pageSize, string cityid, int User_Wise)
        {
            returnAPiFlag abc = new returnAPiFlag();
            List<ReturnComplaintAPI> DesignResult = new List<ReturnComplaintAPI>();
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                if (User_Wise == 0)
                {
                    #region
                    if (cityid != "\"\"" && cityid != "0" && cityid != null)
                    {
                        if (skip >= 0)
                        {
                            int _cityid = Convert.ToInt32(cityid);
                            if (db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Count() <= 10)
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();

                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    tbl_UserDetails userd = db.tbl_UserDetails.Where(x => x.User_fkid == item.User_fkid).FirstOrDefault();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                                     {
                                                                         customername = a.customername,
                                                                         Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                                     }).ToList();

                                    if (userd != null)
                                    {
                                        model.UserProfilepic = userd.ProdilePic;
                                        model.UserFullename = userd.FullName;
                                    }
                                    DesignResult.Add(model);
                                }

                                return Json(DesignResult);
                            }
                            else
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    tbl_UserDetails userd = db.tbl_UserDetails.Where(x => x.User_fkid == item.User_fkid).FirstOrDefault();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    if (userd != null)
                                    {
                                        model.UserProfilepic = userd.ProdilePic;
                                        model.UserFullename = userd.FullName;
                                    }
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                        }
                    }
                    else
                    {
                        if (skip >= 0)
                        {
                            if (db.tbl_ComplaintMaster.OrderByDescending(x => x.pkid).Count() <= 10)
                            {
                                var data = db.tbl_ComplaintMaster.OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    tbl_UserDetails userd = db.tbl_UserDetails.Where(x => x.User_fkid == item.User_fkid).FirstOrDefault();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    if (userd != null)
                                    {
                                        model.UserProfilepic = userd.ProdilePic;
                                        model.UserFullename = userd.FullName;
                                    }
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                            else
                            {
                                var data = db.tbl_ComplaintMaster.OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    tbl_UserDetails userd = db.tbl_UserDetails.Where(x => x.User_fkid == item.User_fkid).FirstOrDefault();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    if (userd != null)
                                    {
                                        model.UserProfilepic = userd.ProdilePic;
                                        model.UserFullename = userd.FullName;
                                    }
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region
                    if (cityid != "\"\"" && cityid != "0" && cityid != null)
                    {
                        if (skip >= 0)
                        {
                            int _cityid = Convert.ToInt32(cityid);
                            if (db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid && x.User_fkid == id).OrderByDescending(x => x.pkid).Count() <= 10)
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid && x.User_fkid == id).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                            else
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.City_fkid == _cityid && x.User_fkid == id).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                        }
                    }
                    else
                    {
                        if (skip >= 0)
                        {
                            if (db.tbl_ComplaintMaster.Where(x => x.User_fkid == id).OrderByDescending(x => x.pkid).Count() <= 10)
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.User_fkid == id).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                            else
                            {
                                var data = db.tbl_ComplaintMaster.Where(x => x.User_fkid == id).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                                foreach (var item in data)
                                {
                                    ReturnComplaintAPI model = new ReturnComplaintAPI();
                                    model.pkid = item.pkid;
                                    model.Category_fkid = item.Category_fkid;
                                    model.ComplaintDescription = item.ComplaintDescription;
                                    model.ComplaintName = item.ComplaintName;
                                    model.AddedDate = item.AddedDate;
                                    model.Active = item.Active;
                                    model.Longitutde = item.Longitutde;
                                    model.Latitude = item.Latitude;
                                    model.City_fkid = item.City_fkid;
                                    model.ImagePath = item.ImagePath;
                                    model.LastModifiedDate = item.LastModifiedDate;
                                    model.Likestatus = db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid && x.User_fkid == id).Count();
                                    model.LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == item.pkid).ToList()
                                                      select new tbl_LikesMasterAPi
                                                      {
                                                          customername = a.customername,
                                                          Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                                      }).ToList();
                                    DesignResult.Add(model);
                                }
                                return Json(DesignResult);
                            }
                        }
                    }
                    #endregion
                }
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }

            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> NoticeboardList(int skip, int pageSize, string cityid)
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                if (cityid != "\"\"" && cityid != "0" && cityid != null)
                {
                    int _cityid = Convert.ToInt32(cityid);
                    if (skip >= 0)
                    {
                        if (db.tbl_NoticeBoardMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Count() <= 10)
                        {
                            var data = db.tbl_NoticeBoardMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                            return Json(data);
                        }
                        else
                        {
                            var data = db.tbl_NoticeBoardMaster.Where(x => x.City_fkid == _cityid).OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                            return Json(data);
                        }
                    }
                }
                else
                {
                    if (skip >= 0)
                    {
                        if (db.tbl_NoticeBoardMaster.OrderByDescending(x => x.pkid).Count() <= 10)
                        {
                            var data = db.tbl_NoticeBoardMaster.OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                            return Json(data);
                        }
                        else
                        {
                            var data = db.tbl_NoticeBoardMaster.OrderByDescending(x => x.pkid).Skip(skip * pageSize).Take(pageSize).ToList();
                            return Json(data);
                        }
                    }
                }
                abc.flag = false;
                return Json(abc);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> CityList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {

                var data = db.tbl_CityMaster.Where(x => x.StateID == 1646).ToList();
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> ComplaintCategoryList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                var data = db.tbl_Category_Complaint.ToList();
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> TourismList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            List<TourimList> data = new List<TourimList>();
            try
            {
                var TList = db.tbl_TourismMaster.ToList();

                foreach (var item in TList)
                {
                    TourimList Single = new TourimList();
                    Single.pkid = item.pkid;
                    Single.Sub_fkid = item.Sub_fkid;
                    Single.city_fkid = item.city_fkid;
                    Single.Name = item.Name;
                    Single.AddedDate = item.AddedDate;
                    Single.Description = item.Description;
                    Single.Longitude = item.Longitude;
                    Single.Lotitude = item.Lotitude;
                    Single.Address = item.Address;
                    Single.ContactNumber = item.ContactNumber;
                    Single.InchargeName = item.InchargeName;
                    Single.fileupload = db.tbl_MultipleFileUpload.Where(x => x.SubMaster_fkid == item.pkid).ToList();
                    data.Add(Single);
                }
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> CateSubcatTourismList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            List<categorSubcategoryList> data = new List<categorSubcategoryList>();
            try
            {
                var TList = db.tbl_Tourism_Category.ToList();
                categorSubcategoryList Single = new categorSubcategoryList();
                foreach (var item in TList)
                {
                    Single.pkid = item.pkid;
                    Single.Category_Name = item.Category_Name;
                    Single.Category_Description = item.Category_Description;
                    Single.LastModifiedDate = item.LastModifiedDate;
                    Single.subCatList = db.tbl_Tourism_SubCategory.Where(x => x.Category_fkid == item.pkid).ToList();
                    data.Add(Single);
                }
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> PrabhagList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {


                List<tbl_Prabhag_MasterAPI> TList = new List<tbl_Prabhag_MasterAPI>();
                var data = db.tbl_Prabhag_Master.ToList();
                foreach (var item in data)
                {
                    tbl_Prabhag_MasterAPI Single = new tbl_Prabhag_MasterAPI();
                    Single.pkid = item.pkid;
                    Single.Prabhag_Name = item.Prabhag_Name;
                    Single.Address = item.Address;
                    Single.Description = item.Description;
                    Single.adddate = item.adddate;
                    Single.status = item.status;

                    List<tbl_Ward_masterAPI> LLwardsingle = new List<tbl_Ward_masterAPI>();
                    foreach (var wardd in db.tbl_Ward_master.Where(x => x.prabhag_fkid == Single.pkid).ToList())
                    {
                        tbl_Ward_masterAPI wardsingle = new tbl_Ward_masterAPI();

                        wardsingle.pkid = wardd.pkid;
                        wardsingle.prabhag_fkid = wardd.prabhag_fkid;
                        wardsingle.ward_Name = wardd.ward_Name;
                        wardsingle.adddate = wardd.adddate;
                        wardsingle.address = wardd.address;
                        wardsingle.description = wardd.description;
                        wardsingle.status = wardd.status;

                        List<tbl_wardMember_Master> LLmembersingle = new List<tbl_wardMember_Master>();
                        foreach (var Member in db.tbl_wardMember_Master.Where(x => x.Ward_fkid == wardsingle.pkid).ToList())
                        {
                            tbl_wardMember_Master membersingle = new tbl_wardMember_Master();

                            membersingle.pkid = Member.pkid;
                            membersingle.Ward_fkid = Member.Ward_fkid;
                            membersingle.Member_Name = Member.Member_Name;
                            membersingle.adddate = Member.adddate;
                            membersingle.Description = Member.Description;
                            membersingle.status = Member.status;
                            membersingle.Address = Member.Address;
                            membersingle.MobileNo = Member.MobileNo;
                            membersingle.ProfilePic = Member.ProfilePic;
                            LLmembersingle.Add(membersingle);
                        }
                        wardsingle.MemberList = LLmembersingle;
                        LLwardsingle.Add(wardsingle);
                    }
                    Single.WardList = LLwardsingle;
                    TList.Add(Single);
                }

                return Json(TList);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        //[Authorize]
        //[HttpGet]
        //public async Task<IHttpActionResult> WardList()
        //{
        //    returnAPiFlag abc = new returnAPiFlag();
        //    try
        //    {
        //        List<tbl_Ward_masterAPI> TList = new List<tbl_Ward_masterAPI>();
        //       var data = db.tbl_Ward_master.ToList();
        //       foreach (var item in data)
        //       {
        //           tbl_Ward_masterAPI Single = new tbl_Ward_masterAPI();
        //           Single.pkid = item.pkid;
        //           Single.prabhag_fkid = item.prabhag_fkid;
        //           Single.ward_Name = item.ward_Name;
        //           Single.adddate = item.adddate;
        //           Single.address = item.address;
        //           Single.description = item.description;
        //           Single.status = item.status;                                
        //           Single.PrabhagList = db.tbl_Prabhag_Master.ToList();
        //           TList.Add(Single);
        //       }
        //        return Json(TList);
        //    }
        //    catch (Exception e)
        //    {
        //        abc.status = "Failed";
        //        abc.flag = false;
        //        return Json(abc);
        //    }
        //}

        //[Authorize]
        //[HttpGet]
        //public async Task<IHttpActionResult> WardMemberList()
        //{
        //    returnAPiFlag abc = new returnAPiFlag();
        //    try
        //    {
        //        List<tbl_wardMember_MasterAPI> TList = new List<tbl_wardMember_MasterAPI>();
        //        var data = db.tbl_wardMember_Master.ToList();
        //        foreach (var item in data)
        //        {
        //            tbl_wardMember_MasterAPI Single = new tbl_wardMember_MasterAPI();
        //            Single.pkid = item.pkid;
        //            Single.Ward_fkid = item.Ward_fkid;
        //            Single.Member_Name = item.Member_Name;
        //            Single.adddate = item.adddate;
        //            Single.Description = item.Description;
        //            Single.status = item.status;
        //            Single.Address = item.Address;
        //            Single.MobileNo = item.MobileNo;
        //            Single.WardList = db.tbl_Ward_master.ToList();
        //            TList.Add(Single);
        //        }
        //        return Json(TList);
        //    }
        //    catch (Exception e)
        //    {
        //        abc.status = "Failed";
        //        abc.flag = false;
        //        return Json(abc);
        //    }
        //}

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> EmergencyContactList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                List<ReturnCOntactListApi> TList = new List<ReturnCOntactListApi>();
                var data = db.tbl_EmergencyContactCategory.ToList();
                foreach (var item in data)
                {
                    ReturnCOntactListApi Single = new ReturnCOntactListApi();
                    Single.pkid = item.pkid;
                    Single.CatgoryName = item.CatgoryName;
                    Single.categoryDescription = item.categoryDescription;
                    Single.LastModifiedDate = item.LastModifiedDate;

                    List<tbl_Emergency_ContactUs> LLwardsingle = new List<tbl_Emergency_ContactUs>();
                    foreach (var cont in db.tbl_Emergency_ContactUs.Where(x => x.Category_fkid == Single.pkid).ToList())
                    {
                        tbl_Emergency_ContactUs wardsingle = cont;
                        LLwardsingle.Add(wardsingle);
                    }
                    Single.Contact = LLwardsingle;
                    TList.Add(Single);
                }
                return Json(TList);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> UserLikeComplaintList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            List<tbl_ComplaintMaster> data = new List<tbl_ComplaintMaster>();
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();               
                var LikeList = (from a in db.tbl_LikesMaster.Where(x => x.Master_fkid == 1 && x.SubMaster_fkid == 1).ToList()
                                  select new tbl_LikesMasterAPi
                                  {
                                      customername = a.customername,
                                      Pic = db.tbl_UserDetails.Where(x => x.User_fkid == a.User_fkid).FirstOrDefault().ProdilePic
                                  }).ToList();
                foreach (var item in LikeList)
                {
                    tbl_ComplaintMaster Single = new tbl_ComplaintMaster();
                    Single = db.tbl_ComplaintMaster.Where(x => x.pkid == item.SubMaster_fkid).FirstOrDefault();
                    data.Add(Single);
                }
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> NewsAndUpdateList()
        {
            returnAPiFlag abc = new returnAPiFlag();          
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                var data = db.tbl_NewsAndUpdated.Where(x => x.status == 1).ToList();
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> DeletedUserList()
        {
            returnAPiFlag abc = new returnAPiFlag();
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                var data = db.tbl_DeletedNoticeUser.Where(x => x.User_fkid == id).ToList();
                return Json(data);
            }
            catch (Exception e)
            {
                abc.status = "Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

        //Httppot Data API
        //[Authorize]
        //[HttpPost]
        //public async Task<IHttpActionResult> ComplaintRegister(tbl_ComplaintMaster model)
        //{
        //    try
        //    {
        //        string id;
        //        id = User.Identity.GetUserId();
        //        id = RequestContext.Principal.Identity.GetUserId();
        //        returnAPiFlag abc = new returnAPiFlag();
        //        if (model.pkid == 0)
        //        {
        //            var httpRequest = HttpContext.Current.Request;
        //            if (httpRequest.Files.Count > 0)
        //            {
        //                var docfiles = new List<string>();
        //                for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
        //                {
        //                    var postedFile = httpRequest.Files[i];
        //                }
        //            }
        //            model.LastModifiedDate = DateTime.Now;
        //            model.User_fkid = id;
        //            db.tbl_ComplaintMaster.Add(model);
        //            db.SaveChanges();
        //            abc.status = "Data Save Successfully";
        //            abc.flag = true;
        //        }
        //        else
        //        {
        //            WebFunction web = new WebFunction();
        //            model.LastModifiedDate = DateTime.Now;
        //            db.tbl_ComplaintMaster.Attach(model);
        //            db.Entry(model).State = EntityState.Modified;
        //            db.SaveChanges();
        //            abc.status = "Data updated Successfully";
        //            abc.flag = true;
        //        }
        //        return Json(abc);
        //    }
        //    catch (Exception e)
        //    {
        //        returnAPiFlag abc = new returnAPiFlag();
        //        abc.status = "Data Save Failed";
        //        abc.flag = false;
        //        return Json(abc);
        //    }
        //}

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> EditUserProfile(tbl_UserDetails model)
        {
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                returnAPiFlag abc = new returnAPiFlag();
                if (model.pkid == 0)
                {
                    var httpRequest = HttpContext.Current.Request;
                    model.LastModifiedDate = DateTime.Now;
                    model.User_fkid = id;
                    db.tbl_UserDetails.Add(model);
                    db.SaveChanges();
                    abc.status = "Data Save Successfully";
                    abc.flag = true;
                }
                else
                {
                    model.LastModifiedDate = DateTime.Now;
                    db.tbl_UserDetails.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abc.status = "Data updated Successfully";
                    abc.flag = true;
                }
                return Json(abc);
            }
            catch (Exception e)
            {
                returnAPiFlag abc = new returnAPiFlag();
                abc.status = "Data Save Failed";
                abc.flag = false;
                return Json(abc);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> LikeSave(tbl_LikesMaster model)
        {
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                string name = User.Identity.GetUserName();
                LikeMasterReturn abc = new LikeMasterReturn();
                int co = db.tbl_LikesMaster.Where(x => x.User_fkid == id && x.Master_fkid == model.Master_fkid && x.SubMaster_fkid == model.SubMaster_fkid).Count();
                if (co == 0 && model.pkid == 0) { model.pkid = co; }
                if (model.pkid == 0)
                {
                    model.LastModifieddatetime = DateTime.Now;
                    model.User_fkid = id;
                    model.customername = db.tbl_UserDetails.Where(x => x.User_fkid == id).FirstOrDefault().FullName;
                    db.tbl_LikesMaster.Add(model);
                    db.SaveChanges();
                    abc.status = "Data Save Successfully";
                    abc.flag = true;
                    abc.count = db.tbl_LikesMaster.Where(x => x.Master_fkid == model.Master_fkid && x.SubMaster_fkid == model.SubMaster_fkid).Count();
                }
                else
                {
                    model.LastModifieddatetime = DateTime.Now;
                    model.User_fkid = id;
                    model.customername = db.tbl_UserDetails.Where(x => x.User_fkid == id).FirstOrDefault().FullName;
                    db.tbl_LikesMaster.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abc.status = "Data updated Successfully";
                    abc.flag = true;
                    abc.count = db.tbl_LikesMaster.Where(x => x.Master_fkid == model.Master_fkid && x.SubMaster_fkid == model.SubMaster_fkid).Count();
                }
                return Json(abc);
            }
            catch (Exception e)
            {
                LikeMasterReturn abc = new LikeMasterReturn();
                abc.status = "Data Save Failed";
                abc.flag = false;
                abc.count = db.tbl_LikesMaster.Where(x => x.Master_fkid == model.Master_fkid && x.SubMaster_fkid == model.SubMaster_fkid).Count();
                return Json(abc);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> NoticeDeleteByUser(tbl_DeletedNoticeUser model)
        {
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                string name = User.Identity.GetUserName();
                returnAPiFlag abcd = new returnAPiFlag();                    
                if (model.pkid == 0)
                {
                    model.LastDatetime = DateTime.Now;
                    model.User_fkid = id;                  
                    db.tbl_DeletedNoticeUser.Add(model);
                    db.SaveChanges();
                    abcd.status = "Data Save Successfully";
                    abcd.flag = true;                 
                }
                else
                {                   
                    model.LastDatetime = DateTime.Now;
                    model.User_fkid = id;
                    db.tbl_DeletedNoticeUser.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abcd.status = "Data updated Successfully";
                    abcd.flag = true;                   
                }
                return Json(abcd);
            }
            catch (Exception e)
            {
                returnAPiFlag abc = new returnAPiFlag();
                abc.status = "Data Save Failed";
                abc.flag = false;              
                return Json(abc);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> FeedBack(tbl_feedback_Master model)
        {
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
                string name = User.Identity.GetUserName();
                returnAPiFlag abcd = new returnAPiFlag();             
                if (model.pkid == 0)
                {
                    model.adddate = DateTime.Now;
                    model.user_fkid = id;
                    db.tbl_feedback_Master.Add(model);
                    db.SaveChanges();
                    abcd.status = "Data Save Successfully";
                    abcd.flag = true;
                }
                else
                {
                    model.adddate = DateTime.Now;
                    model.user_fkid = id;
                    db.tbl_feedback_Master.Attach(model);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abcd.status = "Data updated Successfully";
                    abcd.flag = true;
                }
                return Json(abcd);
            }
            catch (Exception e)
            {
                LikeMasterReturn abc = new LikeMasterReturn();
                abc.status = "Data Save Failed";
                abc.flag = false;
                return Json(abc);
            }
        }

    }
}
