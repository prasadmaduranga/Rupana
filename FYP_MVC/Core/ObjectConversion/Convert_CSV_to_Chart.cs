using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models;
using FYP_MVC.Models.CoreObjects;
namespace FYP_MVC.Core.ObjectConversion
{
    public class Convert_CSV_to_Chart
    {
        int ColumnCount = 0,RowCount=0;

        public ChartComponent Convert(CSVFile csv, ChartComponent chart){
            ColumnCount = csv.Data.Length;
            RowCount = csv.Data[0].Data.Count;
            chart.columnList = new BaseColumn[ColumnCount];
            for (int i = 0; i < ColumnCount; i++)
            {
                processColumn(csv.Data[i], i, chart.columnList);
            }
            return chart;
        }


        public void processColumn(Column col,int i,BaseColumn[] ChartColumns) {
            ChartColumns[i] = new BaseColumn();
            switch (col.Context) {
                case "Nominal": { Column<string> column = new Column<string> { data = processNominalColumn(col), dataType = new Nominal { dataType = "Nominal", type = "string" }, columnHeader = col.Heading }; ChartColumns[i] = column; break; }
                case "Numeric": { Column<double> column = new Column<double> { data = processNumericColumn(col), dataType = new Numeric { dataType = "Numeric", type = "double" }, columnHeader = col.Heading }; ChartColumns[i] = column; break; }
                case "Percentage": { Column<double> column = new Column<double> { data = processNumericColumn(col), dataType = new Numeric { dataType = "Percentage", type = "double" }, columnHeader = col.Heading }; ChartColumns[i] = column; break; }
                case "Time series": { Column<string> column = new Column<string> { data = processDateColumn(col), dataType = new FYP_MVC.Models.CoreObjects.DateTime { dataType = "Nominal", type = "string" }, columnHeader = col.Heading }; ChartColumns[i] = column; break; }
                case "Location": { Column<string> column = new Column<string> { data = processLocationColumn(col), dataType = new FYP_MVC.Models.CoreObjects.Location { dataType = "Location", type = "string" ,region=col.Region,resolution=col.Resolution}, columnHeader = col.Heading }; ChartColumns[i] = column; break; }
            }
        }

        public Data<Double>[] processNumericColumn(Column col) {
            Data<Double>[] temp=new Data<double>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                temp[i] = new Data<double> { value = Double.Parse(col.Data[i]) };
            }
            return temp;
        }

        public Data<string>[] processNominalColumn(Column col)
        {
            Data<string>[] temp = new Data<string>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                temp[i] = new Data<string> { value = col.Data[i] };
            }
            return temp;
        }
        public Data<string>[] processLocationColumn(Column col)
        {
            Data<string>[] temp = new Data<string>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                temp[i] = new Data<string> { value = col.Data[i] };
            }
            return temp;
        }

        public Data<string>[] processDateColumn(Column col)
        {
            Data<string>[] temp = new Data<string>[RowCount];
            for (int i = 0; i < RowCount; i++)
            {
                temp[i] = new Data<string> { value = col.DateValues[i].ToString("yyyy-MM-dd")};
            }
            return temp;
        }

       
    }
}