using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MovieTheatre.Models
{
    public class Movie
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string Director { get; set; }
        public string Poster { get; set; }
        public string Trailer { get; set; }
    }
}
