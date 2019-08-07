using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MovieTheatre.Models
{
    public class LocationPoint
    {
        public int ID { get; set; }
        [Required]
        public double lat { get; set; }
        public double lng { get; set; }
    }
}