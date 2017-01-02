using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models
{
    public class Column
    {
        public String Heading { get; set; }
        public List<string> Data { get; set; }
        public List<string> LocationValues { get; set; }
        public DateTime[] DateValues { get; set; }
        public bool selected { get; set; }
        public String Context { get; set; }
        public bool IsContinous { get; set; }
        public int ColumnId { get; set; }
    }
}