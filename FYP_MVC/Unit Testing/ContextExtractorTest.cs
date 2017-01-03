using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FYP_MVC.Models;
using FYP_MVC.Core.ContextRecognizer;
namespace FYP_MVC.Unit_Testing
{
    [TestClass]
    public class ContextExtractorTest
    {
        [TestMethod]
        public void test_DateTime()
        {
            CSVFile csv = new CSVFile();
            Column col = new Column();
            csv.Data = new Column[1];
            csv.Data[0] = col;
            col.Data = new List<string>();
            col.Data.Add("2015");
            col.Data.Add("2014");
            col.Data.Add("April");
            col.Data.Add("2011");
            col.Data.Add("March");
            ContextExtractor con = new ContextExtractor(csv);
            
            Assert.AreEqual(5f, con.checkForDate(col));

        }


    }
}