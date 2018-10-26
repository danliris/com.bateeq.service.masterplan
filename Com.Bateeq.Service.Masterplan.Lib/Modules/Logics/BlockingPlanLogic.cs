using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BlockingPlanLogic : BaseLogic<BlockingPlan>
    {
        private BookingOrderLogic BookingOrderLogic;
        private BlockingPlanWorkScheduleLogic BlockingPlanWorkScheduleLogic;
        private MasterplanDbContext DbContext;
        public BlockingPlanLogic(BookingOrderLogic bookingOrderLogic, BlockingPlanWorkScheduleLogic blockingPlanWorkScheduleLogic, IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.BlockingPlanWorkScheduleLogic = blockingPlanWorkScheduleLogic;
            this.BookingOrderLogic = bookingOrderLogic;
            this.DbContext = dbContext;
        }

        public override void CreateModel(BlockingPlan model)
        {
            if (model.WorkSchedules != null)
            {
                int count = 0;
                foreach (BlockingPlanWorkSchedule item in model.WorkSchedules)
                {
                    if (item.isConfirmed)
                        count++;
                    BlockingPlanWorkScheduleLogic.CreateModel(item);
                }
                if (count == 0)
                    model.Status = BlockingPlanStatus.BOOKING;
                else if (count == model.WorkSchedules.Count)
                    model.Status = BlockingPlanStatus.FULL_CONFIRMED;
                else
                    model.Status = BlockingPlanStatus.HALF_CONFIRMED;
            }

            base.CreateModel(model);
        }

        public override async Task<BlockingPlan> ReadModelById(int id)
        {
            BlockingPlan blockingPlan = await DbSet.Include(d => d.WorkSchedules).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
            blockingPlan.BookingOrder = await DbContext.BookingOrders.Include(d => d.DetailConfirms).IgnoreQueryFilters().FirstOrDefaultAsync(d => d.Id.Equals(blockingPlan.BookingOrderId));
            return blockingPlan;
        }

        public override async void UpdateModel(int id, BlockingPlan model)
        {
            if (model.WorkSchedules != null)
            {
                HashSet<int> detailIds = BlockingPlanWorkScheduleLogic.GetBlockingPlanWorkScheduleIds(id);
                int countConfirmed = 0;

                foreach (int detailId in detailIds)
                {
                    BlockingPlanWorkSchedule detail = model.WorkSchedules.FirstOrDefault(prop => prop.Id.Equals(detailId));
                    if (detail == null)
                    {
                        await BlockingPlanWorkScheduleLogic.DeleteModel(detailId);
                    }     
                    else
                    {
                        if (detail.isConfirmed == true)
                        {
                            countConfirmed++;
                        }
                        BlockingPlanWorkScheduleLogic.UpdateModel(detailId, detail);
                    }
                }
                foreach (BlockingPlanWorkSchedule item in model.WorkSchedules)
                {
                    if (item.Id == 0)
                        BlockingPlanWorkScheduleLogic.CreateModel(item);
                }

                #region Set Status Blocking Plan Sewing
                if (countConfirmed == model.WorkSchedules.Count)
                {
                    model.Status = BlockingPlanStatus.FULL_CONFIRMED;
                }
                else if (countConfirmed == 0)
                {
                    model.Status = BlockingPlanStatus.BOOKING;
                }
                else
                {
                    model.Status = BlockingPlanStatus.HALF_CONFIRMED;
                }
                #endregion

            }
            base.UpdateModel(id, model);
        }

        public void UpdateModelStatus(int id, BlockingPlan model, string status)
        {
            model.Status = status;
            base.UpdateModel(id, model);
        }

        public Task<BlockingPlan> searchByBookingOrderId(int bookingOrderId)
        {
            var blockingPlanResult = base.DbSet.Where(blockingPlan => blockingPlan.BookingOrderId == bookingOrderId && blockingPlan.IsDeleted == false).FirstOrDefaultAsync();
            return blockingPlanResult;
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
