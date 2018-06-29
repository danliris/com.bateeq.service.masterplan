using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BlockingPlanWorkScheduleLogic : BaseLogic<BlockingPlanWorkSchedule>
    {
        public BlockingPlanWorkScheduleLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
        }

        public HashSet<int> GetBlockingPlanWorkScheduleIds(int id)
        {
            return new HashSet<int>(DbSet.Where(d => d.BlockingPlanId == id).Select(d => d.Id));
        }
    }
}