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
        public int numberOfSuggested = 0;
        const int minNumOfMovies = 3,
                  maxNumOfMovies = 5;

        public class HomeModel
        {
            public List<MovieTheatre.Models.Movie> suggestedMovies { get; set; }
            public List<MovieTheatre.Models.Rating> latestReviews { get; set; }
        }

        public ActionResult Index()
        {
            // Finding connected user
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
            catch
            {
                // Not found
                Session.Add("CurrentUserID", 0);
                Session.Add("isCurrentUserManager", false);
            }

            HomeModel homeModel = new HomeModel();
            homeModel.suggestedMovies = SuggestedMovies();
            homeModel.latestReviews = db.Ratings.OrderByDescending(review => review.ReviewDate).Take(3).ToList();

            return View(homeModel);
        }

        // Get the suggestedMovies
        public List<Movie> SuggestedMovies()
        {
            int currentUserID = (int)Session["CurrentUserID"];
            List<Movie> suggestedMovies = new List<Movie>();

            // Get the favorites genres by average rating at descending level
            var genres = (from m in db.Movies
                          join r in db.Ratings on m.ID equals r.MovieID
                          where r.UserID == currentUserID
                          group r by m.Genre into g
                          orderby g.Average(p => p.Stars) descending
                          select new { genre = g.Key }).ToList();

            // Get all the movies user already rated
            var movies = (from r in db.Ratings
                          join m in db.Movies on r.MovieID equals m.ID
                          where r.UserID == currentUserID
                          select m).ToList();

            // Getting movies unrated from each genre
            for (var i = 0; i < genres.Count() && numberOfSuggested < minNumOfMovies; i++)
            {
                GetMoviesFromGenre(genres[i].genre, movies, suggestedMovies);
            }

            // If not found enough movies add random genre's movies
            if (numberOfSuggested < minNumOfMovies)
            {
                var randGenres = (from m in db.Movies
                                  join r in db.Ratings on m.ID equals r.MovieID into rm
                                  from genre in rm.DefaultIfEmpty()
                                  select new { genre = m.Genre, rating = rm.Count() });

                // Remove rated genres
                randGenres = randGenres.Where(p => p.rating == 0);

                // Find the movies
                for (var i = 0; i < randGenres.ToArray().Length && numberOfSuggested < minNumOfMovies; i++)
                {
                    GetMoviesFromGenre(randGenres.ToArray()[i].genre, new List<Movie>(), suggestedMovies);
                }
            }
            return suggestedMovies;
        }

        // Get the movies he didn't rate from genre
        public void GetMoviesFromGenre(string genre, List<Movie> ratedMovies, List<Movie> returnedList)
        {
            // Get all the movies from genre
            var suggestedMovies = (from m in db.Movies
                                   where m.Genre.Equals(genre)
                                   select m).ToList();

            // Remove the movies he already rated
            foreach (Movie movie in ratedMovies)
            {
                if (suggestedMovies.Contains(movie))
                    suggestedMovies.Remove(movie);
            }

            // Add the rest
            foreach (Movie movie in suggestedMovies)
            {
                if (numberOfSuggested == maxNumOfMovies)
                    return;
                numberOfSuggested++;
                returnedList.Add(movie);
            }
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
                // Trying to find user
                var currentUser =
                (from user in db.Users
                 where user.Email.Equals(u.Email) && user.Password.Equals(u.Password)
                 select user).FirstOrDefault();

                if (currentUser != null && currentUser.ID != 0)
                {
                    // Logged in
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


        public ActionResult GetLocationPoints()
        {
            JsonResult result = new JsonResult();

            List<LocationPoint> locationPoints = db.LocationPoints.ToList();

            result = this.Json(locationPoints, JsonRequestBehavior.AllowGet);

            return result;
        }
    }
}