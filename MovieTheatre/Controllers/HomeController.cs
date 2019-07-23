using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTheatre.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // System.Web.Security.FormsAuthentication.SetAuthCookie("54", false);
            // string userid = HttpContext.User.Identity.Name;
            //HttpContext.Current.Session["userId"] = 5;
            Session.Add("CurrentUser", 54);
            int userid = (int)Session["CurrentUser"];
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
    }
}