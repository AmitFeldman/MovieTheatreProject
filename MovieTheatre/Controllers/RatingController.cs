using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieTheatre.DAL;
using MovieTheatre.Models;

namespace MovieTheatre.Controllers
{
    public class RatingController : Controller
    {
        private Context db = new Context();

        // GET: Rating
        public ActionResult Index(string username = "", string movieName = "", int stars = 0)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            var ratings = db.Ratings.Where(review => review.User.Username.Contains(username))
                                   .Where(review => review.Movie.Name.Contains(movieName));

            if (stars != 0)
            {
                ratings = ratings.Where(s => s.Stars == stars);
            }
                                   
            return View(ratings.ToList());
        }

        // GET: Rating/Details/5
        public ActionResult Details(int? id)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // GET: Rating/Create
        public ActionResult Create()
        {
            var currentUserID = (int)Session["CurrentUserID"];

            if (currentUserID == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "You have to log on or create an account to write a review!" });
            }

            return View();
        }

        // POST: Rating/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,MovieID,Review,Stars,ReviewDate")] Rating rating)
        {
            var currentUserID = (int)Session["CurrentUserID"];
            rating.ReviewDate = DateTime.Now;
            rating.UserID = currentUserID;

            if (currentUserID != 0)
            {
                if (ModelState.IsValid)
                {
                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    return Redirect("../User/Details/" + currentUserID);
                }
            }

            return View(rating);
        }

        // GET: Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Rating/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,MovieID,Review,Stars,ReviewDate")] Rating rating)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(rating).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rating);
        }

        // GET: Rating/Delete/5
        public ActionResult Delete(int? id)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rating rating = db.Ratings.Find(id);
            if (rating == null)
            {
                return HttpNotFound();
            }
            return View(rating);
        }

        // POST: Rating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            Rating rating = db.Ratings.Find(id);
            db.Ratings.Remove(rating);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
