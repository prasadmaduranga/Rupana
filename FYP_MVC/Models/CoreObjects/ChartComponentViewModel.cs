using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_MVC.Models.CoreObjects;

namespace FYP_MVC.Models.CoreObjects
{
    public class ChartComponentViewModel
    {
        public ChartComponent chartComponent { get; set; }
        public int[] mapping { get; set; }
    }
}