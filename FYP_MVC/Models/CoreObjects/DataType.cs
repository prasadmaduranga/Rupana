using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models.CoreObjects
{
    public class DataType
    {
        public string type { get; set; }  
        public string dataType { get; set; }
    }

    public class Nominal:DataType
    {
    }

    public class Numeric : DataType
    {
    }
    public class Location : DataType
    {
        public string resolution { get; set; } 
        public string region { get; set; }
    }

    public class DateTime : DataType
    {

    }


    public class Percentage : DataType
    {
    }
}