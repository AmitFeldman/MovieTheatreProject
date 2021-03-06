﻿using System;
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
    public class UserController : Controller
    {
        private Context db = new Context();

        public class UserDetailsModel
        {
            public User user { get; set; }
            public List<MovieTheatre.Models.Rating> userReviews { get; set; }
        }

        // GET: User
        public ActionResult Index(string userName)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            var users = from m in db.Users
                        select m;

            if (!String.IsNullOrEmpty(userName))
            {
                users = users.Where(s => s.Username.Contains(userName));
            }

            return View(users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Error", new { message = "User not found!" });
            }

            UserDetailsModel detailsModel = new UserDetailsModel();
            detailsModel.user = user;
            detailsModel.userReviews = db.Ratings
                .Where(review => review.UserID == user.ID)
                .OrderByDescending(review => review.ReviewDate)
                .ToList();

            return View(detailsModel);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Username,Email,Password")] User user)
        {
            Boolean isUserLoggedOn = MovieTheatre.Util.SessionManager.isUserLoggedOn(Session);
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (ModelState.IsValid)
            {
                List<User> duplicateUsers = db.Users
                    .Where((u) => u.Email == user.Email || u.Username == user.Username)
                    .ToList();

                if (duplicateUsers.Count > 0)
                {
                    return RedirectToAction("Index", "Error", new { message = "This Username/Email is already in use. Try again!" });
                }

                db.Users.Add(user);
                db.SaveChanges();

                if (isUserLoggedOn && isCurrentUserManager)
                    return RedirectToAction("Index");
                else
                    return RedirectToAction("../Home/Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Error", new { message = "User not found!" });
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Username,Email,Password")] User user)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                List<User> duplicateUsers = db.Users
                    .Where((u) => u.Email == user.Email || u.Username == user.Username)
                    .ToList();
                
                if (duplicateUsers.Count > 0)
                {
                    return RedirectToAction("Index", "Error", new { message = "This Username/Email is already in use. Try again!" });
                }

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index", "Error", new { message = "User not found!" });
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            User user = db.Users.Find(id);
            db.Users.Remove(user);

            List<Rating> ratings = db.Ratings.Where((review) => review.UserID == id).ToList();
            ratings.ForEach((rating) => db.Ratings.Remove(rating));

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
