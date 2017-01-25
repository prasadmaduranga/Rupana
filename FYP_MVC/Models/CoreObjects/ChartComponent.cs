using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FYP_MVC.Models.CoreObjects;
namespace FYP_MVC.Models.CoreObjects
{
    public class ChartComponent
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public FYP_MVC.Models.CoreObjects.BaseColumn[] columnList { get; set; }    

    }
}