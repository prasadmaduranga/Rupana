using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FYP_MVC.Models.DAO
{

    public class ChartTemplate
    {
        [Display(Name = "Chart ID")]
        public int id { get; set; }

        [Display(Name = "Chart Name")]
        public string name { get; set; }

        [DisplayName("Intention")]
        public List<intention> intentionList { get; set; }

        [DisplayName("Comparison")]
        public bool comparison { get; set; }

        [DisplayName("Composition")]
        public bool composition { get; set; }

        [DisplayName("Distribution")]
        public bool distribution { get; set; }

        [DisplayName("Relationship")]
        public bool relationship { get; set; }

        [Display(Name = "Dimension count")]             // list of dimention that can visualize
        public List<int> dimensionCount { get; set; }

        [Display(Name = "Dimension count")]         // number of maximum dimention can be store
        public int dimentionCountVal { get; set; }

        public List<DimensionTemplate> dimentionList { get; set; }

    }
}