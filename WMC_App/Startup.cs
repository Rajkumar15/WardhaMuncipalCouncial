using Microsoft.Owin;
using Owin;
[assembly: OwinStartupAttribute(typeof(WMC_App.Startup))]
namespace WMC_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            RegisterAuth(app);
        }
        public static void RegisterAuth(IAppBuilder app)
        {
            // other code
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            // other code
        }
    }
}
