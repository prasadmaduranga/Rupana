using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using FYP_MVC.Models.DAO;
using System.Data.Entity.Core.Objects;

namespace FYP_MVC.Core.ContextRecognizer
{
    public class ProcessConfirmedContext
    {
        public List<int> errorRows = new List<int>();
        FYPEntities db = new FYPEntities();
        public CSVFile processCSV(CSVFile csv)
        {
            csv.Data = csv.Data.Where(c => c.selected).ToArray();
            foreach (var item in csv.Data)
            {
                switch (item.Context)
                {
                    case "Location": { processLocationColumn(item); break; }
                    case "Numeric": { processNumericColumn(item); break; }
                    case "Percentage": { processNumericColumn(item); break; }
                    case "Time series": { processDateColumn(item);break; }
                }
            }
            removeErrors(csv);
            return csv;
        }

        public void processNominalColumn(Column col)
        {
            for (int i = 0; i < col.Data.Count; i++)
            {
                if (col.Data[i].Equals(string.Empty)) { errorRows.Add(i);}
            }
        }
        public void removeErrors(CSVFile csv)
        {
            int len = csv.rowCount;

            int cols = csv.Data.Count();
            if (errorRows.Count > 0)
            {
                for (int j = 0; j < cols; j++)
                {
                    string[] oldData = csv.Data[j].Data.ToArray();
                    List<string> newArray = new List<string>();
                    for (int i = 0; i < len; i++)
                    {
                        if (!errorRows.Contains(i))
                        {
                            if (csv.Data[j].Context.Equals("Time series"))
                            {
                                newArray.Add(csv.Data[j].Data[i].ToString());
                            }
                            else newArray.Add(oldData[i]);
                        }
                    }
                    csv.Data[j].Data = newArray;
                }
            }
        }
        
        public void processNumericColumn(Column col)
        {
            for (int i = 0; i < col.Data.Count; i++)
            {
                if (!IsNumeric(col.Data[i])) { errorRows.Add(i);}
            }
        }
        public void processLocationColumn(Column col)
        {
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
                
                dynamic jsonResult = JsonConvert.DeserializeObject(val);

                string resultStatus = jsonResult.status;

                // check if identified as a location
                if (resultStatus == "OK")
                {


                    AddressComponents[] address = jsonResult.results[0].address_components.ToObject<AddressComponents[]>();
                    countryList += address[address.Length - 1].long_name;
                    countryList += ",";
                    if (address[0].long_name.ToLower() != col.Data[arrayEntryCount].ToLower())
                    {
                        errorRows.Add(arrayEntryCount);           // if it not a location then add to error
                    }

                }
                else
                {
                    errorRows.Add(arrayEntryCount);           // if it not a location then add to error
                }

                arrayEntryCount++;
            }

            var regionParameter = new ObjectParameter("region", typeof(string));
            var resolutionParameter = new ObjectParameter("resolution", typeof(string));
            
            // get 
            db.getRegionCodeAndResolution(countryList, regionParameter, resolutionParameter);
            col.Region = regionParameter.Value.ToString();
            col.Resolution = resolutionParameter.Value.ToString();
        }
        public void processDateColumn(Column col)
        {
            for (int i = 0; i < col.Data.Count; i++)
            {
                //if (col.DateValues[i]==DateTime.MinValue) { errorRows.Add(i); }
            }
        }
        public bool IsNumeric(String value)
        {
            double s = 0d;
            bool result = double.TryParse(value, out s);
            return result;
        }
    }
}