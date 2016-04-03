using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
            // Warning: First line not compatible with Attribute based routing.
            // WebApiRouteConfig.RegisterRoutes(GlobalConfiguration.Configuration);// NOT new HttpConfiguration

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
