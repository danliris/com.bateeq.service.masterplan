using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Logics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Moonlay.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade
{
    public class BlockingPlanFacade : BaseLogic<BlockingPlan>, IBlockingPlanFacade
    {
        private readonly MasterplanDbContext DbContext;
     //   private readonly DbSet<BlockingPlan> _DbSet;
        private readonly DbSet<BookingOrder> _BookingOrderDbSet;
        private readonly DbSet<BlockingPlanWorkSchedule> _BPWorkSchedulesDbSet;
        private readonly DbSet<WeeklyPlanItem> _WeeklyPlanItemDbSet;
        private readonly BlockingPlanLogic BlockingPlanLogic;
        private readonly BookingOrderLogic BookingOrderLogic;
        private readonly WeeklyPlanLogic _weeklyPlanLogic;

        public BlockingPlanFacade(IServiceProvider serviceProvider, IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BlockingPlan>();
            this._BookingOrderDbSet = this.DbContext.Set<BookingOrder>();
            this._WeeklyPlanItemDbSet = this.DbContext.Set<WeeklyPlanItem>();
           this._BPWorkSchedulesDbSet = this.DbContext.Set<BlockingPlanWorkSchedule>();
            this.BlockingPlanLogic = serviceProvider.GetService<BlockingPlanLogic>();
            this.BookingOrderLogic = serviceProvider.GetService<BookingOrderLogic>();
            this._weeklyPlanLogic = serviceProvider.GetService<WeeklyPlanLogic>();
           
        }

        public async Task<int> Create(BlockingPlan model)
        {
            BlockingPlanLogic.CreateModel(model);
            int created = await DbContext.SaveChangesAsync();
            BookingOrder bookingOrder = await BookingOrderLogic.ReadModelById(model.BookingOrderId);
            BookingOrderLogic.UpdateModelBlockingPlanId(bookingOrder.Id, bookingOrder, model.Id);
            
            foreach(var workschedule in model.WorkSchedules)
            {
                await _weeklyPlanLogic.UpdateByWeeklyplanItemByIdAndWeekId(workschedule);
            }
            
            await DbContext.SaveChangesAsync();

            return created;
        }

        public async Task<int> Delete(int id)
        {
            await BlockingPlanLogic.DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public ReadResponse<BlockingPlan> Read(int page, int size, string orderBY, List<string> select, string keyword, string filter)
         {
            IQueryable<BlockingPlan> queryBP = this.DbSet;          

            List<string> searchAttributes = new List<string>() { };
            queryBP = QueryHelper<BlockingPlan>.Search(queryBP, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            queryBP = QueryHelper<BlockingPlan>.Filter(queryBP, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "BookingOrder", "Status"
                };
            queryBP = queryBP
                .Select(bp => new BlockingPlan{
                        Id = bp.Id,BookingOrder = DbContext.BookingOrders
                        .Where(d => d.Id.Equals(bp.BookingOrderId))
                        .Select(bo => new BookingOrder
                        {
                            Code = bo.Code,
                            BookingDate = bo.BookingDate,
                            BuyerId = bo.BuyerId,
                            BuyerName = bo.BuyerName,
                            OrderQuantity = bo.OrderQuantity,
                            DeliveryDate = bo.DeliveryDate,
                            Remark = bo.Remark,
                            IsModified = bo.IsModified,
                            CanceledBookingOrder = bo.CanceledBookingOrder,
                            CanceledDate = bo.CanceledDate,
                            ExpiredBookingOrder = bo.ExpiredBookingOrder,
                            ExpiredDeletedDate = bo.ExpiredDeletedDate   
                        }).IgnoreQueryFilters()
                        .FirstOrDefault(),
                        Status = bp.Status,
                });

            Dictionary<string, string> orderBYDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(orderBY);
            queryBP = QueryHelper<BlockingPlan>.Order(queryBP, orderBYDictionary);

            Pageable<BlockingPlan> pageable = new Pageable<BlockingPlan>(queryBP, page - 1, size);
            List<BlockingPlan> data = pageable.Data.ToList<BlockingPlan>();
            
            
            int totalData = pageable.TotalCount;

            return new ReadResponse<BlockingPlan>(data, totalData, orderBYDictionary, selectedFields);
            
        }

        public async Task<BlockingPlan> ReadById(int id)
        {
            return await BlockingPlanLogic.ReadModelById(id);
        }

        public async Task<int> Update(int id, BlockingPlan model)
        {
            //BlockingPlanLogic.UpdateModel(id, model);

            #region BookingOrderUpdate
            EntityExtension.FlagForUpdate(model.BookingOrder, IdentityService.Username, "masterplan-service");
            _BookingOrderDbSet.Update(model.BookingOrder);
            #endregion

            List<int> itemIds = await _BPWorkSchedulesDbSet
                                      .Where(w => w.BlockingPlanId.Equals(id) && !w.IsDeleted)
                                      .Select(s => s.Id).ToListAsync();
            int confirmedItems = 0;
            foreach (var itemId in itemIds)
            {
                var item = model.WorkSchedules.FirstOrDefault(f => f.Id.Equals(itemId));
                if (item == null)
                {
                    var itemToDelete = await _BPWorkSchedulesDbSet
                                             .FirstOrDefaultAsync(f => f.Id.Equals(itemId));
                    EntityExtension.FlagForDelete(itemToDelete, IdentityService.Username, "masterplan-service");
                    _BPWorkSchedulesDbSet.Update(itemToDelete);
                }
                else
                {
                    if (item.isConfirmed)
                        confirmedItems ++;
                    EntityExtension.FlagForUpdate(item, IdentityService.Username, "masterplan-service");
                    _BPWorkSchedulesDbSet.Update(item);
                }
            }

            foreach (var item in model.WorkSchedules)
            {
                if (item.Id <= 0)
                {
                    EntityExtension.FlagForCreate(item, IdentityService.Username, "masterplan-service");
                    _BPWorkSchedulesDbSet.Add(item);
                }
            }

            if (confirmedItems == model.WorkSchedules.Count)
            {
                if (confirmedItems == 0)
                {
                    model.Status = BlockingPlanStatus.BOOKING;
                }
                else
                {
                    model.Status = BlockingPlanStatus.FULL_CONFIRMED;
                }

            }
            else if (confirmedItems == 0)
            {
                model.Status = BlockingPlanStatus.BOOKING;
            }
            else
            {
                model.Status = BlockingPlanStatus.HALF_CONFIRMED;
            }

            if (model.IsModified == false || model.IsModified == null)
            {
                model.IsModified = true;
            }
            #region WorkScheduleUdate
            foreach (var workschedule in model.WorkSchedules)
            {
                await _weeklyPlanLogic.UpdateByWeeklyplanItemByIdAndWeekId(workschedule);
            }
            #endregion

            #region BlockingPlanUpdate
            EntityExtension.FlagForUpdate(model, IdentityService.Username, "masterplan-service");
            DbSet.Update(model);
            #endregion


            return await DbContext.SaveChangesAsync();
        }
    }
}
