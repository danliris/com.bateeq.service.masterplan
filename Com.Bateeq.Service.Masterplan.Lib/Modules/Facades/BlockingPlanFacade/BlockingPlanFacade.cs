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

        public BlockingPlanFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BlockingPlan>();
            this.BlockingPlanLogic = serviceProvider.GetService<BlockingPlanLogic>();
        }

        public async Task<int> Create(BlockingPlan model)
        {
            BlockingPlanLogic.CreateModel(model);
            return await DbContext.SaveChangesAsync();
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
                    "Id", "BookingOrder"
                };
            query = query
                .Select(field => new BlockingPlan
                {
                    Id = field.Id,
                    BookingOrder = new BookingOrder
                    {
                        Code = field.BookingOrder.Code,
                        BookingDate = field.BookingOrder.BookingDate,
                        BuyerId = field.BookingOrder.BuyerId,
                        BuyerName = field.BookingOrder.BuyerName,
                        OrderQuantity = field.BookingOrder.OrderQuantity,
                        DeliveryDate = field.BookingOrder.DeliveryDate,
                        Remark = field.BookingOrder.Remark
                    }
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
