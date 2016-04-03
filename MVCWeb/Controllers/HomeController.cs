using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using Microsoft.Owin.Security.Provider;


namespace MVCWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult List()
        {
            var persons = new List<Person>
            {
                new Person
                {
                    Id = 0,
                    Name = "PersonOne",
                    Addresses = new List<Address>
                    {
                        new Address
                        {
                            AddressId = 1,
                            AddressLine1 = "1 High Street",
                            City = "Daventry",
                            State = States.Northamptonshire
                        },
                        new Address
                        {
                            AddressId = 2,
                            AddressLine1 = "20 Main Street",
                            City = "Irvine",
                            State = States.Ayrshire
                        },
                    }

                }
            };

            return View(model: persons);
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Home Page.";
            ViewBag.Message += "I came from controller::action=" + RouteData.Values["controller"] + "::" +
                        RouteData.Values["action"];

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Responds to : /Home/VariousResults/5?retTypes=Redirect
        /// </summary>
        /// <param name="retType"></param>
        /// <returns></returns>
        public ActionResult VariousResults(int id, ReturnTypes retTypes)// ReturnTypes retType, string id)
        {
            //var retType = (ReturnTypes)(RouteData.Values["retType"]); // if param found with name, it doesnt go into RouteData values.

            switch (retTypes)
            {
                case ReturnTypes.RawHtml:
                    return Content("Hello world!" + retTypes, "text/plain");
                case ReturnTypes.FileDownload:
                    return File(Server.MapPath("~/Site/site.css"), "text/css");
                case ReturnTypes.HttpStatusCode:
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // or other statuscode results
                case ReturnTypes.JsonResult:
                    return new JsonResult() { Data = new Person() };
                    return Json(new Person()); // shortcut way of returning JsonResult
                case ReturnTypes.JsResult:
                    return JavaScript("<script>alert('hello from js result');</script>");
                case ReturnTypes.EmptyResult: return new EmptyResult(); // nothing else returnable
                case ReturnTypes.Redirect:
                    //return Redirect("/Home");
                    //return RedirectToAction("About", "Home", new { id = 1, name = "myhome" });
                    return RedirectToRoute("Cuisine", new { name = "redirectedDish" });
            }

            return null;

            //return View();
        }

        public enum ReturnTypes
        {
            RawHtml, FileDownload, HttpStatusCode, JsonResult, JsResult, EmptyResult, Redirect
        }
    }

}