﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class FYPEntities : DbContext
    {
        public FYPEntities()
            : base("name=FYPEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<chart> charts { get; set; }
        public virtual DbSet<chartDimension> chartDimensions { get; set; }
        public virtual DbSet<chartDimensionCardinality> chartDimensionCardinalities { get; set; }
        public virtual DbSet<column> columns { get; set; }
        public virtual DbSet<dimensionContext> dimensionContexts { get; set; }
        public virtual DbSet<dimensionCount> dimensionCounts { get; set; }
        public virtual DbSet<headerContext> headerContexts { get; set; }
        public virtual DbSet<intention> intentions { get; set; }
        public virtual DbSet<originalDataFile> originalDataFiles { get; set; }
        public virtual DbSet<table> tables { get; set; }
        public virtual DbSet<tableDimension> tableDimensions { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<visualizedDataFile> visualizedDataFiles { get; set; }
        public virtual DbSet<feedBack> feedBacks { get; set; }
        public virtual DbSet<userFeedBack> userFeedBacks { get; set; }
    
        public virtual int getRecommendationFromRules(Nullable<int> tableID, string intention)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            var intentionParameter = intention != null ?
                new ObjectParameter("intention", intention) :
                new ObjectParameter("intention", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("getRecommendationFromRules", tableIDParameter, intentionParameter);
        }
    
        public virtual int getRecommendationFromRules_WithoutIntention_(Nullable<int> tableID)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("getRecommendationFromRules_WithoutIntention_", tableIDParameter);
        }
    
        public virtual ObjectResult<Recommendations_Result> getRecommendations(Nullable<int> tableID, string intention)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            var intentionParameter = intention != null ?
                new ObjectParameter("intention", intention) :
                new ObjectParameter("intention", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Recommendations_Result>("getRecommendations", tableIDParameter, intentionParameter);
        }
    
        public virtual ObjectResult<Recommendations_Result> getRecommendations_More(Nullable<int> tableID, string intention)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            var intentionParameter = intention != null ?
                new ObjectParameter("intention", intention) :
                new ObjectParameter("intention", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Recommendations_Result>("getRecommendations_More", tableIDParameter, intentionParameter);
        }
    
        public virtual ObjectResult<Recommendations_Result> getRecommendations_More_WithoutIntention(Nullable<int> tableID)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Recommendations_Result>("getRecommendations_More_WithoutIntention", tableIDParameter);
        }
    
        public virtual ObjectResult<Recommendations_Result> getRecommendations_WithoutIntention(Nullable<int> tableID)
        {
            var tableIDParameter = tableID.HasValue ?
                new ObjectParameter("tableID", tableID) :
                new ObjectParameter("tableID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Recommendations_Result>("getRecommendations_WithoutIntention", tableIDParameter);
        }

        public System.Data.Entity.DbSet<FYP_MVC.Models.Auth.LoginViewModel> LoginViewModels { get; set; }

        public System.Data.Entity.DbSet<FYP_MVC.Models.Auth.RegisterViewModel> RegisterViewModels { get; set; }

        public System.Data.Entity.DbSet<FYP_MVC.Models.DAO.Recommendations_Result> Recommendations_Result { get; set; }
    }
}
