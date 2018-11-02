using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades
{
    public class WeeklyPlanItemFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<WeeklyPlanItem> DbSet;

        public WeeklyPlanItemFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<WeeklyPlanItem>();
        }

        public ReadResponse<WeeklyPlanItem> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<WeeklyPlanItem> Query = this.DbSet;
            List<string> SearchAttributes = new List<string>()
                {
                    "WeekNumber"
                };
            Query = QueryHelper<WeeklyPlanItem>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<WeeklyPlanItem>.Filter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>() { "Id", "WeekNumber", "RemainingEh", "StartDate", "EndDate", "Efficiency" };
            Query = Query.Select(field => new WeeklyPlanItem
            {
                Id = field.Id,
                WeekNumber = field.WeekNumber,
                RemainingEh = field.RemainingEh,
                StartDate = field.StartDate,
                EndDate = field.EndDate,
                Efficiency = field.Efficiency
            });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<WeeklyPlanItem>.Order(Query, OrderDictionary);
            Pageable<WeeklyPlanItem> pageable = new Pageable<WeeklyPlanItem>(Query, page - 1, size);
            List<WeeklyPlanItem> Data = pageable.Data.ToList<WeeklyPlanItem>();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<WeeklyPlanItem>(Data, TotalData, OrderDictionary, SelectedFields);
        }
    }
}
