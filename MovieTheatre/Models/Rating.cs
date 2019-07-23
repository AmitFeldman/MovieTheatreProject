using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTheatre.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int MovieID { get; set; }
        public string Review { get; set; }
        public double Stars { get; set; }
        public DateTime ReviewDate { get; set; }

    }
}