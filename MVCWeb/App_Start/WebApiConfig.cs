using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace MVCWeb
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // IMPORTANT: http://www.asp.net/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2

            // Attribute routing.
            config.MapHttpAttributeRoutes();

            // Convention-based routing.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { RouteParameter.Optional });

//                constraints: new CompoundRouteConstraint({
//                    new BoolRouteConstraint().IfNotNull(null)
//});

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApiByAction",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional },
            //    constraints: new { HttpVerbs.Post }
            //    );

            config.EnsureInitialized();
        }
    }
}
