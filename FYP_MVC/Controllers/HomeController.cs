using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Core.ContextRecognizer;
using FYP_MVC.Models;
using System.Diagnostics;
using System.IO;
using FYP_MVC.Core.ContextRecognizer;
using FYP_MVC.Models.DAO;

namespace FYP_MVC.Controllers
{
    public class HomeController : Controller
    {
        private FYPEntities db = new FYPEntities();

        public ActionResult welcomePage()
        {
            return View();
        }
        public ActionResult Index()
        {
            /*
            ProcessStartInfo pythonInfo = new ProcessStartInfo();
            pythonInfo.FileName = @"C:\Python27\python.exe";
           
           
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Python/date.py");
            pythonInfo.Arguments = string.Format("{0} {1}", path, "2015");
            pythonInfo.CreateNoWindow = false;
            pythonInfo.UseShellExecute = false;
            pythonInfo.RedirectStandardOutput = true;
            string result = "";
            using (Process process = Process.Start(pythonInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        result = reader.ReadLine();
                        if (!result.Equals("error"))
                        {
                            DateTime myDate = DateTime.Parse(result);
                          
                        }
                    }
                }
            }
          
            CSVFile csv = new CSVFile();
            Column col = new Column();
            csv.Data = new Column[1];
            csv.Data[0] = col;
            col.Data = new List<string>();
            col.Data.Add("2015");
            col.Data.Add("2014");
            col.Data.Add("April");
            col.Data.Add("2011");
            col.Data.Add("March");
            ContextExtractor con = new ContextExtractor(csv);
            System.Diagnostics.Debug.WriteLine("sdfasdfasdf");
            System.Diagnostics.Debug.WriteLine(con.checkForDate(col));*/


            user tempUser = db.users.Where(c => c.firstName.Equals("Lahiru")).First();
            Session["user"] = tempUser;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}