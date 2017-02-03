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
            ViewBag.mapping = TempData["mapping"];
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
            ViewBag.mapping = chviz.mappingList.ToList();
            return View(chviz.chrtCom);
        }

        public ActionResult GeoMarker()
        {
<<<<<<< HEAD
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];      
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
        
=======
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
           
            ViewBag.mapping = chviz.mappingList;

            return View(chviz.chrtCom);
>>>>>>> e9dc4b936ec3c8794abf7aca1fd9577cdf67e2c1
        }

        public ActionResult GeoRegion()
        {
            ChartVisualizationObject chviz = (ChartVisualizationObject)TempData["myObject"];
<<<<<<< HEAD
            ViewBag.mapping = chviz.mappingList;
            return View(chviz.chrtCom);
=======
            /*
            string fileLocation = Server.MapPath("~/Content/DataFiles/area.csv");


            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Bar Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader="Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[]{ 0, 1, 2, 3 };
            */
            ViewBag.mapping = chviz.mappingList;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(chviz.chrtCom);
            //return View();

>>>>>>> e9dc4b936ec3c8794abf7aca1fd9577cdf67e2c1
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
            ViewBag.mapping = TempData["mapping"]; 
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