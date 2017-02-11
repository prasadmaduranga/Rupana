using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;
using System.Diagnostics;
using System.IO;
using FYP_MVC.Models.DAO;
using Newtonsoft.Json;
using System.Net;

using FYP_MVC.Models;
using System.Data.Entity.Core.Objects;

namespace FYP_MVC.Core.ContextRecognizer
{
    public class ContextExtractor
    {
        public ContextExtractor() { }
        static int checkingRowMargin = 20;
        string region = "";
        string resolution = "";

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
        float FloatingPointCount = 0;

      
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
            checkForFloat(col);

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
                //special validation for date time with period ex :- "12.50"
                if (col.Context.Equals("Time series") && FloatingPointCount > .6* col.Data.Count) { col.Context = "Numeric"; }
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

        public void checkForFloat(Column col)
        {
            int rowCount = col.Data.Count;
            if (rowCount > checkingRowMargin) { rowCount = checkingRowMargin; }
            FloatingPointCount = 0;
            for (int i = 0; i < rowCount; i++)
            {
                string item = col.Data[i];
                if (item.Contains(".") && IsNumeric(item))
                {
                        FloatingPointCount++;
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
            int rowCount = col.Data.Count;
            string[] jsonResponse = new string[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                string address = col.Data[i];
                string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyBe7bmv5rusSTJ__tPpPoNkCUt0rxjR7jo";
                var request = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                jsonResponse[i] = responseString;
            }

            int arrayEntryCount = 0;
            var locationHeirarchy = new Dictionary<int, string>();
            string countryList = "";

            foreach (var val in jsonResponse)
            {
                arrayEntryCount++;
                dynamic jsonResult = JsonConvert.DeserializeObject(val);

                string resultStatus = jsonResult.status;

                // check if identified as a location
                if (resultStatus == "OK")
                {


                    AddressComponents[] address = jsonResult.results[0].address_components.ToObject<AddressComponents[]>();
                    countryList += address[address.Length - 1].long_name;
                    countryList += ",";
                    if (address[0].long_name.ToLower() == col.Data[arrayEntryCount - 1].ToLower())
                    {
                        LocationCount++;            // if it is location increase location count
                    }

                }
            }

            var regionParameter = new ObjectParameter("region", typeof(string));
            var resolutionParameter = new ObjectParameter("resolution", typeof(string));
            // get 
            db.getRegionCodeAndResolution(countryList, regionParameter, resolutionParameter);
            region = regionParameter.Value.ToString();
            resolution = resolutionParameter.Value.ToString();

            //implementation of Aba
            //update location count variable at the end
        
    }
   
        public float checkForDate(Column col)
        {
            DateCount = 0;
            ProcessStartInfo pythonInfo = new ProcessStartInfo();
            //pythonInfo.FileName = @"C:\Python27\python.exe";

            //pythonInfo.FileName = @"C:\Users\User\AppData\Local\Programs\Python\Python35-32\python.exe";
            pythonInfo.FileName = @"/usr/bin/python";
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