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
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace WMC_App.Controllers
{
    public class UploadFileAppController : ApiController
    {
        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFilesSyS(string ty = "")
        {
            try
            {

                string result = "";
                List<returnAPiFlag> abc = new List<returnAPiFlag>();

                var filePath = "";
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                    {
                        var returnpath = "";
                        var postedFile = httpRequest.Files[i];
                        ty = Regex.Replace(ty, "^\"|\"$", "");
                        int type = Convert.ToInt32(ty);
                        DateTime date = DateTime.Now;
                        string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                        if (type == 1)
                        {
                            returnpath = "/UploadFiles/Complaints/" + dates + postedFile.FileName;
                            filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Complaints/"), dates + postedFile.FileName);
                        }
                        else if (type == 2)
                        {
                            returnpath = "/UploadFiles/Notice/" + dates + postedFile.FileName;
                            filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Notice/"), dates + postedFile.FileName);
                        }
                        else if (type == 3)
                        {
                            returnpath = "/UploadFiles/Tourim/" + dates + postedFile.FileName;
                            filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Notice/"), dates + postedFile.FileName);
                        }
                        if (File.Exists(filePath))
                        {
                            result = "File Already exist please change file name.....";
                            //abc.status = result;
                            //abc.flag = false;
                        }
                        else
                        {
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                            result = returnpath;
                            returnAPiFlag gh = new returnAPiFlag();
                            gh.status = result;
                            gh.flag = true;
                            abc.Add(gh);
                        }

                    }
                    return Json(abc);
                }
                else
                {
                    result = "File not select.";
                    returnAPiFlag gh = new returnAPiFlag();
                    gh.status = result;
                    gh.flag = false;
                    abc.Add(gh);
                    return Json(abc);
                }

            }
            catch (Exception e)
            {
                returnAPiFlag abc = new returnAPiFlag();
                string aaa = "File Save Failed";
                abc.status = aaa;
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> DeleteUploadFilesSyS(Deleteuploadfile model)
        {
            try
            {
                string result = "";
                returnAPiFlag abc = new returnAPiFlag();

                string path = HttpContext.Current.Server.MapPath(model.filepath);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    abc.status = model.filepath;
                    abc.flag = true;                   
                }
                else
                {
                    abc.status = "File Not Available.";
                    abc.flag = false;
                }
                return Json(abc);
            }
            catch (Exception e)
            {
                returnAPiFlag abc = new returnAPiFlag();
                string aaa = "File Delete Failed";
                abc.status = aaa;
                abc.flag = false;
                return Json(abc);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("api/UploadFileApp/MediaUpload")]
        public async Task<IHttpActionResult> MediaUpload()
        {
            WMC_WebApplicationEntities db = new WMC_WebApplicationEntities();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data
            NameValueCollection model = provider.FormData;
            //access files
            IList<HttpContent> files = provider.Files;

            
            returnAPiFlag abcd = new returnAPiFlag();
            tbl_ComplaintMaster abc = new tbl_ComplaintMaster();
            try
            {              
                abc.pkid = Convert.ToInt32(model["pkid"]);
                abc.Category_fkid = Convert.ToInt32(model["Category_fkid"]);
                abc.ComplaintDescription = model["ComplaintDescription"];
                abc.ComplaintName = model["ComplaintName"];
                abc.AddedDate = DateTime.Now;
                abc.Active = Convert.ToInt32(model["Active"]);
                abc.Longitutde = model["Longitutde"];
                abc.Latitude = model["Latitude"];
                abc.City_fkid = Convert.ToInt32(model["City_fkid"]);
                abc.ImagePath = model["ImagePath"];
                abc.ShowIdentity = model["ShowIdentity"];
            }
            catch {
                abcd.status = "Entered Required Field";
                abcd.flag = true;
                return Json(abcd);
            }
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();
              
                if (abc.pkid == 0)
                {
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                        {
                            WebFunction web = new WebFunction();
                            var postedFile = httpRequest.Files[i];
                            DateTime date = DateTime.Now;
                            string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                            var returnpath = "/UploadFiles/Complaints/" + dates + postedFile.FileName;
                           var filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Complaints/"), dates + postedFile.FileName);
                           postedFile.SaveAs(filePath);
                           abc.ImagePath = returnpath;
                        }
                    }
                    abc.LastModifiedDate = DateTime.Now;
                    abc.User_fkid = id;
                    db.tbl_ComplaintMaster.Add(abc);
                    db.SaveChanges();
                    abcd.status = "Data Save Successfully";
                    abcd.flag = true;
                }
                else
                {
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                        {
                            WebFunction web = new WebFunction();
                            var postedFile = httpRequest.Files[i];
                            DateTime date = DateTime.Now;
                            string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                            var returnpath = "/UploadFiles/Complaints/" + dates + postedFile.FileName;
                            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Complaints/"), dates + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            abc.ImagePath = returnpath;
                        }
                    }
                    abc.LastModifiedDate = DateTime.Now;
                    db.tbl_ComplaintMaster.Attach(abc);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abcd.status = "Data updated Successfully";
                    abcd.flag = true;
                }
                return Json(abcd);
            }
            catch (Exception e)
            {
                abcd.status = "Data Save Failed";
                abcd.flag = false;
                return Json(abc);
            }                       
        }


        [Authorize]
        [HttpPost]
        [Route("api/UploadFileApp/SaveSuggestion")]
        public async Task<IHttpActionResult> SaveSuggestion()
        {
            WMC_WebApplicationEntities db = new WMC_WebApplicationEntities();
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());
            //access form data
            NameValueCollection model = provider.FormData;
            //access files
            IList<HttpContent> files = provider.Files;


            returnAPiFlag abcd = new returnAPiFlag();
            tbl_suggestionMaster abc = new tbl_suggestionMaster();
            try
            {
                abc.pkd = Convert.ToInt32(model["pkd"]);               
                abc.title = model["title"];
                abc.description = model["description"];
                abc.adddate = DateTime.Now;
                abc.status = 1;
            }
            catch
            {
                abcd.status = "Entered Required Field";
                abcd.flag = true;
                return Json(abcd);
            }
            try
            {
                string id;
                id = User.Identity.GetUserId();
                id = RequestContext.Principal.Identity.GetUserId();

                if (abc.pkd == 0)
                {
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                        {
                            WebFunction web = new WebFunction();
                            var postedFile = httpRequest.Files[i];
                            DateTime date = DateTime.Now;
                            string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                            var returnpath = "/UploadFiles/Suggestion/" + dates + postedFile.FileName;
                            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Suggestion/"), dates + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            abc.imagepath = returnpath;
                        }
                    }
                  
                    abc.user_fkid = id;
                    db.tbl_suggestionMaster.Add(abc);
                    db.SaveChanges();
                    abcd.status = "Data Save Successfully";
                    abcd.flag = true;
                }
                else
                {
                    var httpRequest = HttpContext.Current.Request;
                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        for (int i = 0; i <= httpRequest.Files.Count - 1; i++)
                        {
                            WebFunction web = new WebFunction();
                            var postedFile = httpRequest.Files[i];
                            DateTime date = DateTime.Now;
                            string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                            var returnpath = "/UploadFiles/Suggestion/" + dates + postedFile.FileName;
                            var filePath = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Suggestion/"), dates + postedFile.FileName);
                            postedFile.SaveAs(filePath);
                            abc.imagepath = returnpath;
                        }
                    }                 
                    db.tbl_suggestionMaster.Attach(abc);
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    abcd.status = "Data updated Successfully";
                    abcd.flag = true;
                }
                return Json(abcd);
            }
            catch (Exception e)
            {
                abcd.status = "Data Save Failed";
                abcd.flag = false;
                return Json(abc);
            }
        }
    }
}
