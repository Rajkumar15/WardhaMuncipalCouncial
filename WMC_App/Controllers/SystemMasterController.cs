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
        public readonly IRepository<tbl_Prabhag_Master> _prabhag;
        public readonly IRepository<tbl_Ward_master> _ward;
        public readonly IRepository<tbl_wardMember_Master> _wardmember;

        public SystemMasterController(IRepository<tbl_UserDetails> user, IRepository<tbl_Category_Complaint> catComplaint, IRepository<tbl_Tourism_Category> catTourism, IRepository<tbl_Tourism_SubCategory> subcatTourism,
            IRepository<tbl_Prabhag_Master> prabhag, IRepository<tbl_Ward_master> ward, IRepository<tbl_wardMember_Master> wardmember)
        {
            _user = user;
            _catComplaint = catComplaint;
            _catTourism = catTourism;
            _subcatTourism = subcatTourism;
            _prabhag = prabhag;
            _ward=ward;
            _wardmember=wardmember;
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

        [HttpGet]
        public ActionResult PrabhagEntry(string id)
        {
           
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Prabhag_Master model = _prabhag.Get(_id);
                tbl_Prabhag_Masterss abc = new tbl_Prabhag_Masterss();
                abc.pkid = model.pkid;
                abc.Prabhag_Name = model.Prabhag_Name;
                abc.Address = model.Address;
                abc.Description = model.Description;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult PrabhagEntry(tbl_Prabhag_Masterss model)
        {          
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Prabhag_Master abc = new tbl_Prabhag_Master();
                    abc.Prabhag_Name = model.Prabhag_Name;
                    abc.Address = model.Address;
                    abc.Description = model.Description;
                    abc.adddate = DateTime.Now;
                    _prabhag.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Prabhag_Master abc = _prabhag.Get(_id);
                    abc.pkid = model.pkid;
                    abc.Prabhag_Name = model.Prabhag_Name;
                    abc.Address = model.Address;
                    abc.Description = model.Description;
                    abc.adddate = DateTime.Now;
                    _prabhag.Update(abc);
                }
                return RedirectToAction("PrabhagEntry", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }

        public ActionResult GetPrabhagList()
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
                var v = (from a in _prabhag.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             name = a.Prabhag_Name,
                             add = a.Address,
                             des = a.Description
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower()) || x.add.ToLower().Contains(search.ToLower()) || x.des.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult WardEntry(string id)
        {
            ViewBag.prabhag = new SelectList(_prabhag.GetAll(), "pkid", "Prabhag_Name");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_Ward_master model = _ward.Get(_id);
                tbl_Ward_masterss abc = new tbl_Ward_masterss();
                abc.pkid = model.pkid;
                abc.prabhag_fkid = model.prabhag_fkid;
                abc.ward_Name=model.ward_Name;
                abc.description=model.description;              
                abc.address = model.address;
                abc.adddate = DateTime.Now;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult WardEntry(tbl_Ward_masterss model)
        {
            ViewBag.prabhag = new SelectList(_prabhag.GetAll(), "pkid", "Prabhag_Name");
            try
            {
                if (model.pkid == 0)
                {
                    tbl_Ward_master abc = new tbl_Ward_master();
                    abc.prabhag_fkid = model.prabhag_fkid;
                    abc.ward_Name = model.ward_Name;
                    abc.description = model.description;
                    abc.address = model.address;
                    abc.adddate = DateTime.Now;
                    _ward.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_Ward_master abc = _ward.Get(_id);
                    abc.pkid = model.pkid;
                    abc.prabhag_fkid = model.prabhag_fkid;
                    abc.ward_Name = model.ward_Name;
                    abc.description = model.description;
                    abc.address = model.address;
                    abc.adddate = DateTime.Now;
                    _ward.Update(abc);
                }
                return RedirectToAction("WardEntry", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }

        public ActionResult GetwardList()
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
                var v = (from a in _ward.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             prabhag = _prabhag.Get(a.prabhag_fkid).Prabhag_Name,
                             name = a.ward_Name,
                             add = a.address,
                             des = a.description
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower()) || x.add.ToLower().Contains(search.ToLower()) || x.des.ToLower().Contains(search.ToLower()) || x.prabhag.ToLower().Contains(search.ToLower())) select b);
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
        public ActionResult WardMemberEntry(string id)
        {
            ViewBag.ward = new SelectList(_ward.GetAll(), "pkid", "ward_Name");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_wardMember_Master model = _wardmember.Get(_id);
                tbl_wardMember_Masterss abc = new tbl_wardMember_Masterss();
                abc.pkid = model.pkid;
                abc.Ward_fkid = model.Ward_fkid;
                abc.Member_Name = model.Member_Name;
                abc.Description = model.Description;
                abc.adddate = model.adddate;
                abc.status = model.status;
                abc.Address = model.Address;
                abc.MobileNo = model.MobileNo;
                return View(abc);
            }
            return View();
        }

        [HttpPost]
        public ActionResult WardMemberEntry(tbl_wardMember_Masterss model)
        {
            ViewBag.ward = new SelectList(_ward.GetAll(), "pkid", "ward_Name");
            try
            {
                if (model.pkid == 0)
                {
                    tbl_wardMember_Master abc = new tbl_wardMember_Master();
                    abc.Ward_fkid = model.Ward_fkid;
                    abc.Member_Name = model.Member_Name;
                    abc.Description = model.Description;
                    abc.adddate = DateTime.Now;
                    abc.status = model.status;
                    abc.Address = model.Address;
                    abc.MobileNo = model.MobileNo;
                    _wardmember.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_wardMember_Master abc = _wardmember.Get(_id);
                    abc.pkid = model.pkid;
                    abc.Ward_fkid = model.Ward_fkid;
                    abc.Member_Name = model.Member_Name;
                    abc.Description = model.Description;
                    abc.adddate = DateTime.Now;
                    abc.status = model.status;
                    abc.Address = model.Address;
                    abc.MobileNo = model.MobileNo;
                    _wardmember.Update(abc);
                }
                return RedirectToAction("WardMemberEntry", "SystemMaster");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }

        public ActionResult GetwardMemberList()
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
                var v = (from a in _wardmember.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             ward = _ward.Get(a.Ward_fkid).ward_Name,
                             name = a.Member_Name,
                             add = a.Address,
                             des = a.Description,
                              mob = a.MobileNo
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.name.ToLower().Contains(search.ToLower()) || x.mob.ToLower().Contains(search.ToLower()) || x.add.ToLower().Contains(search.ToLower()) || x.des.ToLower().Contains(search.ToLower()) || x.ward.ToLower().Contains(search.ToLower())) select b);
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
    }
}