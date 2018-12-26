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
        private readonly BookingOrderLogic BookingOrderLogic;
        private readonly BookingOrderDetailLogic BookingOrderDetailLogic;
        private readonly BlockingPlanWorkScheduleLogic BlockingPlanWorkScheduleLogic;
        private readonly WeeklyPlanLogic WeeklyPlanLogic;
        private readonly MasterplanDbContext DbContext;
        protected DbSet<BookingOrderDetail> _DbSet;
        protected DbSet<BlockingPlan> _blockingPlans;

        public BlockingPlanLogic(BookingOrderLogic bookingOrderLogic, BookingOrderDetailLogic bookingOrderDetailLogic, BlockingPlanWorkScheduleLogic blockingPlanWorkScheduleLogic, WeeklyPlanLogic weeklyPlanLogic, IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.BlockingPlanWorkScheduleLogic = blockingPlanWorkScheduleLogic;
            this.BookingOrderLogic = bookingOrderLogic;
            this.DbContext = dbContext;
            this.BookingOrderDetailLogic = bookingOrderDetailLogic;
            this.WeeklyPlanLogic = weeklyPlanLogic;
            _DbSet = dbContext.Set<BookingOrderDetail>();
            _blockingPlans = dbContext.Set<BlockingPlan>();
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

            if (blockingPlan.IsModified == true)
            {
                blockingPlan.BookingOrder = await DbContext
                                                 .BookingOrders.Include(bo => bo.DetailConfirms)
                                                 .IgnoreQueryFilters()
                                                 .FirstOrDefaultAsync(d => d.Id.Equals(blockingPlan.BookingOrderId));

                blockingPlan.BookingOrder.DetailConfirms = DbContext
                                                            .BookingOrderDetails
                                                            .Where(bod => bod.BookingOrderId == blockingPlan.BookingOrder.Id && bod.IsConfirmDelete == false)
                                                            .ToList();
            }
            else
            {
                blockingPlan.BookingOrder = await DbContext.BookingOrders.Include(d => d.DetailConfirms).FirstOrDefaultAsync(d => d.Id.Equals(blockingPlan.BookingOrderId));
            }

            return blockingPlan;
        }

        public override async void UpdateModel(int id, BlockingPlan model)
        {
            try
            {
                if (model.WorkSchedules != null)
                {

                    HashSet<int> detailIds = BlockingPlanWorkScheduleLogic.GetBlockingPlanWorkScheduleIds(id);

                    int countConfirmed = 0;

                    #region Looping Detail Blocking Plan Work Schedule Logic
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
                    #endregion

                    #region  Looping Item in Model WorkSchedules
                    foreach (BlockingPlanWorkSchedule item in model.WorkSchedules)
                    {
                        if (item.Id == 0)
                            BlockingPlanWorkScheduleLogic.CreateModel(item);

                    }
                    #endregion

                    #region Set Status Blocking Plan Sewing (FULL CONFIRMED, BOOKING, HALF_CONFIRMED)
                    if (countConfirmed == model.WorkSchedules.Count)
                    {
                        if (countConfirmed == 0)
                        {
                            model.Status = BlockingPlanStatus.BOOKING;
                        }
                        else
                        {
                             model.Status = BlockingPlanStatus.FULL_CONFIRMED;
                        }
                        
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

                    if (model.IsModified == false || model.IsModified == null)
                    {
                        model.IsModified = true;
                    }


                    //  #region UpdateBookingOrderDetailConfirm
                    //  foreach (var item in model.BookingOrder)
                    //  {
                    //      if (model.IsModified == false || model.IsModified == null)
                    //      {
                    //          model.IsModified = true;
                    //          base.UpdateModel(id, model);
                    //      }

                    ////      _blockingPlans.Add(item);
                    //  }

                    //  base.UpdateModel(id, model);
                    //  await DbContext.SaveChangesAsync();
                    //  #endregion

                    BookingOrderDetailLogic.UpdateBookingOrderDetailConfirm(model.BookingOrderId);
                    base.UpdateModel(id, model);
                }
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
        }

        public void UpdateModelStatus(int id, BlockingPlan modelBP, string status)
        {
            modelBP.Status = status;
            if (modelBP.IsModified == true)
            {
                modelBP.IsModified = false;
            }

            base.UpdateModel(id, modelBP);
        }

        public Task<bool> UpdateModelStatus(int bookingOrderId, string status)
        {
            try
            {
                bool hasBlockingPlan = false;

                // Check apakah Booking Order memiliki Blocking PLan 
                BlockingPlan modelBP = DbContext.BlockingPlans
                                       .Where(queryBP => queryBP.BookingOrderId == bookingOrderId)
                                       .FirstOrDefault();

                // Jika Ada return hasBlockingPlan = True  else return hasBlockingPlan = False
                if (modelBP != null)
                {
                    hasBlockingPlan = true;
                    modelBP.Status = status;
                    base.UpdateModel(bookingOrderId, modelBP);
                }
                return Task.FromResult(hasBlockingPlan);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception($"Pesan Error Sebagai Berikut : {Ex}");
            }

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
                //EntityExtension.FlagForDelete(item, IdentityService.Username, "masterplan-service");
                await BlockingPlanWorkScheduleLogic.DeleteModel(item.Id);
            }

            BookingOrder bookingOrder = await BookingOrderLogic.ReadModelById(model.BookingOrderId);
            BookingOrderLogic.UpdateModelBlockingPlanId(bookingOrder.Id, bookingOrder, null);
            
            EntityExtension.FlagForDelete(model, IdentityService.Username, "masterplan-service", true);
            DbSet.Update(model);

            await DbContext.SaveChangesAsync();
        }


    }
}
