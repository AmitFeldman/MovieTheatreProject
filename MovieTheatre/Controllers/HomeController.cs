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

        public ActionResult Index()
        {
            try
            {
                var userid = Session["CurrentUser"];
                if (userid == null)
                    Session.Add("CurrentUser", 0);
            }
            catch { Session.Add("CurrentUser", 0); }

            // TODO: Add logic for suggested movies
            ViewData.Model = db.Movies.Take(3). ToList();

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

        public ActionResult LogIn(string button)
        {
            Session.Add("CurrentUser", 0);
            ViewBag.Message = "Your login page.";

            if (button == "Login")
            {
                //db.User.Where(s => s.Name.Contains(userName));
            }

            return View();
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn([Bind(Include = "Email,Password")]User u)
        {

            if (u.Email != null && u.Password != null)
            {
                //var v = db.User.Where(a = > a.Email.Equals(u.Email) && a.Password.Equals(u.Password)).FirstOrDefault();
                var v =
                (from user in db.User
                 where user.Email.Equals(u.Email) && user.Password.Equals(u.Password)
                 select user.ID).FirstOrDefault();
                if (v != 0)
                {
                    Session.Remove("CurrentUser");
                    Session.Add("CurrentUser", v.ToString());
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
            Session.Remove("CurrentUser");
            return Redirect("LogIn");
        }

        /*public ActionResult Register()
        {
            Session.Remove("CurrentUser");
            Session.Add("CurrentUser",0);
            //return Redirect("../User/Create");
            return RedirectToAction("../User/Create");
        }*/
    }
}