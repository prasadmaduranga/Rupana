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
namespace FYP_MVC.Controllers
{
    public class TaskController : Controller
    {

        // GET: Task
        [HttpGet]
        public ActionResult UploadCSV()
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