using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class CuisineController : Controller
    {
        // GET: Cuisine/dishname or 
        public ActionResult Search(string name)
        {
            //return Content("Hello!<hr>"); // injectable scripts vulnerability.. 
            // But useful as a way of sending scripts back to client client callbacks or popup display etc.
            
            return Content(HttpUtility.HtmlEncode(name + "-Hello!<hr>")); // that's sending html as encoded, not raw..
            // View shows it as @Html.Raw .. simples :-D
        }

    }
}