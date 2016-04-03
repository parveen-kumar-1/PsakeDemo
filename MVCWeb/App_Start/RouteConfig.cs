using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Routing basics.
        // http://stephenwalther.com/archive/2009/02/06/chapter-2-understanding-routing
           
            // Http Parameter binding 
        // http://www.tugberkugurlu.com/archive/asp-net-web-api-catch-all-route-parameter-binding

            // configure the route engine with map route..

            //routes.MapHttpRoute() // guess used for WebApi, can set Endpoint Http Message Handler.
            // works for /Cuisine, /Cuisine/
            // Only Segments map to the RouteData values dictionary, no other param
            routes.MapRoute(name: "Cuisine", url: "Cuisine/{name}",
                defaults: new
                {
                    controller = "Cuisine",
                    action = "Search",
                    name = UrlParameter.Optional // best to use this otherwise, it cant find ?name=value
                }); // PUT only a very specific route here, otherwise it will mask the usual controller/action format

            // this route maps to every controller/action/[params]
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            // param constraints
            routes.MapRoute(
                "BlogArchive",
                "Archive/{entryDate}",
                new { controller = "Blog", action = "Archive" },
                //new {entryDate = @"d{2}-d{2}-d{4}"}             // date param constraint 
                new { method = new HttpMethodConstraint("POST") } // HTTP Method constraint

                );
            // custom constraint IRouteConstraint (Authenticated ..)
            routes.MapRoute(
                "Admin",
                "Admin/{action}",
                new {controller = "Admin"},
                new {Auth = new AuthenticatedConstraint()}
                );

            // Catch all consumed as follows:
            //  public string Index(string values)
            //  {
            //      var brokenValues = values.Split('/');
            routes.MapRoute(
                "SortRoute",
                "Sort/{*values}",
                new {controller = "Sort", action = "Index"}
                );
        }

    }

    // Custom constraint
    public class AuthenticatedConstraint : IRouteConstraint
    {

        public bool Match
            (
            HttpContextBase httpContext,
            Route route,
            string parameterName,
            RouteValueDictionary values,
            RouteDirection routeDirection
            )
        {
            return httpContext.Request.IsAuthenticated;
        }

    }
}
