using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Provider;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using MVCWeb;
using Owin;

// Can be disabled by Web.config appSetting key=owin:AutomaticAppStartup value=false
[assembly: OwinStartupAttribute(typeof(MVCWeb.Startup))]

 // This attrib, as of .NET 4.5, can be used multiple times within an assembly
[assembly: PreApplicationStartMethod(typeof(MVCWeb.PreAppStartup), "RunPreAppStartup")]

namespace MVCWeb
{
    public partial class Startup
    {
        // Method wont fire if disabled by appSetting key=owin:AutomaticAppStartup value=false
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

    public class PreAppStartup
    {
        //http://stackoverflow.com/questions/21462777/webactivatorex-vs-owinstartup
        //The order of execution of methods depending on the attributes used.
        // 1. System.Web.PreApplicationStartMethodAttribute
        //      OwinHttpModule calls method as per OwinStartupAttribute 
        //      which in itself is injected in using System.Web.PreApplicationStartMethodAttribute (internally)
        // 2. WebActivatorEx.PreApplicationStartMethodAttribute
        // 3. Global.asax (Application_Start method)
        // 4. OwinStartupAttribute
        public static void RunPreAppStartup()
        {
            // Inject other Http modules into Http pipeline here..

            // OLD approach - doesnt work now.. ignore..
            //var myHttpModule = new MyHttpModule();
            // DynamicHttpApplication.RegisterModule(application => { return myHttpModule; }); doesnt fire mymodule
            // Global.asax.cs base is DynamicHttpApplication
            // END old approach.. 

            // NEW approach: http://stackoverflow.com/questions/239802/programmatically-register-httpmodules-at-runtime
            DynamicModuleUtility.RegisterModule(typeof(MyHttpModule));
            // and more modules here..

            Trace.WriteLine("Pre Application Startup fired...");
        }

        //public abstract class DynamicHttpApplication : HttpApplication
        //{
        //    public static void RegisterModule(Func<HttpApplication, IHttpModule> moduleFactory)
        //    {
        //    }
        //}

        public class MyHttpModule : IHttpModule
        {
            public void Init(HttpApplication context)
            {
                Trace.Write("MyHttpModule initialized..");
            }

            public void Dispose()
            {
                Trace.Write("MyHttpModule disposed..");
            }
        }
    }
}
