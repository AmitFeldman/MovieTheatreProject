using MovieTheatre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTheatre.DAL
{
    public class TheatreInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context DbContext)
        {
            var movies = new List<Movie>
            {
                new Movie{ID=1,Name="Harry Potter",Trailer="https://www.youtube.com/embed/mObK5XD8udk"},
                new Movie{ID=2,Name="Aladin",Trailer="https://www.youtube.com/embed/9HAX7GO2lGc"},
                new Movie{ID=3,Name="The Terminator"},
                new Movie{ID=4,Name="The Dictator"},
                new Movie{ID=5,Name="Toy Story 4",Trailer="https://www.youtube.com/embed/wmiIUN-7qhE"},
                new Movie{ID=6,Name="Spider-Man: Homecoming",Trailer="https://www.youtube.com/embed/n9DwoQ7HWvI"},
                new Movie{ID=7,Name="Finding Nemo",Trailer="https://www.youtube.com/embed/wZdpNglLbt8"},
                new Movie{ID=8,Name="Fast and Furious"}
            };
            movies.ForEach(s => DbContext.Movies.Add(s));
            DbContext.SaveChanges();

            var users = new List<User>
            {
                new User{ID=1,Username="Raz Korteran",Email="razguitar2@gmail.com",isManager=true,Password="1234"},
                new User{ID=2,Username="Amit Feldman",Email="amit@gmail.com",isManager=true,Password="1234"},
                new User{ID=3,Username="Samuel Zerbib",Email="samuel@gmail.com",isManager=true,Password="1234"},
                new User{ID=4,Username="Adir Hanuni",Email="adir@gmail.com",isManager=true,Password="1234"},
                new User{ID=5,Username="Igor Ruchlin",Email="igor@gmail.com",isManager=false,Password="1234"}
            };
            users.ForEach(s => DbContext.Users.Add(s));
            DbContext.SaveChanges();

            var ratings = new List<Rating>
            {
                new Rating{ID=1,UserID=1,MovieID=1,Stars=10,ReviewDate=DateTime.Parse("2019-08-04"),Review="I cried in the last movie"},
                new Rating{ID=2,UserID=1,MovieID=2,Stars=9,ReviewDate=DateTime.Parse("2019-08-04"),Review="Wow! Fantastic"},
                new Rating{ID=3,UserID=1,MovieID=3,Stars=7,ReviewDate=DateTime.Parse("2019-08-04"),Review="Cool but too long"},
                new Rating{ID=4,UserID=1,MovieID=4,Stars=10,ReviewDate=DateTime.Parse("2019-08-04"),Review="I peed in my pants from laugh"},
                new Rating{ID=5,UserID=1,MovieID=5,Stars=8,ReviewDate=DateTime.Parse("2019-08-04"),Review="Not only for children"},
                new Rating{ID=6,UserID=1,MovieID=6,Stars=9,ReviewDate=DateTime.Parse("2019-08-04"),Review="Not like the original but still pretty good"},
                new Rating{ID=7,UserID=2,MovieID=1,Stars=1,ReviewDate=DateTime.Parse("2019-08-04"),Review="I swear to god this is the worst movie ever"},
            };
            ratings.ForEach(s => DbContext.Ratings.Add(s));
            DbContext.SaveChanges();

            var locPoints = new List<LocationPoint>
            {
                new LocationPoint{ID=1, lat=32.0804808, lng=34.7805274},
                new LocationPoint{ID=2, lat=32.0567, lng=34.7817},
                new LocationPoint{ID=3, lat=32.1862752, lng=34.8692427},
                new LocationPoint{ID=4, lat=32.0686867, lng=34.8246812},
            };
            locPoints.ForEach(s => DbContext.LocationPoints.Add(s));
            DbContext.SaveChanges();
        }
    }
}