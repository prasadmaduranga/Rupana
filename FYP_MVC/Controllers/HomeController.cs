using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Core.ContextRecognizer;

namespace FYP_MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           // ContextExtractor ce = new ContextExtractor();
            //ce.checkForDate();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}