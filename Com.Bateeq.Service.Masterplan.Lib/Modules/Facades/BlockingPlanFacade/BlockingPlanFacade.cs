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

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade
{
    public class BlockingPlanFacade : IBlockingPlanFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<BlockingPlan> DbSet;
        private BlockingPlanLogic BlockingPlanLogic;
        private BookingOrderLogic BookingOrderLogic;

        public BlockingPlanFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BlockingPlan>();
            this.BlockingPlanLogic = serviceProvider.GetService<BlockingPlanLogic>();
            this.BookingOrderLogic = serviceProvider.GetService<BookingOrderLogic>();
        }

        public async Task<int> Create(BlockingPlan model)
        {
            BlockingPlanLogic.CreateModel(model);
            int created = await DbContext.SaveChangesAsync();
            BookingOrder bookingOrder = await BookingOrderLogic.ReadModelById(model.BookingOrderId);
            BookingOrderLogic.UpdateModelBlockingPlanId(bookingOrder.Id, bookingOrder, model.Id);
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
            BlockingPlanLogic.UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }
    }
}
