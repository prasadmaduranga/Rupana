using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models
{
    public class CSVFile
    {
        public int ID { get; set; }
        public HttpPostedFileBase csvFile { get; set; }
        public string filename { get; set; }
        public string GUID { get; set; }
        public Column[] Data { get; set; }
        public int rowCount { get; set; }
        public string Intension { get; set; }
        public bool hasHeader { get; set; }
    }
}