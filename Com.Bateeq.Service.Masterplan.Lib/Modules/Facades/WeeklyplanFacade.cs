using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Services.ValidateService;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Logics;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades
{
    public class WeeklyPlanFacade : IBaseFacade<WeeklyPlan>
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<WeeklyPlan> DbSet;
        private IdentityService IdentityService;
        private WeeklyPlanLogic WeeklyPlanLogic;
        private ValidateService ValidateService;

        public WeeklyPlanFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<WeeklyPlan>();
            this.IdentityService = serviceProvider.GetService<IdentityService>();
            this.WeeklyPlanLogic = serviceProvider.GetService<WeeklyPlanLogic>();
            this.ValidateService = serviceProvider.GetService<ValidateService>();
        }

        public async Task<int> Create(WeeklyPlan model)
        {
            WeeklyPlanLogic.CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            await WeeklyPlanLogic.DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<WeeklyPlan> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<WeeklyPlan> Query = this.DbSet;
            List<string> SearchAttributes = new List<string>()
                {
                    "Year"
                };
            Query = QueryHelper<WeeklyPlan>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<WeeklyPlan>.Filter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>() { "Id", "Year", "UnitId", "UnitCode", "UnitName" };
            Query = Query.Select(field => new WeeklyPlan
            {
                Id = field.Id,
                Year = field.Year,
                UnitId = field.UnitId,
                UnitCode = field.UnitCode,
                UnitName = field.UnitName
            });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<WeeklyPlan>.Order(Query, OrderDictionary);
            Pageable<WeeklyPlan> pageable = new Pageable<WeeklyPlan>(Query, page - 1, size);
            List<WeeklyPlan> Data = pageable.Data.ToList<WeeklyPlan>();
            int TotalData = pageable.TotalCount;

            return new ReadResponse<WeeklyPlan>(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public async Task<WeeklyPlan> ReadById(int id)
        {
            var weeklyPlan = await WeeklyPlanLogic.ReadModelById(id);
            return weeklyPlan;
        }

        public async Task<int> Update(int id, WeeklyPlan model)
        {
            WeeklyPlanLogic.UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<WeeklyPlan> GetByYearAndUnitCode(string year, string code)
        {
            var weeklyPlan = await WeeklyPlanLogic.GetByYearAndUnitCode(year, code);
            return weeklyPlan;
        }

        public async Task<List<WeeklyPlan>> ReadByYear(string year)
        {
            var weeklyPlans = await DbSet.Include(p => p.Items).Where(d => d.Year == year && d.IsDeleted == false).ToListAsync();
            foreach(var weeklyPlan in weeklyPlans)
                weeklyPlan.Items = weeklyPlan.Items.OrderBy(s => s.WeekNumber).ToArray();
            return weeklyPlans;
        }

        public async void UpdateWeeklyplanItem(ICollection<BlockingPlanWorkSchedule> model)
        {
            foreach (var workSchedule in model)
            {
                var ehBooking = workSchedule.EH_Booking;
                var weeklyPlan = await WeeklyPlanLogic.GetByYearAndUnitCode(workSchedule.YearText, workSchedule.UnitId);

                foreach(var weeklyplanItem in weeklyPlan.Items)
                {
                    weeklyplanItem.UsedEh = weeklyplanItem.UsedEh + ehBooking;
                    weeklyplanItem.RemainingEh = weeklyplanItem.EhTotal - weeklyplanItem.UsedEh;
                }

                WeeklyPlanLogic.UpdateModel(weeklyPlan.Id, weeklyPlan);
            }
        }
    }
}
