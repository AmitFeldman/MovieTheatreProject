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
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            var ratings = db.Ratings.Where(review => review.User.Username.Contains(username) || username == null)
                                    .Where(review => review.Movie.Name.Contains(movieName) || movieName == null)
                                    .Where(review => review.Stars == stars || stars == 0);                        
            return View(ratings.ToList());
        }

        // GET: Rating/Details/5
        public ActionResult Details(int? id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

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
        public ActionResult Create(int id = -1)
        {
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return RedirectToAction("Index", "Error", new { message = "You shouldn't be here!" });
            }

            ViewBag.movie = movie;

            Boolean isUserLoggedOn = MovieTheatre.Util.SessionManager.isUserLoggedOn(Session);

            if (!isUserLoggedOn)
            {
                return RedirectToAction("Index", "Error", new { message = "You have to log on or create an account to write a review!" });
            }

            Rating rating = new Rating();
            rating.MovieID = movie.ID;

            return View(rating);
        }

        // POST: Rating/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,MovieID,Review,Stars,ReviewDate")] Rating rating)
        {
            Boolean isUserLoggedOn = MovieTheatre.Util.SessionManager.isUserLoggedOn(Session);

            if (rating.MovieID == 0)
            {
                return RedirectToAction("Index", "Error", new { message = "There was an error in submitting this review. Please try again." });
            }

            if (isUserLoggedOn)
            {
                if (ModelState.IsValid)
                {
                    rating.ReviewDate = DateTime.Now;
                    int currentUserID = MovieTheatre.Util.SessionManager.getUserID(Session);
                    rating.UserID = currentUserID;

                    db.Ratings.Add(rating);
                    db.SaveChanges();
                    return RedirectToAction("Details/" + currentUserID, "User");
                }
            }

            return View(rating);
        }

        // GET: Rating/Edit/5
        public ActionResult Edit(int? id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

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
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

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
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

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
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

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
