using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieTheatre.Models
{
    public class User
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Please Provide name", AllowEmptyStrings = false)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Provide email", AllowEmptyStrings = false)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please provide password", AllowEmptyStrings = false)]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
    }
}