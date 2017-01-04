using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;
using System.Diagnostics;
using System.IO;
using FYP_MVC.Models.DAO;

namespace FYP_MVC.Core.ContextRecognizer
{
    public class ContextExtractor
    {
        public ContextExtractor() { }

        FYPEntities db = new FYPEntities();
        public CSVFile csv;
        public ContextExtractor(CSVFile csv)
        {
            this.csv = csv;
          
        }
        
        // Counting variables
        float NumericCount = 0;
        float LocationCount = 0;
        float DateCount = 0;

        //tempory numeric list
        float numericTotal = 0f;

        public CSVFile processCSV()
        {
            foreach (var item in csv.Data)
            {
                processColumn(item);
            }
            return csv;
        }
        public void processColumn(Column col)
        {
            int rowCount = col.Data.Count;
            checkForLocation(col);
            checkForNumeric(col);
            checkForDate(col);
            checkHeader(col);

            // Entering condition count > num_rows/2
            bool IsLocationEnters = (LocationCount > rowCount / 2) ? true:false;
            bool IsDateEnters = (DateCount > rowCount / 2) ? true : false;
            bool IsNumericEnters = (NumericCount > rowCount / 2) ? true : false;

            //Nothing enters context is Nominal
            if (!IsDateEnters && !IsLocationEnters && !IsNumericEnters)
            {
                col.Context = "Nominal";
            }
            else
            {
                float max = NumericCount;
                if (NumericCount < LocationCount) { max = LocationCount; }
                if (NumericCount < DateCount && LocationCount < DateCount) { max = DateCount; }

                if (max == NumericCount)
                {
                    bool isPersentage = false;
                    col.Context = "Numeric";
                    if (numericTotal > .9f && numericTotal < 1.1f) { isPersentage = true; }
                    if (numericTotal > 90f && numericTotal < 110f) { isPersentage = true; }
                    if (isPersentage) { col.Context = "Persentage"; }
                }
                else if (max == LocationCount) { col.Context = "Location"; }
                else if (max == DateCount) { col.Context = "DateTime"; }
            }
            numericTotal = 0f;
        }

        public void checkForNumeric(Column col)
        {
            numericTotal = 0f;
            NumericCount = 0;
            foreach (var item in col.Data)
            {
                if (IsNumeric(item)) { NumericCount++;numericTotal += float.Parse(item); }
            }
        }

        // try convert to numeric
        public bool IsNumeric(String value)
        {
            double s = 0d;
            bool result = double.TryParse(value, out s);
            return result;
        }
        public void checkHeader(Column col)
        {
            String heading = col.Heading.ToLower();
            String[] headingarr = heading.Split(new Char[] { ',', '_',' ','.'});
            foreach (var item in headingarr)
            {
                String context = db.headerContexts.Where(c => c.Word.Equals(item)).Select(d => d.ContextType).FirstOrDefault();
                if (context != null) { context = context.Replace("\r\n", string.Empty); }
                switch (context)
                {
                    case "percentage": { NumericCount += (float)(csv.rowCount * .2); break; }
                    case "date": { DateCount += (float)(csv.rowCount * .2); break; }
                    case "location": { LocationCount += (float)(csv.rowCount * .2); break; }
                    default: break;
                }
            }
            
        }
        public void checkForLocation(Column col)
        {
            LocationCount = 0;
            //implementation of Aba
            //update location count variable at the end
        }
        public float checkForDate(Column col)
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
           // string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/Python/date.py");
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Python/date.py");
            pythonInfo.Arguments = string.Format("{0} {1}",path,query);
            pythonInfo.CreateNoWindow = false;
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
            return DateCount;
        }
    }
}