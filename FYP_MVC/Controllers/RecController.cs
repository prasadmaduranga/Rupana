using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_MVC.Models;
using FYP_MVC.Core.Recommendations;
using FYP_MVC.Models.DAO;
using FYP_MVC.Models.CoreObjects;
using FYP_MVC.Core.Injector;
namespace FYP_MVC.Controllers
{
    public class RecController : Controller
    {
        // GET: Recommendation
 
        public ActionResult ShowRecommendation(int tableID, CSVFile csv) 
        {
            List<Recommendations_Result> recommendations = Recommendation.getRecommendationsWithoutIntention(tableID);
            List<string> arr = new List<string>();
            foreach (var item in recommendations)
            {
                arr.Add(item.chartName);
            }
            arr = arr.Distinct().ToList();
            int ColCount = recommendations.Count / arr.Count;

            ChartVisualizationObject mainObj = new ChartVisualizationObject();
            mainObj.chartTypes = new string[arr.Count];
            mainObj.chartTypes = arr.ToArray();
            mainObj.mappingList = new List<int[]>();
        
            int counter = 0;
            for (int i = 0; i < arr.Count; i++)
            {
                int[] TableDimIndex = new int[ColCount];
                int[] ChartDimIndex = new int[ColCount];
                for (int j = i*ColCount; j < (i+1)*ColCount; j++)
                {
                    TableDimIndex[counter] = recommendations[j].tableDimIndex.Value;
                    ChartDimIndex[counter] = recommendations[j].chartDimIndex.Value;
                    counter++;
                }
                counter = 0;
                int[] finalArr = new int[ColCount];
                for (int k = 0; k < ColCount; k++)
                {
                    finalArr[k] = TableDimIndex[ChartDimIndex[k]-1];
                }
                mainObj.mappingList.Add(finalArr);  
            }
            //Setting dependency injection
            ChartVisualizationObjectInjector.CVObject = mainObj;

            return View(mainObj);
        }
    }
}