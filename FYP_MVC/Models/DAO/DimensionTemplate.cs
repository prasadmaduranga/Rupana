using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYP_MVC.Models.DAO
{
    public class DimensionTemplate
    {
        [Display(Name = "Dimension Index")]
        public int dimensionIndex { get; set; }

        [Display(Name = "Is Selected")]
        public bool isSelected { get; set; }

        [Display(Name = "Is Contineous")]
        public bool isContineous { get; set; }

        [Display(Name = "Cardinality type")]
        public List<chartDimensionCardinality> cardinalityType { get; set; }

        [Display(Name = "Cardinality High")]
        public bool cardinalityHigh { get; set; }

        [Display(Name = "Cardinality Medium")]
        public bool cardinalityMedium { get; set; }

        [Display(Name = "Cardinality Low")]
        public bool cardinalityLow { get; set; }

        [Display(Name = "Cardinality Any")]
        public bool cardinalityAny { get; set; }



        [Display(Name = "context type")]
        public List<dimensionContext> contextType { get; set; }

        [Display(Name = "Context Time series")]
        public bool contextTimeseries { get; set; }

        [Display(Name = "Context Numeric")]
        public bool contextNumeric { get; set; }

        [Display(Name = "Context Location")]
        public bool contextLocation { get; set; }

        [Display(Name = "Context Percentage")]
        public bool contextPercentage { get; set; }

        [Display(Name = "Context Nominal")]
        public bool contextNominal { get; set; }



    }
}