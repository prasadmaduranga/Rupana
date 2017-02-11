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
            bool intensionAvailable = (CSVInjector.csv.Intension !="None");
            List<Recommendations_Result> recommendations = null;
            List<Recommendations_Result> more_Recommendations = null;
            if (intensionAvailable)
            {
                recommendations = Recommendation.getRecommendations(tableID, CSVInjector.csv.Intension);
                more_Recommendations = Recommendation.getRecommendationsMore(tableID, CSVInjector.csv.Intension);
            }
            else {
                recommendations = Recommendation.getRecommendationsWithoutIntention(tableID);
                more_Recommendations = Recommendation.getRecommendationsWithoutIntentionMore(tableID);
            }
        
            List<string> arr = new List<string>();
            List<string> more_arr = new List<string>();

            foreach (var item in recommendations)
            {
                arr.Add(item.chartName);
            }
            arr = arr.Distinct().ToList();
            
            foreach (var item in more_Recommendations)
            {
                more_arr.Add(item.chartName);
            }
            more_arr = more_arr.Distinct().ToList();
            int ColCount = recommendations.Count / arr.Count;
            int ColCountMore = more_Recommendations.Count / more_arr.Count;

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

          
            mainObj.more_chartTypes = new string[more_arr.Count];
            mainObj.more_chartTypes = more_arr.ToArray();
            mainObj.more_mappingList = new List<int[]>();
            counter = 0;
            for (int i = 0; i < more_arr.Count; i++)
            {
                int[] TableDimIndex = new int[ColCountMore];
                int[] ChartDimIndex = new int[ColCountMore];
                for (int j = i * ColCountMore; j < (i + 1) * ColCountMore; j++)
                {
                    TableDimIndex[counter] = more_Recommendations[j].tableDimIndex.Value;
                    ChartDimIndex[counter] = more_Recommendations[j].chartDimIndex.Value;
                    counter++;
                }
                counter = 0;
                int[] finalArr2 = new int[ColCountMore];
                for (int k = 0; k < ColCountMore; k++)
                {
                    finalArr2[k] = TableDimIndex[ChartDimIndex[k] - 1];
                }
                mainObj.more_mappingList.Add(finalArr2);
            }
            
            //Setting dependency injection
            ChartVisualizationObjectInjector.CVObject = mainObj;

            return View(mainObj);
        }
    }
}