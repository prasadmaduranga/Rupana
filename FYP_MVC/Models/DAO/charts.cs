using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models.DAO
{
    
    [MetadataType(typeof(chartMetaData))]
    public partial class chart
    {

    }
    public class chartMetaData
    {
        [Display(Name = "Chart Name")]
        public string name { get; set; }
    }
}