using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYP_MVC.Models.DAO
{
  
    [MetadataType(typeof(Recommendations_ResultMetaData))]
    public partial class Recommendations_Result
    {

    }

    public class Recommendations_ResultMetaData
    {
        [Key]
        [Column(Order = 0)]
        public string chartName { get; set; }
        [Key]
        [Column(Order = 1)]
        public Nullable<int> tableDimIndex { get; set; }
        [Key]
        [Column(Order = 2)]
        public Nullable<int> chartDimIndex { get; set; }
    }
}