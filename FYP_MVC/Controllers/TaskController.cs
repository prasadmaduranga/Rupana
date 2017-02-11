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
            if (selectedcout < 1 || selectedcout > 7)
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
            csv.Data = csv.Data.Where(c => c.Context != null).ToArray();
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

        public ActionResult saveFeedback(int num)
        {
            CSVFile csv = CSVInjector.csv;
            ChartVisualizationObject chViz = ChartVisualizationObjectInjector.CVObject;
            feedBack fb = new feedBack();
            int recCount = chViz.chartTypes.Count();
            if (recCount > num) { fb.recommendation = chViz.chartTypes[num]; }
            else { fb.recommendation = chViz.more_chartTypes[num-recCount]; }
            fb.tableID = (int)Session["CurrentTableId"];
            int colCount = csv.Data.Count();
            fb.numOfDim = colCount;
            fb.intention = csv.Intension;
            fb.dim1_IsContinuous = csv.Data[0].IsContinous ? 1 : 0;
            fb.dim1_context = csv.Data[0].Context;
            fb.dim1_Cardinality = csv.Data[0].NumDiscreteValues;
            if (colCount == 7)
            {
                fb.dim7_IsContinuous = csv.Data[6].IsContinous ? 1 : 0;
                fb.dim7_context = csv.Data[6].Context;
                fb.dim7_Cardinality = csv.Data[6].NumDiscreteValues;
            }
            if (colCount > 5)
            {
                fb.dim6_IsContinuous = csv.Data[5].IsContinous ? 1 : 0;
                fb.dim6_context = csv.Data[5].Context;
                fb.dim6_Cardinality = csv.Data[5].NumDiscreteValues;
            }
            if (colCount > 4)
            {
                fb.dim5_IsContinuous = csv.Data[4].IsContinous ? 1 : 0;
                fb.dim5_context = csv.Data[4].Context;
                fb.dim5_Cardinality = csv.Data[4].NumDiscreteValues;
            }
            if (colCount > 3)
            {
                fb.dim4_IsContinuous = csv.Data[3].IsContinous ? 1 : 0;
                fb.dim4_context = csv.Data[3].Context;
                fb.dim4_Cardinality = csv.Data[3].NumDiscreteValues;
            }
            if (colCount > 2)
            {
                fb.dim3_IsContinuous = csv.Data[2].IsContinous ? 1 : 0;
                fb.dim3_context = csv.Data[2].Context;
                fb.dim3_Cardinality = csv.Data[2].NumDiscreteValues;
            }
            if (colCount > 1)
            {
                fb.dim2_IsContinuous = csv.Data[1].IsContinous ? 1 : 0;
                fb.dim2_context = csv.Data[1].Context;
                fb.dim2_Cardinality = csv.Data[1].NumDiscreteValues;
            }
            db.feedBacks.Add(fb);
            db.SaveChanges();
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult visualizeDataFile(int num)
        {
            ChartVisualizationObject chViz = ChartVisualizationObjectInjector.CVObject;
            CSVFile csv = CSVInjector.csv;
            Convert_CSV_to_Chart converter = new Convert_CSV_to_Chart();
            chViz.chrtCom = new ChartComponent();
            chViz.chrtCom.name = chViz.chartTypes[num];
            chViz.chrtCom = converter.Convert(csv, chViz.chrtCom);
            string chart = "";
            int recCount = chViz.chartTypes.Count();
            if (recCount > num) { chart = chViz.chartTypes[num]; }
            else { chart = chViz.more_chartTypes[num - recCount]; }

            switch (chart)
            {
                case ("Area chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;                    
                        return RedirectToAction("AreaChart", "Visualisation");
                    }
                case ("Stacked bar chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("StackedBarChart", "Visualisation");

                    }

                case ("Pin point location  map"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("GeoMarker", "Visualisation");

                    }

                case ("Bar chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BarChart", "Visualisation");

                    }

                case ("Pie chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("PieChart", "Visualisation");

                    }
                case ("Tree Map"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("TreeMap", "Visualisation");

                    }
                case ("Stacked area chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("StackedAreaChart", "Visualisation");
                    }
                case ("Nomalized stacked bar chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("NormalizedStackedBarChart", "Visualisation");
                    }
                case ("Scatterplot"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("Scatterplot", "Visualisation");

                    }
                case ("Mark Area  map"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("GeoRegion", "Visualisation");

                    }
                case ("Bubble chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BubbleChart", "Visualisation");
                    }
                case ("Calender view chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("CalenderChart", "Visualisation");

                    }
                case ("Clusture dendogram"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("WordChart", "Visualisation");

                    }
                case ("Parallel cordinates"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("ParallelCordinates", "Visualisation");
                    }
                case ("Sankey diagram"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("SankeyDiagram", "Visualisation");
                    }
                case ("Boxplot"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("Boxplot", "Visualisation");
                    }
                case ("Line chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("LineChart", "Visualisation");
                    }
                case ("Normalized stacked area chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("NormalizedStackedAreaChart", "Visualisation");

                    }
                case ("Histrogram"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("Histogram", "Visualisation");
                    }
                case ("Time Line"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("TimeLine", "Visualisation");
                    }

                    
                case ("Grouped bar chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BarChart", "Visualisation");
                    }

                case ("Muiltiple Bar chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BarChart", "Visualisation");
                    }


                case ("Bivariate area chart"):
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        return RedirectToAction("BivariateAreaChart", "Visualisation");
                    }


                default:
                    {
                        TempData["mapping"] = chViz.mappingList.ToList().ElementAt(num);
                        TempData["myObject"] = chViz;
                        //return RedirectToAction("AreaChart", "Visualisation", new { chviz = chViz });
                        return RedirectToAction("AreaChart", "Visualisation");
                    }

            }


        }
    }
}