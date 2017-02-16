using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FYP_MVC.Controllers;
using System.Web.Mvc;

namespace FYP_MVC.Tests
{
    [TestClass]
    public class test1
    {
        [TestMethod]
        public void testMethod()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

    }
}