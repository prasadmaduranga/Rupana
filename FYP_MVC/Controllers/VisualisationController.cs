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
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult BarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult BubbleChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult CalenderChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult GeoMarker()
        {
<<<<<<< HEAD
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
=======

            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];      
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        

>>>>>>> 41aa695c0674823f5648263f8860b4a15cc5b3fd
        }

        public ActionResult GeoRegion()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
<<<<<<< HEAD
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);

=======

            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);

          
>>>>>>> 41aa695c0674823f5648263f8860b4a15cc5b3fd

        }

        public ActionResult Histogram()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult LineChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult NormalizedStackedAreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult NormalizedStackedBarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult PieChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult SankeyDiagram()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult ScatterPlot()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult StackedAreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult StackedBarChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult TimeLine()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult TreeMap()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult WordChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        public ActionResult Boxplot()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }

        
        public ActionResult BivariateAreaChart()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
        }
        
        public ActionResult ParallelCordinates()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
            ChartComponentViewModel modelObj = new ChartComponentViewModel();
            modelObj.chartComponent = chviz.chrtCom;
            modelObj.mapping = (int[])TempData["mapping"];
            return View(modelObj);
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