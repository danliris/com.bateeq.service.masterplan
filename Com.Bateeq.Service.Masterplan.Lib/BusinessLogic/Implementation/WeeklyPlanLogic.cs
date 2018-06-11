using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using System.Linq.Dynamic.Core;
using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using Com.Moonlay.Models;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Bateeq.Service.Masterplan.Lib.Services;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation
{
    public class WeeklyPlanLogic : BaseLogic<WeeklyPlan>
    {
        public WeeklyPlanLogic(IServiceProvider serviceProvider, MasterplanDbContext dbContext) : base(serviceProvider, dbContext)
        {
        }

        public override ReadResponse<WeeklyPlan> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<WeeklyPlan> Query = this.DbSet;
            List<string> SearchAttributes = new List<string>() { "Id", "Year", "UnitId", "UnitCode", "UnitName" };
            Query = QueryHelper.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper.Filter(Query, FilterDictionary);

            Query = Query.Select(field => new WeeklyPlan
            {
                Id = field.Id,
                Year = field.Year,
                UnitId = field.UnitId,
                UnitCode = field.UnitCode,
                UnitName = field.UnitName
            });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper.Order(Query, OrderDictionary);

            Pageable<WeeklyPlan> pageable = new Pageable<WeeklyPlan>(Query, page - 1, size);
            List<WeeklyPlan> Data = pageable.Data.ToList<WeeklyPlan>();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<WeeklyPlan>(Data, TotalData, OrderDictionary, SearchAttributes);
        }

        override public void CreateModel(WeeklyPlan model)
        {
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, "masterplan-service");
            }
            
            EntityExtension.FlagForCreate(model, IdentityService.Username, "masterplan-service");
            DbSet.Add(model);
        }

        override public void UpdateModel(int id, WeeklyPlan model)
        {
            foreach (var item in model.Items)
            {
                EntityExtension.FlagForUpdate(item, IdentityService.Username, "masterplan-service");
            }

            EntityExtension.FlagForUpdate(model, IdentityService.Username, "masterplan-service");
            DbSet.Update(model);
        }

        override public async Task DeleteModel(int id)
        {
            var model = await ReadModelById(id);

            foreach (var item in model.Items)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, "masterplan-service");
            }

            EntityExtension.FlagForDelete(model, IdentityService.Username, "masterplan-service", true);
            DbSet.Update(model);
        }
    }
}
