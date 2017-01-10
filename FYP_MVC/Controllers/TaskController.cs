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

namespace FYP_MVC.Controllers
{
    public class TaskController : Controller
    {
        private FYPEntities db = new FYPEntities();
        // GET: Task
        [HttpGet]
        public ActionResult UploadCSV()
        {
            return View();
        }

        public ActionResult Home()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UploadCSV(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    CSVFile csv = new CSVFile();
                    //Saving File in server
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

                        //creating new columns 
                        for (int i = 0; i < columnCount; i++)
                        {
                            Column col = new Column();
                            col.Data = new List<string>();
                            col.selected = true;
                            col.Heading = columns[i];
                            csv.Data[i] = col;
                        }

                        //now read rest of the file 
                        int rows = 0;
                        while ((a = sr.ReadLine()) != null)
                        {
                            string[] cols = a.Split(',');
                            for (int i = 0; i < columnCount; i++)
                            {
                                csv.Data[i].Data.Add(cols[i]);
                            }
                            rows++;
                        }
                        csv.rowCount = rows;
                        // numRows - used as loop variable in creating table
                        if (rows > 10) { ViewBag.numRows = 10; }
                        else { ViewBag.numRows = rows; }
                    }

                    
                    // saving to database : OriginalDataFile
                    originalDataFile ori_data = new originalDataFile();
                    ori_data.date = DateTime.Now;
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

        [HttpPost]
        public ActionResult showContextInfo(CSVFile csv)
        {
            CSVFile csv2 = CSVInjector.csv;
            for (int i = 0; i < csv.Data.Length; i++)
            {
                csv2.Data.ToList()[i].selected = csv.Data.ToList()[i].selected;
            }
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
            writeFinalCSV(csv2);
           

            return View(csv2);
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
                    else { text += selected[j].Data[i];}
                }
                csv.AppendLine(text);
            }

            String GUID = Guid.NewGuid().ToString();
            var myUniqueFileName = string.Format(@"{0}.csv", GUID);
            string path = Path.Combine(Server.MapPath("~/CSVFinalized"), Path.GetFileName(myUniqueFileName));
            System.IO.File.AppendAllText(path, csv.ToString());

            //Writing database values
            visualizedDataFile vizFile = new visualizedDataFile();
            vizFile.date = DateTime.Now;
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
                tbldim.dimensionIndex = i+1;
                tbldim.context = selected[i].Context;
                tbldim.tableID = tblId;
                tbldim.cardinality = selected[i].NumDiscreteValues;
                tbldim.isContinuous = selected[i].IsContinous ? 1 : 0;
                db.tableDimensions.Add(tbldim);
                db.SaveChanges();
            }

        }

        [HttpGet]
        public ActionResult showCSV()
        {
            CSVFile csv = (CSVFile)TempData["csv"];
            CSVInjector.csv = csv;
            bool[] selections = new bool[csv.Data.Length];
            for (int i = 0; i < selections.Length; i++)
            {
                selections[i] = new bool();
                selections[i] = true;
            }
            ViewBag.selections = selections;
            return View(csv);
        }

    }
}