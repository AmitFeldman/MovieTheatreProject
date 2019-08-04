using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MovieTheatre.Models
{
    public class Rating
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public string Review { get; set; }
        [Range(1, 10)]
        public int Stars { get; set; }
        public DateTime ReviewDate { get; set; }

        public virtual User User { get; set; }
        public virtual Movie Movie { get; set; }
    }
}