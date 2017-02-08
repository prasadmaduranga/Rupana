using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models.CoreObjects
{
    public class ChartVisualizationObject
    {
        public ChartComponent chrtCom;
        public List<int[]> mappingList;
        public string[] chartTypes;
        public List<int[]> more_mappingList;
        public string[] more_chartTypes;
    }
}