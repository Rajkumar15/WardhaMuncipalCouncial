using DataLayer.WMCModel;
using WMC_App.Models.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WMC_App.Models.BAL
{
    public class WebFunction
    {
        WMC_WebApplicationEntities db = new WMC_WebApplicationEntities();
        public string Storefile(HttpPostedFileBase filepost, int numb)
        {
            try
            {
                if (filepost != null)
                {
                    if (filepost.FileName != "")
                    {
                        DateTime date = DateTime.Now;
                        string dates = date.Day.ToString() + date.Month.ToString() + date.Year.ToString() + date.Hour.ToString() + date.Minute.ToString() + date.Second.ToString() + date.ToString("tt");
                        var path = "";
                        var returnpath = "";
                        if (numb == 1)
                        {
                            returnpath = "/UploadFiles/Complaints/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Complaints/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/Complaints/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 2)
                        {
                            returnpath = "/UploadFiles/Notice/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Notice/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/Notice/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 3)
                        {
                            returnpath = "/UploadFiles/Tourism/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Tourism/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/Tourism/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 4)
                        {
                            returnpath = "/UploadFiles/Userspic/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/Userspic/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/Userspic/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 5)
                        {
                            returnpath = "/UploadFiles/WardMember/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/WardMember/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/WardMember/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        if (numb == 6)
                        {
                            returnpath = "/UploadFiles/NewsAndUpdate/" + dates + filepost.FileName;
                            path = Path.Combine(HttpContext.Current.Server.MapPath("/UploadFiles/NewsAndUpdate/"), dates + filepost.FileName);
                            var filename = HttpContext.Current.Server.MapPath("/UploadFiles/NewsAndUpdate/" + dates + filepost.FileName);
                            filepost.SaveAs(path);
                        }
                        return returnpath;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public void DeleteImage(string file)
        {
            try
            {
                string fullPath = HttpContext.Current.Server.MapPath(file);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch
            {

            }
        }
        public string BlockScript(string input)
        {
            try
            {
                bool chk = true;
                if (input.Contains("<script>"))
                    chk = false;
                if (input.Contains("<script type=" + "text/javascript" + ">"))
                    chk = false;
                if (input.Contains("script"))
                    chk = false;
                if (input.Contains("</script>"))
                    chk = false;
                if (chk == false)
                    return "";
                else
                    return input;
            }
            catch
            {
                return "";
            }
        }


    }
}