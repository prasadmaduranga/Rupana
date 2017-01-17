using FYP_MVC.Models.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_MVC.Core.Recommendations
{
    public class Recommendation
    {
        private static FYPEntities db = new FYPEntities();


        public static List<Recommendations_Result> getRecommendations(int tableID, string intention)
        {
            return db.getRecommendations(tableID, intention).ToList<Recommendations_Result>();


        }
        public static List<Recommendations_Result> getRecommendationsMore(int tableID, string intention)
        {
            return db.getRecommendations_More(tableID, intention).ToList<Recommendations_Result>();


        }
        public static List<Recommendations_Result> getRecommendationsWithoutIntention(int tableID)
        {
            return db.getRecommendations_WithoutIntention(tableID).ToList<Recommendations_Result>();


        }
        public static List<Recommendations_Result> getRecommendationsWithoutIntentionMore(int tableID, string intention)
        {
            return db.getRecommendations_More_WithoutIntention(tableID).ToList<Recommendations_Result>();


        }
    }
}