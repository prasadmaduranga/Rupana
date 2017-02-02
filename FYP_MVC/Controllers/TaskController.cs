using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models;
using System.IO;
using System.Data;
using FYP_MVC.Core.ContextRecognizer;
using FYP_MVC.Core.Injector;
using FYP_MVC.Models.DAO;
using FYP_MVC.Models.CoreObjects;
using FYP_MVC.Core.ObjectConversion;

namespace FYP_MVC.Controllers
{
    public class TaskController : Controller
    {
        private FYPEntities db = new FYPEntities();
        // GET: Task
        [HttpGet]
        public ActionResult UploadCSV()
        {
            CSVFile csv = new CSVFile();
            csv.hasHeader = true;
            return View(csv);
        }

        public ActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadCSV(bool hasHeader, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    CSVFile csv = new CSVFile();
                    //Saving File in server
                    csv.hasHeader = hasHeader;
                    String GUID = Guid.NewGuid().ToString();
                    var myUniqueFileName = string.Format(@"{0}.csv", GUID);
                    string path = Path.Combine(Server.MapPath("~/CSV"), Path.GetFileName(myUniqueFileName));
                    file.SaveAs(path);

                    //Initializing CSV 
                    csv.csvFile = file;
                    csv.filename = file.FileName;
                    csv.GUID = GUID;

                    int columnCount = 0;
                    using (Stream fileStream = file.InputStream)
                    using (StreamReader sr = new StreamReader(fileStream))
                    {
                        string a = null;
                        // reading csv header
                        a = sr.ReadLine();
                        string[] columns = a.Split(',');
                        columnCount = columns.Length;
                        csv.Data = new Column[columnCount];
                        for (int i = 0; i < columnCount; i++)
                        {
                            Column col = new Column();
                            col.Data = new List<string>();
                            col.selected = true;
                            csv.Data[i] = col;
                        }
                        int rows = 0;
                        if (hasHeader)
                        {
                            //creating new columns 
                            for (int i = 0; i < columnCount; i++)
                            {
                                csv.Data[i].Heading = columns[i];
                            }
                        }
                        else
                        {
                            rows = 1;
                            if (!columns.Where(c => c.Equals(String.Empty)).Any())
                            {

                                for (int i = 0; i < columnCount; i++)
                                {
                                    csv.Data[i].Data.Add(columns[i]);
                                    csv.Data[i].Heading = "Column " + (i + 1);
                                }
                            }
                            else
                            { //fatal error
                            }
                        }
                        //now read rest of the file 

                        while ((a = sr.ReadLine()) != null)
                        {
                            string[] cols = a.Split(',');
                            if (!cols.Where(c => c.Equals(String.Empty)).Any())
                            {

                                for (int i = 0; i < columnCount; i++)
                                {
                                    csv.Data[i].Data.Add(cols[i]);
                                }
                                rows++;
                            }
                            else { }
                        }
                        csv.rowCount = rows;
                        // numRows - used as loop variable in creating table
                        if (rows > 10) { ViewBag.numRows = 10; }
                        else { ViewBag.numRows = rows; }
                    }


                    // saving to database : OriginalDataFile
                    originalDataFile ori_data = new originalDataFile();
                    ori_data.date = System.DateTime.Now;
                    ori_data.fileLocation = csv.GUID;
                    user temp = (user)Session["user"];
                    ori_data.userID = temp.ID;
                    db.originalDataFiles.Add(ori_data);
                    db.SaveChanges();

                    csv.ID = ori_data.ID;
                    TempData["csv"] = csv;
                    return RedirectToAction("showCSV", "Task");

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult showCSV()
        {
            ViewBag.errorMessage = TempData["errorMessage"];
            CSVFile csv = (CSVFile)TempData["csv"];
            CSVInjector.csv = csv;
            if (csv.rowCount < 15)
            {
                ViewBag.rowCount = csv.rowCount;
            }
            else ViewBag.rowCount = 15;
            return View(csv);
        }
        [HttpPost]
        public ActionResult showContextInfo(CSVFile csv)
        {
            int selectedcout = csv.Data.Where(c => c.selected).Count();
            if (selectedcout < 2 || selectedcout > 7)
            {
                TempData["csv"] = CSVInjector.csv;
                TempData["errorMessage"] = "Please select number of columns in range 2-7";
                return RedirectToAction("showCSV", "Task");
            }
            CSVFile csv2 = CSVInjector.csv;
            for (int i = 0; i < csv.Data.Length; i++)
            {
                csv2.Data.ToList()[i].selected = csv.Data.ToList()[i].selected;
            }
            csv2.Intension = csv.Intension;
            ContextExtractor con = new ContextExtractor(csv2);
            CSVFile csvs = con.processCSV();
            CSVInjector.csv = csvs;

            return View(csvs);
        }

        [HttpPost]
        public ActionResult Recommendations(CSVFile csv)
        {
            CSVFile csv2 = CSVInjector.csv;
            for (int i = 0; i < csv.Data.Length; i++)
            {
                csv2.Data.ToList()[i].Context = csv.Data.ToList()[i].Context;
            }
            ProcessConfirmedContext PCC = new ProcessConfirmedContext();
            PCC.processCSV(csv2);
            writeFinalCSV(csv2);
            int tblId = (int)Session["CurrentTableId"];
            return RedirectToAction("ShowRecommendation", "Rec", new { tableID = tblId, csv = csv2 });

        }
        //Write CSV to file system with only selected columns
        public void writeFinalCSV(CSVFile csvfile)
        {
            Column[] selected = (from t in csvfile.Data where t.selected == true select t).ToArray();
            var csv = new System.Text.StringBuilder();
            int numCols = selected.Length;
            int numRows = selected[0].Data.Count;
            string text = "";

            for (int i = 0; i < numCols; i++)
            {
                if (i != 0)
                {
                    text += ("," + selected[i].Heading);
                }
                else { text += selected[i].Heading; }
            }
            csv.AppendLine(text);

            for (int i = 0; i < numRows; i++)
            {
                text = "";
                for (int j = 0; j < numCols; j++)
                {
                    if (j != 0)
                    {
                        text += ("," + selected[j].Data[i]);
                    }
                    else { text += selected[j].Data[i]; }
                }
                csv.AppendLine(text);
            }

            String GUID = Guid.NewGuid().ToString();
            var myUniqueFileName = string.Format(@"{0}.csv", GUID);
            string path = Path.Combine(Server.MapPath("~/CSVFinalized"), Path.GetFileName(myUniqueFileName));
            System.IO.File.AppendAllText(path, csv.ToString());

            //Writing database values
            visualizedDataFile vizFile = new visualizedDataFile();
            vizFile.date = System.DateTime.Now;
            vizFile.fileLocation = myUniqueFileName;
            vizFile.parentFileID = csvfile.ID;
            db.visualizedDataFiles.Add(vizFile);
            db.SaveChanges();
            int ID = vizFile.ID;
            table tbl = new table();
            tbl.fileID = ID;
            tbl.name = csvfile.filename;
            tbl.numOfRows = numRows;
            db.tables.Add(tbl);
            db.SaveChanges();
            int tblId = tbl.ID;
            for (int i = 0; i < numCols; i++)
            {
                tableDimension tbldim = new tableDimension();
                tbldim.dimensionIndex = i + 1;
                tbldim.context = selected[i].Context;
                tbldim.tableID = tblId;
                tbldim.cardinality = selected[i].NumDiscreteValues;
                tbldim.isContinuous = selected[i].IsContinous ? 1 : 0;
                db.tableDimensions.Add(tbldim);
                db.SaveChanges();
            }
            Session["CurrentTableId"] = tbl.ID;
        }



        public ActionResult logout()
        {
            Session["user"] = null;

            return RedirectToAction("Login", "Authentication");

        }

        public ActionResult visualizeDataFile(int num)
        {
            ChartVisualizationObject chViz = ChartVisualizationObjectInjector.CVObject;
            CSVFile csv = CSVInjector.csv;
            Convert_CSV_to_Chart converter = new Convert_CSV_to_Chart();
            chViz.chrtCom = new ChartComponent();
            chViz.chrtCom.name = chViz.chartTypes[num];
            chViz.chrtCom = converter.Convert(csv, chViz.chrtCom);

            switch (chViz.chrtCom.name)
            {
                case ("Area chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("AreaChart", "Visualisation", "prasad");
                    }
                case ("Stacked bar chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BarChart", "Visualisation");
                    
                    }

                case ("Pin point location map"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("GeoMarker", "Visualisation");

                    }

                case ("Bar chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BarChart", "Visualisation");

                    }

                case ("Pie chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("PieChart", "Visualisation");

                    }
                case ("Tree Map"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("TreeMap", "Visualisation");

                    }
                case ("Stacked area chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("StackedAreaChart", "Visualisation");

                    }
                case ("Nomalized stacked bar chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("NormalizedStackedBarChart", "Visualisation");

                    }
                case ("Scatterplot"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("ScatterPlot", "Visualisation");

                    }
                case ("Mark Area  map"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("GeoRegion", "Visualisation");

                    }
                case ("Bubble chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BubbleChart", "Visualisation");

                    }
                case ("Calender view chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("CalenderChart", "Visualisation");

                    }
                case ("Clusture dendogram"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("WordChart", "Visualisation");

                    }
                case ("Parallel cordinates"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("ParallelCordinates", "Visualisation");

                    }
                case ("Sankey diagram"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("SankeyDiagram", "Visualisation");

                    }
                case ("Boxplot"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BoxPlot", "Visualisation");

                    }
                case ("Line chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("LineChart", "Visualisation");

                    }
                case ("Normalized stacked area chart"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("NormalizedStackedAreaChart", "Visualisation");

                    }
                case ("Histrogram"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("Histogram", "Visualisation");

                    }
                case ("Time Line"):
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("TimeLine", "Visualisation");

                    }

                default:
                    {
                        TempData["myObject"] = chViz;
                        return RedirectToAction("GeoMarker", "Visualisation");
                    }

            }


        }
    }
}