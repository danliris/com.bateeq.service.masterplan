using System.Linq;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using System.Linq.Dynamic.Core;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class WeeklyPlanLogic : BaseLogic<WeeklyPlan>
    {
        public WeeklyPlanLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
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

        override public async Task<WeeklyPlan> ReadModelById(int id)
        {
            var weeklyPlan = await DbSet.Include(p => p.Items).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
            weeklyPlan.Items = weeklyPlan.Items.OrderBy(s => s.WeekNumber).ToArray();
            return weeklyPlan;
        }


        public async Task<WeeklyPlan> GetByYearAndUnitCode(string year, string code)
        {
            var model = await DbSet.Include(p => p.Items).FirstOrDefaultAsync(item => item.Year == year && item.UnitCode == code && item.IsDeleted.Equals(false));
            return model;
        }
    }
}
