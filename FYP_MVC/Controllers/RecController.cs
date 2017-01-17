using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models;
using FYP_MVC.Core.Recommendations;
using FYP_MVC.Models.DAO;

namespace FYP_MVC.Controllers
{
    public class RecController : Controller
    {
        // GET: Recommendation
 
        public ActionResult ShowRecommendation(int tableID)
        {
            List<Recommendations_Result> recommendations = Recommendation.getRecommendationsWithoutIntention(tableID);
            List<string> arr = new List<string>();
            
            foreach (var item in recommendations)
            {
                arr.Add(item.chartName);
            }
            ViewBag.charts = arr;
            return View(recommendations);
        }
    }
}