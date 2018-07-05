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

        public ReadResponse<BlockingPlan> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BlockingPlan> query = this.DbSet;

            List<string> searchAttributes = new List<string>() { };
            query = QueryHelper<BlockingPlan>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<BlockingPlan>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "BookingOrder", "Status"
                };
            query = query
                .Select(field => new BlockingPlan
                {
                    Id = field.Id,
                    BookingOrder = DbContext.BookingOrders
                        .Where(d => d.Id.Equals(field.BookingOrderId))
                        .Select(d => new BookingOrder
                        {
                            Code = d.Code,
                            BookingDate = d.BookingDate,
                            BuyerId = d.BuyerId,
                            BuyerName = d.BuyerName,
                            OrderQuantity = d.OrderQuantity,
                            DeliveryDate = d.DeliveryDate,
                            Remark = d.Remark
                        })
                        .IgnoreQueryFilters()
                        .FirstOrDefault(),
                    Status = field.Status
                });

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<BlockingPlan>.Order(query, orderDictionary);

            Pageable<BlockingPlan> pageable = new Pageable<BlockingPlan>(query, page - 1, size);
            List<BlockingPlan> data = pageable.Data.ToList<BlockingPlan>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<BlockingPlan>(data, totalData, orderDictionary, selectedFields);
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
