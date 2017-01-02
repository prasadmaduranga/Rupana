using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models;
using System.IO;
using System.Data;

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
                    //Saving File in server
                    String GUID = Guid.NewGuid().ToString();
                    var myUniqueFileName = string.Format(@"{0}.csv", GUID);
                    string path = Path.Combine(Server.MapPath("~/CSV"), Path.GetFileName(myUniqueFileName));
                    file.SaveAs(path);

                    //Initializing CSV 
                    CSVFile csv = new CSVFile();
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
                        if (rows > 10) { csv.rowCount = 10; }
                        else { csv.rowCount = rows; }
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

        [HttpGet]
        public ActionResult showCSV()
        {
            CSVFile csv = (CSVFile)TempData["csv"];
            return View(csv);
        }

        


    }
}