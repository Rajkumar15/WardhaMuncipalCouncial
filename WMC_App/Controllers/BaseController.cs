using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMC_App.Helper;


namespace WMC_App.Controllers
{
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            try
            {
                int culture = 0;
                if (this.Session == null || this.Session["CurrentCulture"] == null)
                {
                    int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                    this.Session["CurrentCulture"] = culture;
                }
                else
                {
                    culture = (int)this.Session["CurrentCulture"];
                }
                // calling CultureHelper class properties for setting
                CultureHelper.CurrentCulture = culture;

                base.ExecuteCore();
            }
            catch {
               
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }

    }
}