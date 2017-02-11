using FYP_MVC.Models;
using FYP_MVC.Models.DAO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models.Auth;
using System.Text;

namespace FYP_MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private FYPEntities db = new FYPEntities();
        public ActionResult Index()
        {
            ViewData["activeMenu"] = "Index";
            return View();
        }

        public ActionResult Home()
        {
            ViewData["activeMenu"] = "Home";
            return View();
        }

        //----------- chart related controllers----------------------------------------------
        /**
        list all available charts
            */
        public ActionResult ChartList()
        {
            ViewData["activeMenu"] = "ChartList";
            return View(db.charts.ToList());
        }

        /**
        get detail of single chart
            */
        public ActionResult ChartDetails(int id)
        {
            ViewData["activeMenu"] = "ChartList";
            chart result = db.charts.Find(id);
            ChartTemplate chartData = new ChartTemplate();
            chartData.name = result.name;
            // search for intention
            List<intention> intensionList = db.intentions.Where(b => b.chartID == id).ToList();
            chartData.intentionList = intensionList;        // add intention

            // search for dimension count
            List<dimensionCount> dimensionCount = db.dimensionCounts.Where(b => b.chartID == id).ToList();
            chartData.dimensionCount = new List<int>();
            foreach (var eachDim in dimensionCount)
            {
                chartData.dimensionCount.Add(eachDim.count.Value);
            }

            // search for each dimension
            List<chartDimension> dimensionChart = db.chartDimensions.Where(b => b.chartID == id).ToList();
            chartData.dimentionList = new List<DimensionTemplate>();
            foreach (var item in dimensionChart)
            {

                DimensionTemplate dimensionTemplate = new DimensionTemplate();
                dimensionTemplate.isContineous = Convert.ToBoolean(item.isContinuous);  // add is contineous
                dimensionTemplate.dimensionIndex = item.dimensionIndex.Value;


                // search for cardinality
                List<chartDimensionCardinality> dimensionChartCardinality = db.chartDimensionCardinalities.Where(b => b.dimensionID == item.ID).ToList();
                dimensionTemplate.cardinalityType = dimensionChartCardinality;

                // search for context
                List<dimensionContext> dimentionContext = db.dimensionContexts.Where(b => b.dimensionID == item.ID).ToList();
                dimensionTemplate.contextType = dimentionContext;

                chartData.dimentionList.Add(dimensionTemplate);
            }

            return View(chartData);
        }
        /**
        view form to add new chart
            */
        public ActionResult AddNewChart()
        {
            ViewData["activeMenu"] = "AddNewChart";
            return View();
        }
        /**
        second step to edit dimension data
            */
        public ActionResult AddNewChartName(ChartTemplate model)
        {
            //return "comparison" + model.comparison + "composition" + model.composition+"name"+model.name+"dimcount"+model.dimensionCount;
            Session["dimensionData"] = model;
            ViewData["dimensionCount"] = model.dimentionCountVal;
            List<DimensionTemplate> temp = new List<DimensionTemplate>();
            for (int i = 0; i < model.dimentionCountVal; i++)
            {
                DimensionTemplate template = new DimensionTemplate();
                template.dimensionIndex = i;
                temp.Add(template);
            }
            return View(temp);
        }

        /**
        put new chart into the database
            */
        public ActionResult AddNewChartNameVal(List<DimensionTemplate> dimList)
        {
            string val = "";
            ChartTemplate sessionModel = (ChartTemplate)Session["dimensionData"];

            chart chartTable = new chart();
            chartTable.name = sessionModel.name;
            db.charts.Add(chartTable);
            db.SaveChanges();


            if (sessionModel.comparison)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTable.ID;
                tableIntention.intention1 = "Comparison";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
                val += ",comparison added";
            }
            if (sessionModel.composition)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTable.ID;
                tableIntention.intention1 = "Composition";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
                val += ",composition added";
            }
            if (sessionModel.distribution)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTable.ID;
                tableIntention.intention1 = "Distribution";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
                val += ",distribution added";
            }
            if (sessionModel.relationship)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTable.ID;
                tableIntention.intention1 = "Relationship";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
                val += ",relationip added";
            }

            foreach (var item in dimList)
            {
                item.dimensionIndex++;
                chartDimension tableChartDimension = new chartDimension();
                tableChartDimension.chartID = chartTable.ID;
                tableChartDimension.dimensionIndex = item.dimensionIndex;
                tableChartDimension.isContinuous = Convert.ToInt32(item.isContineous);
                db.chartDimensions.Add(tableChartDimension);
                db.SaveChanges();

                if (item.isSelected)
                {
                    dimensionCount tableDimensionCount = new dimensionCount();
                    tableDimensionCount.chartID = chartTable.ID;
                    tableDimensionCount.count = item.dimensionIndex;
                    db.dimensionCounts.Add(tableDimensionCount);
                    db.SaveChanges();
                    val += ",isselected added";
                }

                if (item.cardinalityAny)
                {
                    chartDimensionCardinality tableChartDimensionCardinality = new chartDimensionCardinality();
                    tableChartDimensionCardinality.dimensionID = tableChartDimension.ID;
                    tableChartDimensionCardinality.cardinality = "Any";
                    db.chartDimensionCardinalities.Add(tableChartDimensionCardinality);
                    db.SaveChanges();
                }
                else
                {
                    if (item.cardinalityHigh)
                    {
                        chartDimensionCardinality tableChartDimensionCardinality = new chartDimensionCardinality();
                        tableChartDimensionCardinality.dimensionID = tableChartDimension.ID;
                        tableChartDimensionCardinality.cardinality = "High";
                        db.chartDimensionCardinalities.Add(tableChartDimensionCardinality);
                        db.SaveChanges();
                    }
                    if (item.cardinalityMedium)
                    {
                        chartDimensionCardinality tableChartDimensionCardinality = new chartDimensionCardinality();
                        tableChartDimensionCardinality.dimensionID = tableChartDimension.ID;
                        tableChartDimensionCardinality.cardinality = "Medium";
                        db.chartDimensionCardinalities.Add(tableChartDimensionCardinality);
                        db.SaveChanges();
                    }
                    if (item.cardinalityLow)
                    {
                        chartDimensionCardinality tableChartDimensionCardinality = new chartDimensionCardinality();
                        tableChartDimensionCardinality.dimensionID = tableChartDimension.ID;
                        tableChartDimensionCardinality.cardinality = "Low";
                        db.chartDimensionCardinalities.Add(tableChartDimensionCardinality);
                        db.SaveChanges();
                    }
                }

                if (item.contextLocation)
                {
                    dimensionContext tableDimensionContext = new dimensionContext();
                    tableDimensionContext.dimensionID = tableChartDimension.ID;
                    tableDimensionContext.context = "Location";
                    db.dimensionContexts.Add(tableDimensionContext);
                    db.SaveChanges();
                }
                if (item.contextNominal)
                {
                    dimensionContext tableDimensionContext = new dimensionContext();
                    tableDimensionContext.dimensionID = tableChartDimension.ID;
                    tableDimensionContext.context = "Nominal";
                    db.dimensionContexts.Add(tableDimensionContext);
                    db.SaveChanges();
                }
                if (item.contextNumeric)
                {
                    dimensionContext tableDimensionContext = new dimensionContext();
                    tableDimensionContext.dimensionID = tableChartDimension.ID;
                    tableDimensionContext.context = "Numeric";
                    db.dimensionContexts.Add(tableDimensionContext);
                    db.SaveChanges();
                }
                if (item.contextPercentage)
                {
                    dimensionContext tableDimensionContext = new dimensionContext();
                    tableDimensionContext.dimensionID = tableChartDimension.ID;
                    tableDimensionContext.context = "Percentage";
                    db.dimensionContexts.Add(tableDimensionContext);
                    db.SaveChanges();
                }
                if (item.contextTimeseries)
                {
                    dimensionContext tableDimensionContext = new dimensionContext();
                    tableDimensionContext.dimensionID = tableChartDimension.ID;
                    tableDimensionContext.context = "Time series";
                    db.dimensionContexts.Add(tableDimensionContext);
                    db.SaveChanges();
                }



            }






            return RedirectToAction("ChartList", "Admin");
        }
        public ActionResult EditChart(int id)
        {
            ChartTemplate chartTemplate = new ChartTemplate();
            chartTemplate.id = id;
            var resultChart = db.charts.Where(p => p.ID == id);
            foreach (var item in resultChart)
            {
                chartTemplate.name = item.name;                             // update from chart table
            }

            var intentionVal = db.intentions.Where(p => p.chartID == id);   
            foreach (var intenDim in intentionVal)                          // update intention table
            {   
                if(intenDim.intention1== "Comparison")
                {
                    chartTemplate.comparison = true;
                }
                else
                {
                    chartTemplate.comparison = false;
                }
                if (intenDim.intention1 == "Composition")
                {
                    chartTemplate.composition = true;
                }
                else
                {
                    chartTemplate.composition = false;
                }
                if (intenDim.intention1 == "Distribution")
                {
                    chartTemplate.distribution = true;
                }
                else
                {
                    chartTemplate.distribution = false;
                }
                if (intenDim.intention1 == "Relationship")
                {
                    chartTemplate.relationship = true;
                }
                else
                {
                    chartTemplate.relationship = false;
                }

            }

            var chartDim = db.chartDimensions.Where(p => p.chartID == id);
            chartTemplate.dimentionList = new List<DimensionTemplate>();
            foreach (var itemDim in chartDim)
            {
                DimensionTemplate dimention = new DimensionTemplate();
                if (itemDim.isContinuous == 1)
                {
                    dimention.isContineous = true;
                }
                else
                {
                    dimention.isContineous = false;
                }
                dimention.dimensionIndex = itemDim.dimensionIndex.Value;

                var contextDim = db.dimensionContexts.Where(d => d.dimensionID == itemDim.ID);
                foreach(var contextVar in contextDim)
                {
                    if(contextVar.context== "Location")
                    {
                        dimention.contextLocation = true;
                    }
                    else
                    {
                        dimention.contextLocation = false;
                    }
                    if (contextVar.context == "Nominal")
                    {
                        dimention.contextNominal = true;
                    }
                    else
                    {
                        dimention.contextNominal = false;
                    }
                    if (contextVar.context == "Numeric")
                    {
                        dimention.contextNumeric = true;
                    }
                    else
                    {
                        dimention.contextNumeric = false;
                    }
                    if (contextVar.context == "Percentage")
                    {
                        dimention.contextPercentage = true;
                    }
                    else
                    {
                        dimention.contextPercentage = false;
                    }
                    if (contextVar.context == "Time series")
                    {
                        dimention.contextTimeseries = true;
                    }
                    else
                    {
                        dimention.contextTimeseries = false;
                    }
                }

                chartTemplate.dimentionList.Add(dimention);
            }

            return View(chartTemplate);
        }

        public ActionResult EditChartVal(ChartTemplate chartTemplate)
        {
            chart resultChart = db.charts.Where(s => s.ID == chartTemplate.id).FirstOrDefault<chart>();
            resultChart.name = chartTemplate.name;
            db.Entry(resultChart).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var intentionVal = db.intentions.Where(p => p.chartID == chartTemplate.id);

            foreach (var intenDim in intentionVal)                          // update intention table
            {
                db.intentions.Remove(intenDim);

            }
            db.Entry(resultChart).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            if (chartTemplate.comparison)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTemplate.id;
                tableIntention.intention1 = "Comparison";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
            }
            if (chartTemplate.composition)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTemplate.id;
                tableIntention.intention1 = "Composition";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
            }
            if (chartTemplate.distribution)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTemplate.id;
                tableIntention.intention1 = "Distribution";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
            }
            if (chartTemplate.relationship)
            {
                intention tableIntention = new intention();
                tableIntention.chartID = chartTemplate.id;
                tableIntention.intention1 = "Relationship";
                db.intentions.Add(tableIntention);
                db.SaveChanges();
            }

            foreach(var dimList in chartTemplate.dimentionList)
            {
                chartDimension chartDim = db.chartDimensions.Where(s => s.chartID == chartTemplate.id).Where(p=>p.dimensionIndex==dimList.dimensionIndex).FirstOrDefault<chartDimension>();
                chartDim.isContinuous = Convert.ToInt32(dimList.dimensionIndex);
                db.Entry(chartDim).State = System.Data.Entity.EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
                


                var dimContext = db.dimensionContexts.Where(s => s.dimensionID == dimList.dimensionIndex);
                foreach(var listDimContext in dimContext)
                {
                    db.dimensionContexts.Remove(listDimContext);
                }
                try
                {
                    db.SaveChanges();
                }catch(Exception e)
                {

                }
                
                if (dimList.cardinalityAny)
                {
                    dimensionContext dimCon = new dimensionContext();
                    dimCon.dimensionID = dimList.dimensionIndex;
                    dimCon.context = "Any";
                    db.dimensionContexts.Add(dimCon);
                }
                if (dimList.contextTimeseries)
                {
                    dimensionContext dimCon = new dimensionContext();
                    dimCon.dimensionID = dimList.dimensionIndex;
                    dimCon.context = "Time series";
                    db.dimensionContexts.Add(dimCon);
                }
                if (dimList.contextNumeric)
                {
                    dimensionContext dimCon = new dimensionContext();
                    dimCon.dimensionID = dimList.dimensionIndex;
                    dimCon.context = "Numeric";
                    db.dimensionContexts.Add(dimCon);
                }
                if (dimList.contextNominal)
                {
                    dimensionContext dimCon = new dimensionContext();
                    dimCon.dimensionID = dimList.dimensionIndex;
                    dimCon.context = "Nominal";
                    db.dimensionContexts.Add(dimCon);
                }
                if (dimList.contextLocation)
                {
                    dimensionContext dimCon = new dimensionContext();
                    dimCon.dimensionID = dimList.dimensionIndex;
                    dimCon.context = "Location";
                    db.dimensionContexts.Add(dimCon);
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
            

            return RedirectToAction("ChartList", "Admin");
        }

        public ActionResult DeleteChart(int id)
        {

            var wcd1 = db.charts.Where(p => p.ID == id);

            foreach (var wc in wcd1.ToList())
            {
                db.charts.Remove(wc);                   // remove from charts
            }
            var wcd2 = db.intentions.Where(p => p.chartID == id);

            foreach (var wc in wcd2.ToList())
            {
                db.intentions.Remove(wc);               // remove from intention
            }
            var wcd3 = db.dimensionCounts.Where(p => p.chartID == id);

            foreach (var wc in wcd3.ToList())
            {
                db.dimensionCounts.Remove(wc);          // remove from dimensionCount
            }


            var dimList = db.chartDimensions.Where(p => p.chartID == id);
            foreach (var dimVar in dimList)
            {
                var dimCardinality = db.chartDimensionCardinalities.Where(p => p.dimensionID == dimVar.ID);
                foreach (var dimCardinalityVar in dimCardinality)
                {
                    db.chartDimensionCardinalities.Remove(dimCardinalityVar);   // remove from chartDimensionCardinalities
                }

                var dimContext = db.dimensionContexts.Where(p => p.dimensionID == dimVar.ID);
                foreach (var dimContextVar in dimContext)
                {
                    db.dimensionContexts.Remove(dimContextVar);                 // remove from dimesionContexts
                }

                db.chartDimensions.Remove(dimVar);

            }

            db.SaveChanges();
            return RedirectToAction("ChartList", "Admin");
        }

        //-------------- user related controllers-----------------------------------------------
        public ActionResult UserList()
        {
            ViewData["activeMenu"] = "UserList";
            return View(db.users.ToList());
        }

        /**
        View page to add new user into the system
            */
        public ActionResult AddNewUser()
        {
            return View();
        }

        /**
        add/edit user data into the system
            */
        public ActionResult AddNewUserVal(user model)
        {

            MD5 md5Hash = MD5.Create();
            string hassedPassword = GetMd5Hash(md5Hash, model.email);
            user user = new user
            {
                firstName = model.firstName,
                lastName = model.lastName,
                email = model.email,
                passwword = hassedPassword,
                userType = model.userType

            };

            // Add the new object to the Orders collection.
            db.users.Add(user);

            // Submit the change to the database.
            try
            {
                db.SaveChanges();
                List<user> users = db.users.Where(u => u.email == model.email).ToList();
                Session["user"] = users.FirstOrDefault();
                return RedirectToAction("UserList", "Admin");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError("", "Errors while creating your account. Try again..");
                return RedirectToAction("UserList", "Admin");
            }

            //return View(model);
        }

        public ActionResult EditUser(int id)
        {
            user tempUser = db.users.Find(id);
            tempUser.passwword = "";
            return View(tempUser);
        }
        public ActionResult EditUserVal(user model)
        {
            user editUser = db.users.Where(s => s.ID == model.ID).FirstOrDefault<user>();
            editUser.firstName = model.firstName;
            editUser.lastName = model.lastName;
            editUser.email = model.email;
            editUser.userType = model.userType;
            if (model.passwword != "")
            {
                MD5 md5Hash = MD5.Create();
                string hassedPassword = GetMd5Hash(md5Hash, model.email);
                editUser.passwword = hassedPassword;
            }
            db.Entry(editUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("UserList", "Admin");
            //return View(model);
        }

        public ActionResult UserDetails(int id)
        {
            return View(db.users.Find(id));
        }

        public ActionResult DeleteUser(int id)
        {
            var wcd = db.users.Where(p => p.ID == id);

            foreach (var wc in wcd.ToList())
            {
                db.users.Remove(wc);
            }

            db.SaveChanges();
            return RedirectToAction("UserList", "Admin");
        }

        public ActionResult MyProfile()
        {
            ViewData["activeMenu"] = "MyProfile";
           if(((user)Session["user"]) != null){
            int userId=  ((user)Session["user"]).ID;

            return View(db.users.Find(userId));
            }
            return RedirectToAction("Home", "Admin");
        }

        public String CheckCountry()
        {
            Column col = new Column();
            string loc1 = "rathgama";
            string loc2 = "London";
            string loc6 = "Dublin";
            string loc3 = "Paris";
            string loc4 = "NY";
            string loc5 = "Rathnapura";

            col.Data = new List<string>();
            col.Data.Add(loc6);
            col.Data.Add(loc2);
            col.Data.Add(loc1);


            int rowCount = col.Data.Count;
            string[] jsonResponse = new string[rowCount];
            /*
            for (int i = 0; i < rowCount; i++)
            {
                string address = col.Data[i];
                string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=AIzaSyBe7bmv5rusSTJ__tPpPoNkCUt0rxjR7jo";
                var request = (HttpWebRequest)WebRequest.Create(url);

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                jsonResponse[i] = responseString;
            }
           */
            //jsonResponse[0]="{ \"results" : [{\"address_components\" : [{\"long_name\" : \"Rathgama\",\"short_name\" : \"Rathgama\", \"types\" : [ \"locality\", \"political\" ]},{\"long_name\" : \"Galle\",  \"short_name\" : \"Galle\", \"types\" : [ \"administrative_area_level_2\", \"political\" ]},  { \"long_name\" : \"Southern Province\", \"short_name\" : \"SP\", \"types\" : [ \"administrative_area_level_1\", \"political\" ]}, { \"long_name\" : \"Sri Lanka\", \"short_name\" : \"LK\", \"types\" : [ \"country\", "political" ] } ], "formatted_address" : "Rathgama, Sri Lanka", "geometry" : { "bounds" : { "northeast" : {  "lat" : 6.0994063, "lng" : 80.15857869999999 }, "southwest" : { "lat" : 6.085765599999999, "lng" : 80.1325178} }, "location" : { "lat" : 6.0936187, "lng" : 80.14305419999999},"location_type" : "APPROXIMATE", "viewport" : {"northeast" : { "lat" : 6.0994063, "lng" : 80.15857869999999 },"southwest" : { "lat" : 6.085765599999999, "lng" : 80.1325178 } } }, "place_id" : "ChIJc73LoPd24ToRpNTaC290rk0", "types" : [ "locality", "political" ] } ], "status" : "OK"}"

            int arrayEntryCount = 0;
            var locationHeirarchy = new Dictionary<int, string>();
            string countryList = "";

            foreach (var val in jsonResponse)
            {
                arrayEntryCount++;
                dynamic jsonResult = JsonConvert.DeserializeObject(val);

                string resultStatus = jsonResult.status;

                if (resultStatus == "OK")
                {
                    //LocationCount++;
                    AddressComponents[] address = jsonResult.results[0].address_components.ToObject<AddressComponents[]>();
                    countryList += address[address.Length - 1].long_name;
                    countryList += ",";


                }
            }

            FYPEntities entity = new FYPEntities();

            var regionParameter = new ObjectParameter("region", typeof(string));
            var resolutionParameter = new ObjectParameter("resolution", typeof(string));

            entity.getRegionCodeAndResolution(countryList, regionParameter, resolutionParameter);


            return regionParameter.Value.ToString() + " " + resolutionParameter.Value.ToString();
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            System.Text.StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}