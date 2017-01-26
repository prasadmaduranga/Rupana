using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;

namespace FYP_MVC.Core.ContextRecognizer
{
    public class ProcessConfirmedContext
    {
        public List<int> errorRows = new List<int>();
        public CSVFile processCSV(CSVFile csv)
        {
            csv.Data = csv.Data.Where(c => c.selected).ToArray();
            foreach (var item in csv.Data)
            {
                switch (item.Context)
                {
                   // case "Location": { //implementation of aba// break; }
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
                                newArray.Add(csv.Data[j].DateValues[i].ToString());
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

        public void processDateColumn(Column col)
        {
            for (int i = 0; i < col.Data.Count; i++)
            {
                if (col.DateValues[i]==DateTime.MinValue) { errorRows.Add(i); }
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