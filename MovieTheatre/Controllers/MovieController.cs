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
        private Random rnd = new Random();

        public class MovieDetailsModel
        {
            public Movie movie { get; set; }
            public List<MovieTheatre.Models.Rating> movieReviews { get; set; }
        }

        // GET: Movie
        public ActionResult Index(string movieName = "", string year = "", string director = "", string genre = "")
        {
            var movies = db.Movies
                   .Where(movie => movie.Name.Contains(movieName))
                   .Where(movie => movie.Year.Contains(year))
                   .Where(movie => movie.Director.Contains(director))
                   .Where(movie => movie.Genre.Contains(genre));

            GetMovieDetails(movies);

            HigherRankedMovies();

            return View(movies.ToList());
        }

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

        public ActionResult GetDirectorData()
        {
            JsonResult result = new JsonResult();

            // Get count of each genre
            var directors = (from m in db.Movies
                          group m by m.Director into g
                          orderby g.Count() descending
                          select new ChartData { label = g.Key, amount = g.Count() });

            var directorList = directors.ToList();
            result = this.Json(directorList, JsonRequestBehavior.AllowGet);

            return result;
        }

        // Get the movies from the webservice and saves them in db
        public void GetMovieDetails(IQueryable<Movie> movies)
        {
            var client = new WebClient();
            string genre;
            foreach (var item in movies)
            {
                if (item.Poster == null || item.Description == null ||
                    item.Director == null || item.Year == null ||
                    item.Genre == null)
                {
                    string httpString = "http://www.omdbapi.com/?t=" +
                                  item.Name +
                                  "&y=" + item.Year +
                                  "&apikey=4c2cc9b2";
                    var json = client.DownloadString(httpString);
                    var data = (JObject)JsonConvert.DeserializeObject(json);

                    if (data["Response"].Value<string>() == "True")
                    {
                        item.Poster = data["Poster"].Value<string>();
                        item.Description = data["Plot"].Value<string>();
                        item.Director = data["Director"].Value<string>();
                        item.Year = data["Year"].Value<string>();
                        genre = data["Genre"].Value<string>();
                        item.Genre = genre.Contains(",") ? genre.Substring(0, genre.IndexOf(','))
                                                         : genre;
                    }
                }
            }
            db.SaveChanges();
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            MovieDetailsModel detailsModel = new MovieDetailsModel();

            detailsModel.movie = movie;
            detailsModel.movieReviews = db.Ratings.Where(review => review.MovieID == movie.ID).ToList();

            return View(detailsModel);
        }

        public ActionResult AutocompleteMovieData(string movieName)
        {
            var client = new WebClient();
            Movie movie = new Movie();
            string httpString = "";
            /* httpString = "http://www.omdbapi.com/?t=" +
                                   name +
                                   //  "&y=" + item.Year +
                                   "&apikey=4c2cc9b2";
             var json = client.DownloadString(httpString);
             var data = (JObject)JsonConvert.DeserializeObject(json);
             if (data["Response"].Value<string>() == "True")
             {
                 movie.Name = data["Title"].Value<string>();
                 movie.Poster = data["Poster"].Value<string>();
                 movie.Description = data["Plot"].Value<string>();
                 movie.Director = data["Director"].Value<string>();
                 movie.Year = data["Year"].Value<string>();
                 movie.Genre = data["Genre"].Value<string>();
             }*/
            return RedirectToAction("Create", movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

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
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
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
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
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
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

            if (isCurrentUserManager == false)
            {
                return RedirectToAction("Index", "Error", new { message = "You're not allowed here!" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
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
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var isCurrentUserManager = (Boolean)Session["isCurrentUserManager"];

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
