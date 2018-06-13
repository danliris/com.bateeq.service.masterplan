using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Implementation;
using Com.Bateeq.Service.Masterplan.Lib.Utils;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades
{
    public class WeeklyplanFacade : IBaseFacade<WeeklyPlan>
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<WeeklyPlan> DbSet;
        private IdentityService IdentityService;
        private WeeklyPlanLogic WeeklyPlanLogic;
        private ValidateService ValidateService;

        public WeeklyplanFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
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

        public ReadResponse<WeeklyPlan> Read(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            return WeeklyPlanLogic.ReadModel(Page, Size, Order, Select, Keyword, Filter);
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

        public async Task<WeeklyPlan> GetByYearAndUnitCode(int year, string code)
        {
            var weeklyPlan = await WeeklyPlanLogic.GetByYearAndUnitCode(year, code);
            return weeklyPlan;
        }
    }
}
