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
        [HttpGet]
        [AllowAnonymous]
        public ActionResult AreaChart(ChartVisualizationObject chviz)
        {

            /*
            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Area Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2, 3 };
            */
            ViewBag.mapping = chviz.mappingList;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(chviz.chrtCom);
        }

        public ActionResult BarChart(ChartComponent chart)
        {

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

            ViewBag.mapping=mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult BubbleChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/bubbleChart.csv");


            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult CalenderChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/calenderChart.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Calender Chart";


            Data<String>[] Date = { new Data<string> { value = "2012-3-13" }, new Data<string> { value = "2012-3-14" }, new Data<string> { value = "2012-3-15" }, new Data<string> { value = "2012-3-16" }, new Data<string> { value = "2012-3-17" }, new Data<string> { value = "2012-9-4" } };
            Data<double>[] Win_Lose = { new Data<double> { value = 37032 }, new Data<double> { value = 38024 }, new Data<double> { value = 38024 }, new Data<double> { value = 38108 }, new Data<double> { value = 38229 }, new Data<double> { value = 38177 } };

            Column<String> column1 = new Column<string> { data = Date, dataType = new FYP_MVC.Models.CoreObjects.DateTime { dataType = "Nominal", type = "string" }, columnHeader = "Date" };
            Column<double> column2 = new Column<double> { data = Win_Lose, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Win_Lose" };
           
            BaseColumn[] colList = new BaseColumn[2];
            colList[0] = column1;
            colList[1] = column2;
            

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1};

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult GeoMarker()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/geoMarker.csv");


            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult GeoRegion()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/geoRegion.csv");


            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult Histogram()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/histogrm.csv");


            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult LineChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/lineChart.csv");


            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult NormalizedStackedAreaChart()
        {
            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Normalized Stacked Area Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2, 3 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult NormalizedStackedBarChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/area.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Stacked Bar Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2, 3 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult PieChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/pie.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "PieChart";


            Data<String>[] Task = { new Data<string> { value = "Work" }, new Data<string> { value = "Eat" }, new Data<string> { value = "Commute" }, new Data<string> { value = "Watch" }, new Data<string> { value = "Sleep" } };
            Data<double>[] HoursPerDay = { new Data<double> { value = 11 }, new Data<double> { value = 2 }, new Data<double> { value = 3 }, new Data<double> { value = 5 }, new Data<double> { value = 3 } };

            Column<String> column1 = new Column<string> { data = Task, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Task" };
            Column<double> column2 = new Column<double> { data = HoursPerDay, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "HoursPerDay" };

            BaseColumn[] colList = new BaseColumn[2];
            colList[0] = column1;
            colList[1] = column2;
            

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1};

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult SankeyDiagram()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/sankey.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "SankeyDiagram";


            Data<String>[] From = { new Data<string> { value = "A" }, new Data<string> { value = "A" }, new Data<string> { value = "A" }, new Data<string> { value = "B" }, new Data<string> { value = "B" }, new Data<string> { value = "B" } };
            Data<String>[] To = { new Data<string> { value = "X" }, new Data<string> { value = "Y" }, new Data<string> { value = "Z" }, new Data<string> { value = "X" }, new Data<string> { value = "Y" }, new Data<string> { value = "Y" } };
            Data<double>[] Weight = { new Data<double> { value = 5 }, new Data<double> { value = 7 }, new Data<double> { value = 6 }, new Data<double> { value = 2 }, new Data<double> { value = 9 }, new Data<double> { value = 4 } };

            Column<String> column1 = new Column<string> { data = From, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "From" };
            Column<String> column2 = new Column<string> { data = To, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "To" };
            Column<double> column3 = new Column<double> { data = Weight, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Weight" };

            BaseColumn[] colList = new BaseColumn[3];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;


            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2};

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult ScatterPlot()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/sankey.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "ScatterPlot";


            Data<double>[] Age = { new Data<double> { value = 8 }, new Data<double> { value = 4 }, new Data<double> { value = 11 }, new Data<double> { value = 4 }, new Data<double> { value = 3 }, new Data<double> { value = 6 } };
            Data<double>[] Weight = { new Data<double> { value = 12 }, new Data<double> { value = 5 }, new Data<double> { value = 14 }, new Data<double> { value = 5 }, new Data<double> { value = 3 }, new Data<double> { value = 5 } };
            Data<double>[] Height = { new Data<double> { value = 1.85 }, new Data<double> { value = 1.55 }, new Data<double> { value = 1.78 }, new Data<double> { value = 2.00 }, new Data<double> { value = 1.2 }, new Data<double> { value = 1.5 } };

            Column<double> column1 = new Column<double> { data = Age, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Age" };
            Column<double> column2 = new Column<double> { data = Weight, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Weight" };
            Column<double> column3 = new Column<double> { data = Height, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Height" };

            BaseColumn[] colList = new BaseColumn[3];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;


            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult StackedAreaChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/area.csv");


            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Stacked Area Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2, 3 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult StackedBarChart()
        {


            string fileLocation = Server.MapPath("~/Content/DataFiles/area.csv");


            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "Stacked Bar Chart";


            Data<String>[] Name = { new Data<string> { value = "Prasad" }, new Data<string> { value = "Maduranga" }, new Data<string> { value = "Dilshan" } };
            Data<double>[] Sinhala = { new Data<double> { value = 85 }, new Data<double> { value = 95 }, new Data<double> { value = 34 } };
            Data<double>[] Maths = { new Data<double> { value = 65 }, new Data<double> { value = 25 }, new Data<double> { value = 54 } };
            Data<double>[] Science = { new Data<double> { value = 55 }, new Data<double> { value = 26 }, new Data<double> { value = 14 } };

            Column<String> column1 = new Column<string> { data = Name, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Name" };
            Column<double> column2 = new Column<double> { data = Sinhala, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Sinhala" };
            Column<double> column3 = new Column<double> { data = Maths, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Maths" };
            Column<double> column4 = new Column<double> { data = Science, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "Science" };

            BaseColumn[] colList = new BaseColumn[4];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;
            colList[3] = column4;

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2, 3 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult TimeLine()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/area.csv");
            ChartComponent testChart = new ChartComponent();
            testChart.ID = 1;
            testChart.name = "areaChart";
            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(testChart);
        }

        public ActionResult TreeMap()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/sankey.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "TreeMap";


            Data<String>[] Location = { new Data<string> { value = "Global" }, new Data<string> { value = "America" }, new Data<string> { value = "Europe" }, new Data<string> { value = "Asia" }, new Data<string> { value = "Australia" }, new Data<string> { value = "Africa" }, new Data<string> { value = "Brazil" }, new Data<string> { value = "Usa" }, new Data<string> { value = "Mexica" }, new Data<string> { value = "Canada" } };
            Data<String>[] Parent = { new Data<string> { value = "null" }, new Data<string> { value = "Global" }, new Data<string> { value = "Global" }, new Data<string> { value = "Global" }, new Data<string> { value = "Global" }, new Data<string> { value = "Global" }, new Data<string> { value = "America" }, new Data<string> { value = "America" }, new Data<string> { value = "America" }, new Data<string> { value = "America" } };
            Data<double>[] GTV = { new Data<double> { value = 0 }, new Data<double> { value = 0 }, new Data<double> { value = 0 }, new Data<double> { value = 0 }, new Data<double> { value = 0 }, new Data<double> { value = 0 }, new Data<double> { value = 11 }, new Data<double> { value = 52 }, new Data<double> { value = 24 }, new Data<double> { value = 16 } };

            Column<String> column1 = new Column<string> { data = Location, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Location" };
            Column<String> column2 = new Column<string> { data = Parent, dataType = new Nominal { dataType = "Nominal ", type = "string" }, columnHeader = "Parent" };
            Column<double> column3 = new Column<double> { data = GTV, dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = "GTV" };

            BaseColumn[] colList = new BaseColumn[3];
            colList[0] = column1;
            colList[1] = column2;
            colList[2] = column3;


            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0, 1, 2 };

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
        }

        public ActionResult WordChart()
        {
            string fileLocation = Server.MapPath("~/Content/DataFiles/pie.csv");

            ChartComponent sampleChart = new ChartComponent();
            sampleChart.ID = 1;
            sampleChart.name = "WordChart";


            Data<String>[] Phrases = { new Data<string> { value = "cats are better than dogs" }, new Data<string> { value = "cats eat kibble" }, new Data<string> { value = "cats are better than hamsters" }, new Data<string> { value = "cats are awesome" }, new Data<string> { value = "cats are people too" }, new Data<string> { value = "cats eat mice" }, new Data<string> { value = "cats meowing" }, new Data<string> { value = "cats in the cradle" }, new Data<string> { value = "cats eat mice" }, new Data<string> { value = "cats in the cradle lyrics" }, new Data<string> { value = "cats eat kibble" }, new Data<string> { value = "cats for adoption" }, new Data<string> { value = "cats are family" } };

            Column<String> column1 = new Column<string> { data = Phrases, dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = "Phrases" };

            BaseColumn[] colList = new BaseColumn[1];
            colList[0] = column1;
            

            sampleChart.columnList = colList;

            int[] mapping = new int[] { 0};

            ViewBag.mapping = mapping;


            //testChart.dataStr = VisualisationController.readCSVFileString(fileLocation);
            //testChart.numOfColumns = System.IO.File.ReadLines(fileLocation).Select(x => x.Split(',')).ToArray()[0].Length;
            return View(sampleChart);
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