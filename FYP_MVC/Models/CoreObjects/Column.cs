using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models.CoreObjects
{
    public class BaseColumn
    {
    }
    public class Column<T>: BaseColumn
    {
        public Data<T>[] data { get; set; }
        public DataType dataType { get; set; }
        public string columnHeader { get; set; }


    }
}