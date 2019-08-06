using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTheatre.Controllers
{
    public class ErrorController : Controller
    {
        private const string DefaultErrorMessage = "You broke the website!";

        // GET: Error
        public ActionResult Index(string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.ErrorMessage = message;
            }
            else
            {
                ViewBag.ErrorMessage = DefaultErrorMessage;
            }
            return View();
        }
    }
}