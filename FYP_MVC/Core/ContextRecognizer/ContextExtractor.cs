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
        static int checkingRowMargin = 20;

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
                if (item.selected)
                {
                    processColumn(item);
                }
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
            if (rowCount > checkingRowMargin) { rowCount = checkingRowMargin; }
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
                bool isPercentage = false;
                col.Context = "Numeric";
                if (numericTotal > .9f && numericTotal < 1.1f) { isPercentage = true; }
                if (numericTotal > 90f && numericTotal < 110f) { isPercentage = true; }
                if (isPercentage) { col.Context = "Percentage"; }
                else if (LocationCount>.6*NumericCount) { col.Context = "Location"; }
                else if (DateCount>.6*NumericCount) { col.Context = "Time series"; }
            }
            numericTotal = 0f;

            //final processing
            if (col.Context.Equals("Location") || col.Context.Equals("Nominal")) { col.IsContinous = false; }
            else if (col.Context.Equals("Numeric") || col.Context.Equals("Percentage") || col.Context.Equals("Time series")) { col.IsContinous = true; }

            //counting discrete values
            col.NumDiscreteValues = col.Data.Distinct().Count();
            if (col.Context.Equals("Percentage") || col.Context.Equals("Numeric")) { col.NumDiscreteValues = 1000; }
        }

        public void checkForNumeric(Column col)
        {
            int rowCount = col.Data.Count;
            if (rowCount > checkingRowMargin) { rowCount = checkingRowMargin; }
            numericTotal = 0f;
            NumericCount = 0;
            for (int i = 0; i < rowCount; i++)
            {
                string item = col.Data[i];
                if (IsNumeric(item))
                {
                    NumericCount++; numericTotal += float.Parse(item);
                }
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
                int rowCount = col.Data.Count;
                if (rowCount > checkingRowMargin) { rowCount = checkingRowMargin; }
                switch (context)
                {
                    case "Percentage": { NumericCount += (float)(rowCount * .2); break; }
                    case "date": { DateCount += (float)(rowCount * .2); break; }
                    case "location": { LocationCount += (float)(rowCount * .2); break; }
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
            int numRows = col.Data.Count;
            if (numRows > checkingRowMargin) { numRows = checkingRowMargin; }
            int iterations = numRows/5;
            int remainder = numRows % 5;
            string query = "";
            for (int i = 0; i < iterations; i++)
            {
                query = generateQuery(arr, i * 5, (i + 1) * 5);
                callPython(pythonInfo,query,i*5,(i+1)*5,col);
            }
            if (remainder > 0)
            {
                query = generateQuery(arr, iterations * 5, col.Data.Count);
                callPython(pythonInfo, query, iterations * 5, col.Data.Count, col);
            }         
            return DateCount;
        }

        public string generateQuery(string[] arr, int start, int end)
        {
            String query = arr[start];
            for (int i = start+1; i < end; i++)
            {
                query += "," + arr[i];
            }
            return query;
        }
        public void callPython(ProcessStartInfo pythonInfo,string query,int start,int finish, Column col)
        {

            // calling python script passing parameter "query"
            // string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/Python/date.py");
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Python/date.py");
            pythonInfo.Arguments = string.Format("{0} {1}", path, query);
            pythonInfo.CreateNoWindow = false;
            pythonInfo.UseShellExecute = false;
            pythonInfo.RedirectStandardOutput = true;
            string result = "";
            using (Process process = Process.Start(pythonInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    for (int i = start; i < finish; i++)
                    {
                        result = reader.ReadLine();
                        if (result != null)
                        {
                            if (!result.Equals("error"))
                            {
                                DateTime myDate = DateTime.Parse(result);
                                if (myDate.Year > 1900 && myDate.Year < 2100)
                                {
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
    }
}