using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieTheatre.DAL;
using MovieTheatre.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovieTheatre.Controllers
{
    public class MovieController : Controller
    {
        private Context db = new Context();

        public class MovieDetailsModel
        {
            public Movie movie { get; set; }
            public List<MovieTheatre.Models.Rating> movieReviews { get; set; }
        }

        // GET: Movie
        public ActionResult Index(string movieName = "", string year = "", string director = "", string genre = "")
        {
            var movies = db.Movies;

            // Get details about movies from webservice
            GetMovieDetails(movies.ToList());

            // Filter movies
            var showedMovies = db.Movies
                   .Where(movie => movie.Name.Contains(movieName) || movieName == "")
                   .Where(movie => movie.Year.Contains(year) || year == "")
                   .Where(movie => movie.Director.Contains(director) || director == "")
                   .Where(movie => movie.Genre.Contains(genre) || genre == "");
            
            // Get the Higher ranked movies
            HigherRankedMovies();

            return View(showedMovies.ToList());
        }

        // Get the genres data for the graph
        public ActionResult GetGenreData()
        {
            JsonResult result = new JsonResult();

            // Get count of each genre
            var genres = (from m in db.Movies
                          group m by m.Genre into g
                          orderby g.Count() descending
                          select new ChartData { label = g.Key, amount = g.Count() });

            var genreList = genres.ToList();
            result = this.Json(genreList, JsonRequestBehavior.AllowGet);

            return result;
        }

        // Get the directors data for the graph
        public ActionResult GetDirectorData()
        {
            JsonResult result = new JsonResult();

            // Get count of each directors
            var directors = (from m in db.Movies
                             group m by m.Director into g
                             orderby g.Count() descending
                             select new ChartData { label = g.Key, amount = g.Count() });

            var directorList = directors.ToList();
            result = this.Json(directorList, JsonRequestBehavior.AllowGet);

            return result;
        }

        // Get the movies from the webservice and saves them in db
        public void GetMovieDetails(List<Movie> movies)
        {
            MovieTheatre.Util.MovieManager.UpdateMovies(movies, db);
        }

        // Generate the 10 higher rank movies
        public void HigherRankedMovies()
        {
            var movies = (from m in db.Movies
                          join r in db.Ratings on m.ID equals r.MovieID
                          group r by m.Name into g
                          orderby g.Average(p => p.Stars) descending
                          select new { indexLabel = g.Key, y = g.Average(p => p.Stars) }).Take(10);

            ViewBag.movieRating = movies;
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Movie not found!" });
            }

            MovieDetailsModel detailsModel = new MovieDetailsModel();

            detailsModel.movie = movie;
            detailsModel.movieReviews = db.Ratings.Where(review => review.MovieID == movie.ID).ToList();

            return View(detailsModel);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Genre,Description,Year,Director,Poster,Trailer")] Movie movie)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                List<Movie> duplicateMovies = db.Movies
                    .Where((m) => m.Name == movie.Name )
                    .ToList();

                if (duplicateMovies.Count > 0)
                {
                    return RedirectToAction("Index", "Error", new { message = "Movie already exists!" });
                }

                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
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

            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Movie not found!" });
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Genre,Description,Year,Director,Poster,Trailer")] Movie movie)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                List<Movie> duplicateMovies = db.Movies
                    .Where((m) => m.Name == movie.Name)
                    .ToList();

                if (duplicateMovies.Count > 0)
                {
                    return RedirectToAction("Index", "Error", new { message = "Movie already exists!" });
                }

                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
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
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return RedirectToAction("Index", "Error", new { message = "Movie not found!" });
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Boolean isCurrentUserManager = MovieTheatre.Util.SessionManager.isCurrentUserManager(Session);

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);

            List<Rating> ratings = db.Ratings.Where((review) => review.MovieID == id).ToList();
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
