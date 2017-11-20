using DataLayer;
using DataLayer.WMCModel;
using itechDll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMC_App.Models.DAL;

namespace WMC_App.Controllers
{
    [Authorize]
    public class SystemMasterController : BaseController
    {

        public readonly IRepository<tbl_UserDetails> _user;
        public readonly IRepository<tbl_Category_Complaint> _catComplaint;
        public readonly IRepository<tbl_Tourism_Category> _catTourism;
        public readonly IRepository<tbl_Tourism_SubCategory> _subcatTourism;

        public SystemMasterController(IRepository<tbl_UserDetails> user, IRepository<tbl_Category_Complaint> catComplaint, IRepository<tbl_Tourism_Category> catTourism, IRepository<tbl_Tourism_SubCategory> subcatTourism)
        {
            _user = user;
            _catComplaint = catComplaint;
            _catTourism = catTourism;
            _subcatTourism = subcatTourism;
        }
        // GET: SystemMaster
        [HttpGet]
        public ActionResult ComplaintCat(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Category_Complaint model = _catComplaint.Get(_id);
                tbl_Category_Complaintss abc = new tbl_Category_Complaintss();
                abc.pkid = model.pkid;
                abc.CategoryName = model.CategoryName;
                abc.Description = model.Description;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult ComplaintCat(tbl_Category_Complaintss model)
        {
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Category_Complaint abc = new tbl_Category_Complaint();
                    abc.CategoryName = model.CategoryName;
                    abc.Description = model.Description;
                    abc.LastModifedDate = DateTime.Now;
                    _catComplaint.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Category_Complaint abc = _catComplaint.Get(_id);
                    abc.pkid = model.pkid;
                    abc.CategoryName = model.CategoryName;
                    abc.Description = model.Description;
                    abc.LastModifedDate = DateTime.Now;
                    _catComplaint.Update(abc);
                }
                return RedirectToAction("ComplaintCat", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetCatComplaintList()
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
                var v = (from a in _catComplaint.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             brandname = a.CategoryName,
                             brandimage = a.Description
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.brandname.ToLower().Contains(search.ToLower()) || x.brandimage.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    // v = v.OrderBy(sortColumn, sortColumnDir);
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
        public ActionResult TourismCat(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Tourism_Category model = _catTourism.Get(_id);
                tbl_Tourism_Categoryss abc = new tbl_Tourism_Categoryss();
                abc.pkid = model.pkid;
                abc.Category_Name = model.Category_Name;
                abc.Category_Description = model.Category_Description;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult TourismCat(tbl_Tourism_Categoryss model)
        {
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Tourism_Category abc = new tbl_Tourism_Category();
                    abc.Category_Name = model.Category_Name;
                    abc.Category_Description = model.Category_Description;
                    abc.LastModifiedDate = DateTime.Now;
                    _catTourism.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Tourism_Category abc = _catTourism.Get(_id);
                    abc.pkid = model.pkid;
                    abc.Category_Name = model.Category_Name;
                    abc.Category_Description = model.Category_Description;
                    abc.LastModifiedDate = DateTime.Now;
                    _catTourism.Update(abc);
                }
                return RedirectToAction("TourismCat", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetTourismCatList()
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
                var v = (from a in _catTourism.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             brandname = a.Category_Name,
                             brandimage = a.Category_Description
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.brandname.ToLower().Contains(search.ToLower()) || x.brandimage.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    // v = v.OrderBy(sortColumn, sortColumnDir);
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
        public ActionResult TourismSubcate(string id)
        {
            ViewBag.catList = new SelectList(_catTourism.GetAll(), "pkid", "Category_Name");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Tourism_SubCategory model = _subcatTourism.Get(_id);
                tbl_Tourism_SubCategoryss abc = new tbl_Tourism_SubCategoryss();
                abc.pkid = model.pkid;
                abc.Category_fkid = model.Category_fkid;
                abc.SubCategory_Name = model.SubCategory_Name;
                abc.SubCategory_Description = model.SubCategory_Description;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult TourismSubcate(tbl_Tourism_SubCategoryss model)
        {
            ViewBag.catList = new SelectList(_catTourism.GetAll(), "pkid", "Category_Name");
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Tourism_SubCategory abc = new tbl_Tourism_SubCategory();
                    abc.SubCategory_Name = model.SubCategory_Name;
                    abc.SubCategory_Description = model.SubCategory_Description;
                    abc.Category_fkid = model.Category_fkid;
                    abc.LastModifiedDate = DateTime.Now;
                    _subcatTourism.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Tourism_SubCategory abc = _subcatTourism.Get(_id);
                    abc.pkid = model.pkid;
                    abc.SubCategory_Name = model.SubCategory_Name;
                    abc.Category_fkid = model.Category_fkid;
                    abc.SubCategory_Description = model.SubCategory_Description;
                    abc.LastModifiedDate = DateTime.Now;
                    _subcatTourism.Update(abc);
                }
                return RedirectToAction("TourismSubcate", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetTourismSubcateList()
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
                var v = (from a in _subcatTourism.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             cat = (a.Category_fkid != null ? _catTourism.Get(a.Category_fkid).Category_Name : ""),
                             brandname = a.SubCategory_Name,
                             brandimage = a.SubCategory_Description
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.brandname.ToLower().Contains(search.ToLower()) || x.brandimage.ToLower().Contains(search.ToLower()) || x.cat.ToLower().Contains(search.ToLower())) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    // v = v.OrderBy(sortColumn, sortColumnDir);
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

        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            string Username = User.Identity.Name;
            var data = _user.GetAll().Where(x => x.UserName == Username).FirstOrDefault();
            tbl_UserDetailsss abc = new tbl_UserDetailsss();
            abc.User_fkid = data.User_fkid;
            abc.UserName = data.UserName;
            abc.RoleName = data.RoleName;
            abc.EmailId = data.EmailId;
            abc.FullName = data.FullName;
            abc.ContactNumber = data.ContactNumber;
            abc.RegisteredDate = data.RegisteredDate;
            abc.AddressLine1 = data.AddressLine1;
            abc.AddressLine2 = data.AddressLine2;
            return View(abc);
        }
    }
}