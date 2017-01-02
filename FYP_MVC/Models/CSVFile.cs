using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models
{
    public class CSVFile
    {
        public HttpPostedFileBase csvFile { get; set; }
        public String filename { get; set; }
        public String GUID { get; set; }
        public Column[] Data { get; set; }
        public int rowCount { get; set; }
    }
}