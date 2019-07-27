using MovieTheatre.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTheatre.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        public string[] Posters { get; set; }

        public ActionResult Index()
        {
            // System.Web.Security.FormsAuthentication.SetAuthCookie("54", false);
            // string userid = HttpContext.User.Identity.Name;
            //HttpContext.Current.Session["userId"] = 5;
            Session.Add("CurrentUser", 54);
            int userid = (int)Session["CurrentUser"];


            var tableBuilder = "";
            var columns = 3;
            var data = db.Movies;
            var movieList = data.ToList();

            for (var index = 0; index < movieList.Count; index++)
            {
                var item = movieList[index];
                if (index % columns == 0)
                {
                    if (index > 0) tableBuilder += "</tr>";
                    tableBuilder += "<tr width=\"100%\">";
                }

                tableBuilder += "<td text-align=center>";
                // tableBuilder += "<tr><img src="+ @Html.DisplayFor(modelItem => item.Poster +") height=\"250px\" width=\"250px\"></tr>";
                tableBuilder += item.Name + "</br>";
                tableBuilder += item.Year + "</br>";
                tableBuilder += item.Genre + "</br></br></br></br>";
                tableBuilder += "</td>";
            }

            tableBuilder += "</tr>";
            ViewData["myTable"] = tableBuilder;

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

        public ActionResult LogIn()
        {
            ViewBag.Message = "Your login page.";

            return View();
        }
    }
}