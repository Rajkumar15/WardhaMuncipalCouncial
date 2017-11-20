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
    public class AdministrationController : BaseController
    {

        public readonly IRepository<tbl_UserDetails> _user;
        public readonly IRepository<tbl_CityMaster> _city;
        public readonly IRepository<tbl_NoticeBoardMaster> _notice;
        public readonly IRepository<tbl_ComplaintMaster> _complaint;
        public readonly IRepository<tbl_Category_Complaint> _catComplaint;
        public readonly IRepository<tbl_Tourism_Category> _catTourisom;
        public readonly IRepository<tbl_Tourism_SubCategory> _subcatTourism;
        public readonly IRepository<tbl_TourismMaster> _tourim;
        public readonly IRepository<tbl_MultipleFileUpload> _mfile;

        public AdministrationController(IRepository<tbl_UserDetails> user, IRepository<tbl_CityMaster> city, IRepository<tbl_NoticeBoardMaster> NoticeMaster, IRepository<tbl_ComplaintMaster> complaint, IRepository<tbl_Category_Complaint> catcomplaint,
            IRepository<tbl_Tourism_Category> catTourisom, IRepository<tbl_Tourism_SubCategory> subcatTourism, IRepository<tbl_TourismMaster> tourim, IRepository<tbl_MultipleFileUpload> mfile)
        {
            _user = user;
            _city = city;
            _notice = NoticeMaster;
            _complaint = complaint;
            _catComplaint = catcomplaint;
            _catTourisom = catTourisom;
            _subcatTourism = subcatTourism;
            _tourim = tourim;
            _mfile = mfile;
        }
        // GET: Administration
        [HttpGet]
        public ActionResult Addnotice(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_NoticeBoardMaster model = _notice.Get(_id);
                tbl_NoticeBoardMasterss abc = new tbl_NoticeBoardMasterss();
                abc.pkid = model.pkid;
                abc.NoticeName = model.NoticeName;
                abc.NoticeDescription = model.NoticeDescription;
                abc.Image_Path = model.Image_Path;
                abc.AddedDate = model.AddedDate;
                abc.RequiredDays = model.RequiredDays;
                abc.Active = model.Active;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Addnotice(tbl_NoticeBoardMasterss model,HttpPostedFileBase image)
        {
            try
            {
                if (model.pkid == 0)
                {

                    tbl_NoticeBoardMaster abc = new tbl_NoticeBoardMaster();
                    if (image != null)
                    {
                        WebFunction web = new WebFunction();
                        model.Image_Path = web.Storefile(image,2);
                    }
                    abc.NoticeName = model.NoticeName;
                    abc.NoticeDescription = model.NoticeDescription;
                    abc.Image_Path = model.Image_Path;
                    abc.AddedDate = DateTime.Now;
                    abc.RequiredDays = model.RequiredDays;
                    abc.Active = model.Active;
                    abc.LastModifiedDate = DateTime.Now;
                    _notice.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_NoticeBoardMaster abc = _notice.Get(_id);
                    if (image != null)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(model.Image_Path);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        WebFunction web = new WebFunction();
                        model.Image_Path = web.Storefile(image,2);
                    }
                    abc.pkid = model.pkid;
                    abc.NoticeName = model.NoticeName;
                    abc.NoticeDescription = model.NoticeDescription;
                    abc.Image_Path = model.Image_Path;
                    abc.AddedDate = model.AddedDate;
                    abc.RequiredDays = model.RequiredDays;
                    abc.Active = model.Active;
                    abc.LastModifiedDate = DateTime.Now;
                    _notice.Update(abc);
                }
                return RedirectToAction("Addnotice", "Administration");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetNoticeList()
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
                var v = (from a in _notice.GetAll()
                         select new
                         {
                             pkid = a.pkid,
                             noticename = a.NoticeName,
                             Noticedes = a.NoticeDescription,
                             image = System.Web.HttpContext.Current.Server.MapPath(a.Image_Path),
                             adddate=a.AddedDate,
                             active=a.Active
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.noticename.ToLower().Contains(search.ToLower()) || x.Noticedes.ToLower().Contains(search.ToLower()) || x.adddate.ToString().Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    // v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.OrderByDescending(x=>x.pkid).Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult DeleteNotice(int id)
        {
            try
            {
                tbl_NoticeBoardMaster abc = _notice.Get(id);
                _notice.Remove(id, true);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChangeStateNotice(int id)
        {
            string result = "";
            try
            {
                tbl_NoticeBoardMaster abc = _notice.Get(id);
               var st = abc.Active;               
               if (!String.IsNullOrWhiteSpace(st.ToString()))
               {
                   int _st = Convert.ToInt32(st);
                   if (_st == 1)
                   {
                       abc.Active = 0;
                       _notice.Update(abc);
                       result = "Deactive";
                   }
                   else
                   {
                       abc.Active = 1;
                       _notice.Update(abc);
                       result = "Active";
                   }
               }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public ActionResult Addcomplaint(string id)
        {
            ViewBag.comList = new SelectList(_catComplaint.GetAll(), "pkid", "CategoryName");
            ViewBag.cityList = new SelectList(_city.GetAll().Where(x => x.StateID == 1646).ToList(), "ID", "Name");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_ComplaintMaster model = _complaint.Get(_id);
                tbl_ComplaintMasterss abc = new tbl_ComplaintMasterss();
                abc.pkid = model.pkid;
                abc.Category_fkid = model.Category_fkid;
                abc.ComplaintDescription = model.ComplaintDescription;
                abc.ComplaintName = model.ComplaintName;
                abc.AddedDate = model.AddedDate;
                abc.Active = model.Active;
                abc.Longitutde = model.Longitutde;
                abc.Latitude = model.Latitude;
                abc.City_fkid = model.City_fkid;
                abc.ImagePath = model.ImagePath;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult Addcomplaint(tbl_ComplaintMasterss model, HttpPostedFileBase image)
        {
            ViewBag.comList = new SelectList(_catComplaint.GetAll(), "pkid", "CategoryName");
            ViewBag.cityList = new SelectList(_city.GetAll().Where(x => x.StateID == 1646).ToList(), "ID", "Name");
            try
            {
                if (model.pkid == 0)
                {

                    tbl_ComplaintMaster abc = new tbl_ComplaintMaster();
                    if (image != null)
                    {
                        WebFunction web = new WebFunction();
                        model.ImagePath = web.Storefile(image, 1);
                    }
                    abc.Category_fkid = model.Category_fkid;
                    abc.ComplaintDescription = model.ComplaintDescription;
                    abc.ComplaintName = model.ComplaintName;
                    abc.AddedDate = model.AddedDate;
                    abc.Active = model.Active;                
                    abc.Longitutde = model.Longitutde;
                    abc.Latitude = model.Latitude;
                    abc.City_fkid = model.City_fkid;
                    abc.ImagePath = model.ImagePath;
                    abc.LastModifiedDate = DateTime.Now;
                    _complaint.Add(abc);
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_ComplaintMaster abc = _complaint.Get(_id);
                    if (image != null)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath(model.ImagePath);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        WebFunction web = new WebFunction();
                        model.ImagePath = web.Storefile(image, 1);
                    }
                    abc.pkid = model.pkid;
                    abc.Category_fkid = model.Category_fkid;
                    abc.ComplaintDescription = model.ComplaintDescription;
                    abc.ComplaintName = model.ComplaintName;
                    abc.AddedDate = model.AddedDate;
                    abc.Active = model.Active;
                    abc.Longitutde = model.Longitutde;
                    abc.Latitude = model.Latitude;
                    abc.City_fkid = model.City_fkid;
                    abc.ImagePath = model.ImagePath;
                    abc.LastModifiedDate = DateTime.Now;
                    _complaint.Update(abc);
                }
                return RedirectToAction("Addcomplaint", "Administration");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetComplaintList()
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
                var v = (from a in _complaint.GetAll()

                         select new
                         {
                             pkid = a.pkid,
                             comname = a.ComplaintName,
                             des = a.ComplaintDescription,
                             catg = (a.Category_fkid != null ? _catComplaint.Get(a.Category_fkid).CategoryName : ""),
                             adddate = a.AddedDate,
                             active = a.Active
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.comname.ToLower().Contains(search.ToLower()) || x.des.ToLower().Contains(search.ToLower()) || x.catg.ToLower().Contains(search.ToLower()) || x.adddate.ToString().Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                     //v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.OrderByDescending(x=>x.pkid).Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult DeleteComplaint(int id)
        {
            try
            {
                tbl_ComplaintMaster abc = _complaint.Get(id);
                _complaint.Remove(id, true);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpGet]
        public ActionResult AddTourism(string id)
        {
            ViewBag.subList = new SelectList(_subcatTourism.GetAll(), "pkid", "SubCategory_Name");
            ViewBag.cityList = new SelectList(_city.GetAll().Where(x => x.StateID == 1646).ToList(), "ID", "Name");
            if (!String.IsNullOrWhiteSpace(id))
            {
                int _id = Convert.ToInt32(id);
                tbl_TourismMaster model = _tourim.Get(_id);
                tbl_TourismMasterss abc = new tbl_TourismMasterss();
                abc.pkid = model.pkid;
                abc.city_fkid = model.city_fkid;
                abc.Sub_fkid = model.Sub_fkid;
                abc.Name = model.Name;
                abc.AddedDate = model.AddedDate;
                abc.Description = model.Description;
                abc.Longitude = model.Longitude;
                abc.Lotitude = model.Lotitude;
                abc.openingTime = model.openingTime;
                abc.ClosingTime = model.ClosingTime;
                abc.Lid = model.Lid;
                abc.Address = model.Address;
                abc.ContactNumber = model.ContactNumber;
                abc.InchargeName = model.InchargeName;
                return View(abc);
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddTourism(tbl_TourismMasterss model, HttpPostedFileBase[] files)
        {
            ViewBag.subList = new SelectList(_subcatTourism.GetAll(), "pkid", "SubCategory_Name");
            ViewBag.cityList = new SelectList(_city.GetAll().Where(x => x.StateID == 1646).ToList(), "ID", "Name");
            try
            {
               
                if (model.pkid == 0)
                {
                    tbl_TourismMaster abc = new tbl_TourismMaster();
                    abc.city_fkid = model.city_fkid;
                    abc.Sub_fkid = model.Sub_fkid;
                    abc.Name = model.Name;
                    abc.AddedDate = model.AddedDate;
                    abc.Description = model.Description;
                    abc.Longitude = model.Longitude;
                    abc.openingTime = model.openingTime;
                    abc.ClosingTime = model.ClosingTime;
                    abc.Lid = model.Lid;
                    abc.Lotitude = model.Lotitude;
                    abc.Address = model.Address;
                    abc.ContactNumber = model.ContactNumber;
                    abc.InchargeName = model.InchargeName;
                    abc.LastModifiedDate = DateTime.Now;
                    _tourim.Add(abc);
                    int maxid = _tourim.GetAll().Max(x => x.pkid);
                    if (files != null)
                    {
                        foreach (HttpPostedFileBase file in files)
                        {
                            //Checking file is available to save.  
                            if (file != null)
                            {
                                tbl_MultipleFileUpload fob = new tbl_MultipleFileUpload();
                                WebFunction web = new WebFunction();
                                fob.Master_fkid = 3;
                                fob.SubMaster_fkid = maxid;
                                fob.Lastmodifieddatetime = DateTime.Now;
                                fob.filepath = web.Storefile(file, 3);
                                _mfile.Add(fob);
                            }
                        }
                    }
                }
                else
                {
                    int _id = Convert.ToInt32(model.pkid);
                    tbl_TourismMaster abc = _tourim.Get(_id);

                    abc.city_fkid = model.city_fkid;
                    abc.Sub_fkid = model.Sub_fkid;
                    abc.Name = model.Name;
                    abc.AddedDate = model.AddedDate;
                    abc.Description = model.Description;
                    abc.Longitude = model.Longitude;
                    abc.Lotitude = model.Lotitude;
                    abc.openingTime = model.openingTime;
                    abc.ClosingTime = model.ClosingTime;
                    abc.Lid = model.Lid;
                    abc.Address = model.Address;
                    abc.ContactNumber = model.ContactNumber;
                    abc.InchargeName = model.InchargeName;
                    abc.LastModifiedDate = DateTime.Now;
                    _tourim.Update(abc);
                    foreach (HttpPostedFileBase file in files)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            tbl_MultipleFileUpload fob = new tbl_MultipleFileUpload();
                            WebFunction web = new WebFunction();
                            fob.Master_fkid = 3;
                            fob.SubMaster_fkid = model.pkid;
                            fob.Lastmodifieddatetime = DateTime.Now;
                            fob.filepath = web.Storefile(file, 3);
                            _mfile.Add(fob);
                        }
                    }
                }
                return RedirectToAction("AddTourism", "Administration");
            }
            catch (Exception e)
            {
                Commonfunction.LogError(e, Server.MapPath("~/Log.txt"));
                ViewBag.Exception = e.Message;
                return View();
            }
        }
        public ActionResult GetTourismList()
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
                var v = (from a in _tourim.GetAll()

                         select new
                         {
                             pkid = a.pkid,
                             comname = a.Name,
                             des = a.Description,
                             catg = (a.Sub_fkid != null ? _subcatTourism.Get(a.Sub_fkid).SubCategory_Name : ""),
                             adddate = a.AddedDate,
                             active = a.ContactNumber
                         });
                if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
                {
                    v = (from b in v.Where(x => x.comname.ToLower().Contains(search.ToLower()) || x.des.ToLower().Contains(search.ToLower()) || x.catg.ToLower().Contains(search.ToLower()) || x.adddate.ToString().Contains(search)) select b);
                }
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    //v = v.OrderBy(sortColumn, sortColumnDir);
                }
                recordsTotal = v.Count();
                var data = v.OrderByDescending(x => x.pkid).Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = "" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult DeleteTourism(int id)
        {
            try
            {
                WebFunction web = new WebFunction();
                tbl_TourismMaster abc = _tourim.Get(id);
                List<tbl_MultipleFileUpload> file = _mfile.GetAll().Where(x => x.SubMaster_fkid == abc.pkid).ToList();
                foreach (var item in file)
                {
                    web.DeleteImage(item.filepath);
                    _mfile.Remove(item,true);
                }
                _tourim.Remove(id, true);
                return Json("success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("failed", JsonRequestBehavior.AllowGet);
            }
        }
    }
}