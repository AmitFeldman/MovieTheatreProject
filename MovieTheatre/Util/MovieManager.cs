using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Web.Mvc;
using MovieTheatre.DAL;
using MovieTheatre.Models;
namespace MovieTheatre.Util
{
    public static class MovieManager
    {

        public static void UpdateMovies(List<Movie> movies, Context db)
        {
            var client = new WebClient();
            string genre;
            foreach (var item in movies)
            {
                // If need to find new information about the movie
                if (string.IsNullOrEmpty(item.Poster) || 
                    string.IsNullOrEmpty(item.Description) ||
                    string.IsNullOrEmpty(item.Director) ||
                    string.IsNullOrEmpty(item.Year) ||
                    string.IsNullOrEmpty(item.Genre))
                {
                    string httpString = "http://www.omdbapi.com/?t=" +
                                  item.Name +
                                  "&y=" + item.Year +
                                  "&apikey=4c2cc9b2";
                    try
                    {
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
                    catch
                    {
                        try
                        {
                            // Backup webservice
                            httpString = "https://api.themoviedb.org/3/search/movie?" +
                                         "api_key=e62278365dd3d1d53e0415f424532c2a" +
                                         "&query=" + item.Name;
                            var json = client.DownloadString(httpString);
                            var data = (JObject)JsonConvert.DeserializeObject(json);

                            if (data["total_results"].Value<string>() != "0")
                            {
                                item.Poster = "https://image.tmdb.org/t/p/w500" + data["results"][0]["poster_path"].Value<string>();
                                item.Description = data["results"][0]["overview"].Value<string>();
                                //item.Director = data["results"][0]["Director"].Value<string>(); *not avilable in this webservice
                                item.Year = data["results"][0]["release_date"].Value<string>().Substring(0, 4);
                                var genreId = data["results"][0]["genre_ids"][0].Value<string>();
                                // Get the genre name
                                httpString = "https://api.themoviedb.org/3/genre/movie/list" +
                                             "?api_key=e62278365dd3d1d53e0415f424532c2a" +
                                             "&language=en-US";
                                json = client.DownloadString(httpString);
                                data = (JObject)JsonConvert.DeserializeObject(json);
                                var genres = data["genres"].ToList();
                                foreach (var genreType in genres)
                                    if (genreType["id"].ToString() == genreId)
                                    {
                                        item.Genre = genreType["name"].ToString();
                                        break;
                                    }
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }
                }
            }

            db.SaveChanges();
        }
    }
}