using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models.CoreObjects;
using System.Text;
using System.IO;

namespace FYP_MVC.Controllers
{
    public class VisualisationController : Controller
    {
        // GET: Visualisation
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult AreaChart(ChartComponent chart,int[] mapping)
        
        public ActionResult AreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult BarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult BubbleChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult CalenderChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult GeoMarker()
        {

            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];      
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        

        }

        public ActionResult GeoRegion()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];

            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);

          

        }

        public ActionResult Histogram()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult LineChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult NormalizedStackedAreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult NormalizedStackedBarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult PieChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult SankeyDiagram()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult ScatterPlot()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult StackedAreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult StackedBarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult TimeLine()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult TreeMap()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        public ActionResult WordChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        }

        private static string readCSVFileString(string fileLocation)
        {
           StringBuilder sb;
            using (StreamReader reader = new StreamReader(fileLocation))
            {
                string line;
                sb = new StringBuilder();
                int count = 1;
                while ((line = reader.ReadLine()) != null)
                {
                    if (count == 1)
                    {
                        sb.Append(line);

                    }
                    else
                    {
                        sb.Append(",");
                        sb.Append(line);

                    }

                    count++;

                }


            }
            return sb.ToString();

        }

    }
}