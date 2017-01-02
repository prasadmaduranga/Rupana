using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;
using System.Diagnostics;
using System.IO;

namespace FYP_MVC.Core.ContextRecognizer
{
    public class ContextExtractor
    {
        public CSVFile csv;
        public ContextExtractor(CSVFile csv)
        {
            this.csv = csv;
        }
        
        // Counting variables
        int NumericCount = 0;
        int PercentageCount = 0;
        int LocationCount = 0;
        int DateCount = 0;

        //tempory numeric list
        List<double> temp_num = new List<double>();

        public void processColumn(Column col)
        {
            int rowCount = col.Data.Count;
            checkForLocation(col);
            checkForNumeric(col);
            checkForDate(col);
            PercentageCount = 0;
            PercentageCount = temp_num.Where(c => c >= 0d && c <= 100d).Count();
        }

        public void checkForNumeric(Column col)
        {
            NumericCount = 0;
            foreach (var item in col.Data)
            {
                if (IsNumeric(item)) { NumericCount++;}
            }
        }

        // try convert to numeric
        public bool IsNumeric(String value)
        {
            double s = 0d;
            bool result = double.TryParse(value, out s);
            temp_num.Add(s);
            return result;
        }

        public void checkForLocation(Column col)
        {
            LocationCount = 0;
            //implementation of Aba
            //update location count variable at the end
        }
        public void checkForDate(Column col)
        {
            DateCount = 0;
            ProcessStartInfo pythonInfo = new ProcessStartInfo();
            pythonInfo.FileName = @"C:\Python27\python.exe";
            int temp = col.Data.Count;
            col.DateValues = new DateTime[temp];
            String[] arr = col.Data.ToArray();
            String query = "";
            query = arr[0];
            // creating query string
            if (temp > 1)
            {
                for (int i = 1; i < temp; i++)
                {
                    query += "," + arr[i];
                }
            }
            // calling python script passing parameter "query"
            String path = HttpContext.Current.Server.MapPath("~/Content/Python/date.py");
            pythonInfo.Arguments = string.Format("{0} {1}",path,query);
            pythonInfo.CreateNoWindow = true;
            pythonInfo.UseShellExecute = false;
            pythonInfo.RedirectStandardOutput = true;
            string result = "";
            using (Process process = Process.Start(pythonInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    for (int i = 0; i < temp; i++)
                    {
                        result = reader.ReadLine();
                        if (!result.Equals("error"))
                        {
                            DateTime myDate = DateTime.Parse(result);
                            col.DateValues[i] = new DateTime();
                            col.DateValues[i] = myDate;
                            DateCount++;
                        }
                    }
                }
            }
        }
    }
}