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

            return csv;
        }

        public Column processColumn(Column col)
        {
            return col;
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
                if (col.DateValues[i]==null) { errorRows.Add(i); }
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