//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FYP_MVC.Models.DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class chartRuleBase_View
    {
        public int ID { get; set; }
        public string name { get; set; }
        public Nullable<int> count { get; set; }
        public Nullable<int> dimensionIndex { get; set; }
        public Nullable<int> isContinuous { get; set; }
        public string context { get; set; }
        public string cardinality { get; set; }
        public string intention { get; set; }
    }
}
