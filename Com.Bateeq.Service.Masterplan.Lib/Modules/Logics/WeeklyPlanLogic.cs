using System.Linq;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using System.Linq.Dynamic.Core;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using System.Collections.Generic;
using System;

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
            var weeklyPlan = await DbSet.Include(p => p.Items)
                                        .FirstOrDefaultAsync(d => d.Id.Equals(id) && 
                                                                  d.IsDeleted.Equals(false));
            weeklyPlan.Items = weeklyPlan.Items.OrderBy(s => s.WeekNumber, new WeekComparer()).ToArray();
            return weeklyPlan;
        }


        public async Task<WeeklyPlan> GetByYearAndUnitCode(string year, string code)
        {
            var model = await DbSet.Include(p => p.Items)
                                   .FirstOrDefaultAsync(item => item.Year == year && 
                                                                item.UnitCode == code && 
                                                                item.IsDeleted.Equals(false));
            return model;
        }

        public async Task UpdateByWeeklyplanItemByIdAndWeekId(int yearId, int weekId, double ehBooking)
        {
            WeeklyPlanItem weeklyPlanItem = new WeeklyPlanItem();
            var query = await DbSet.Include(weeklyplanItem => weeklyplanItem.Items)
                             .Where(weeklyplan => weeklyplan.Id == yearId)
                             .FirstOrDefaultAsync();
            
            foreach(var weeklyplanItem in query.Items)
            {
                if (weeklyplanItem.Id == weekId)
                {
                    weeklyPlanItem = weeklyplanItem;
                    weeklyplanItem.UsedEh = weeklyplanItem.UsedEh + ehBooking;
                    weeklyplanItem.RemainingEh = weeklyplanItem.EhTotal - weeklyplanItem.UsedEh;
                }
            }

            UpdateModel(query.Id, query);
        }
    }

    public class WeekComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            return string.Compare(s1, s2, true);
        }

        public static bool IsNumeric(object value)
        {
            int week;
            return int.TryParse(value.ToString(), out week);
        }
    }
}
