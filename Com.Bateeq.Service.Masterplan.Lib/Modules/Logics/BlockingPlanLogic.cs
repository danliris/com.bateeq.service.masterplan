using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BlockingPlanLogic : BaseLogic<BlockingPlan>
    {
        private BookingOrderLogic BookingOrderLogic;
        private BlockingPlanWorkScheduleLogic BlockingPlanWorkScheduleLogic;
        public BlockingPlanLogic(BlockingPlanWorkScheduleLogic blockingPlanWorkScheduleLogic, IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.BlockingPlanWorkScheduleLogic = blockingPlanWorkScheduleLogic;
        }

        public override void CreateModel(BlockingPlan model)
        {
            foreach (BlockingPlanWorkSchedule item in model.WorkSchedules)
            {
                BlockingPlanWorkScheduleLogic.CreateModel(item);
            }
            base.CreateModel(model);
        }

        public override Task<BlockingPlan> ReadModelById(int id)
        {
            return DbSet.Include(d => d.WorkSchedules).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public override async void UpdateModel(int id, BlockingPlan model)
        {
            if (model.WorkSchedules != null)
            {
                HashSet<int> detailIds = BlockingPlanWorkScheduleLogic.GetBlockingPlanWorkScheduleIds(id);

                foreach (int detailId in detailIds)
                {
                    BlockingPlanWorkSchedule detail = model.WorkSchedules.FirstOrDefault(prop => prop.Id.Equals(detailId));
                    if (detail == null)
                        await BlockingPlanWorkScheduleLogic.DeleteModel(detailId);
                    else
                        BlockingPlanWorkScheduleLogic.UpdateModel(detailId, detail);
                }

                foreach (BlockingPlanWorkSchedule item in model.WorkSchedules)
                {
                    if (item.Id == 0)
                        BlockingPlanWorkScheduleLogic.CreateModel(item);
                }
            }
            base.UpdateModel(id, model);
        }

        public override async Task DeleteModel(int id)
        {
            BlockingPlan model = await ReadModelById(id);

            foreach (var item in model.WorkSchedules)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, "masterplan-service");
            }

            BookingOrder bookingOrder = await BookingOrderLogic.ReadModelById(model.BookingOrderId);
            BookingOrderLogic.UpdateModelBlockingPlanId(bookingOrder.Id, bookingOrder, null);

            EntityExtension.FlagForDelete(model, IdentityService.Username, "masterplan-service", true);
            DbSet.Update(model);
        }
    }
}
