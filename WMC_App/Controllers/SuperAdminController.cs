using DataLayer;
using DataLayer.WMCModel;
using itechDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMC_App.Models.BAL;
using WMC_App.Models.DAL;

namespace WMC_App.Controllers
{
    public class SuperAdminController : BaseController
    {
        // GET: SuperAdmin
        public readonly IRepository<tbl_UserDetails> _user;
     

        public SuperAdminController(IRepository<tbl_UserDetails> user)
        {
            _user = user;
        }

        [HttpGet]
        public ActionResult SaveCustomer()
        {
            string userid = "";
            try
            {
                userid = TempData["id"].ToString();
                string ro = TempData["role"].ToString();
                string Uname = null;
                try { Uname = TempData["username"].ToString(); }
                catch { }
                string email = null;
                try { email = TempData["ema"].ToString(); }
                catch { }
                string pas = TempData["passwo"].ToString();
                string funame = null;
                try { funame = TempData["fname"].ToString(); }
                catch { }
                string mobino = null;
                try { TempData["mobi"].ToString(); }
                catch { }

                tbl_UserDetails abc = new tbl_UserDetails();
                abc.User_fkid = userid;
                abc.UserName = Uname;
                abc.RoleName = ro;
                abc.EmailId = email;
                abc.FullName = funame;
                abc.ContactNumber = mobino;
                abc.RegisteredDate = DateTime.Now;
                abc.LastModifiedDate = DateTime.Now;
                _user.Add(abc);
                return RedirectToAction("Register", "Account");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                return RedirectToAction("Register", "Account", new { Exceptionmsg = e.Message, id = userid });
            }
        }

        [HttpGet]
        public ActionResult UserList()
        {
            return View();
        }
        public ActionResult GetUserList()
        {
            var search = Request.Form.GetValues("search[value]")[0];
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Find Order Column
            string sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            string sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            //dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            try
            {
                var v = (from a in _user.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             fullname = a.FullName,
                             add = a.AddressLine1,
                             role = a.RoleName,
                             userid = a.User_fkid,
                             Radd = a.AddressLine1,
                             mob = a.ContactNumber,
                             un=a.UserName,
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.fullname.ToLower().Contains(search.ToLower()) || x.add.ToLower().Contains(search.ToLower()) || x.role.Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult EditUserDetails(int id)
        {
            var data = _user.Get(id);
            tbl_UserDetailsss abc = new tbl_UserDetailsss();
            abc.pkid = data.pkid;
            abc.FullName = data.FullName;
            abc.UserName = data.UserName;
            abc.ContactNumber = data.ContactNumber;
            abc.AddressLine1 = data.AddressLine1;
            abc.RoleName = data.RoleName;
            abc.EmailId = data.EmailId;
            abc.LastModifiedDate = DateTime.Now;
            return View(abc);
        }
        [HttpPost]
        public ActionResult EditUserDetails(tbl_UserDetailsss model,HttpPostedFileBase pic)
        {
            try
            {
                tbl_UserDetails abc = _user.Get(model.pkid);
                abc.FullName = model.FullName;
                abc.ContactNumber = model.ContactNumber;
                abc.AddressLine1 = model.AddressLine1;
                abc.RoleName = model.RoleName;
                abc.EmailId = model.EmailId;
                abc.LastModifiedDate = DateTime.Now;
                if (pic != null)
                {
                    string path = System.Web.HttpContext.Current.Server.MapPath(model.ProdilePic);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    WebFunction web = new WebFunction();
                    model.ProdilePic = web.Storefile(pic, 4);
                }
                _user.Update(abc);
                return RedirectToAction("Register", "Account");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
    }
}