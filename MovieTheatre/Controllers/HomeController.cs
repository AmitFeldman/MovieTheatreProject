using MovieTheatre.DAL;
using MovieTheatre.Models;
using System;
using System.Data.Entity;
using System.Data;
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

        public class HomeModel
        {
            public List<MovieTheatre.Models.Movie> suggestedMovies { get; set; }
            public List<MovieTheatre.Models.Rating> latestReviews { get; set; }
            public List<MovieTheatre.Models.LocationPoint> locationPoints { get; set; }
        }

        public ActionResult Index()
        {
            try
            {
                var currentUserID = Session["CurrentUserID"];
                var isCurrentUserManager = Session["isCurrentUserManager"];
                
                if (currentUserID == null || isCurrentUserManager == null)
                {
                    Session.Add("CurrentUserID", 0);
                    Session.Add("isCurrentUserManager", false);
                }
            }
            catch {
                Session.Add("CurrentUserID", 0);
                Session.Add("isCurrentUserManager", false);
            }

            // TODO: Add logic for suggested movies and latest reviews
            HomeModel homeModel = new HomeModel();
            homeModel.suggestedMovies = db.Movies.Take(5).ToList();
            homeModel.latestReviews = db.Ratings.OrderByDescending(review => review.ReviewDate).Take(3).ToList();

            return View(homeModel);
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

        public ActionResult LogIn(string button)
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "Email,Password")]User u)
        {
            if (u.Email != null && u.Password != null)
            {
                var currentUser =
                (from user in db.Users
                 where user.Email.Equals(u.Email) && user.Password.Equals(u.Password)
                 select user).FirstOrDefault();
                db.Users.Where((user) => user.Email.Equals(u.Email) && user.Password.Equals(u.Password)).FirstOrDefault();

                if (currentUser != null && currentUser.ID != 0)
                {
                    Session.Remove("CurrentUserID");
                    Session.Remove("isCurrentUserManager");

                    Session.Add("CurrentUserID", currentUser.ID);
                    Session.Add("isCurrentUserManager", currentUser.isManager);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "User not found";
                }
            }

            return View(u);
        }

        public ActionResult Logout()
        {
            Session.Remove("CurrentUserID");
            Session.Remove("isCurrentUserManager");

            Session.Add("CurrentUserID", 0);
            Session.Add("isCurrentUserManager", false);
            return Redirect("LogIn");
        }

        /*public ActionResult Register()
        {
            Session.Remove("CurrentUser");
            Session.Add("CurrentUser",0);
            //return Redirect("../User/Create");
            return RedirectToAction("../User/Create");
        }*/

        public ActionResult GetLocationPoint()
        {
            JsonResult result = new JsonResult();

            // Get count of each genre
            var points = (from m in db.LocationPoints
                          select new LocationPoint { lat = m.lat, lng = m.lng });

            var pointList = points.ToList();
            result = this.Json(pointList, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}