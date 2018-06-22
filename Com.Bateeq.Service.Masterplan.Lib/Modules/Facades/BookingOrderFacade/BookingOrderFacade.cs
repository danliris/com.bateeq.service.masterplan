using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Logics;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade
{
    public class BookingOrderFacade : IBookingOrderFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<BookingOrder> DbSet;
        private BookingOrderLogic BookingOrderLogic;
        private BookingOrderDetailLogic BookingOrderDetailLogic;

        public BookingOrderFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BookingOrder>();
            this.BookingOrderLogic = serviceProvider.GetService<BookingOrderLogic>();
            this.BookingOrderDetailLogic = serviceProvider.GetService<BookingOrderDetailLogic>();
        }

        public ReadResponse<BookingOrder> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BookingOrder> query = this.DbSet;

            List<string> searchAttributes = new List<string>()
                {
                    "Code"
                };
            query = QueryHelper<BookingOrder>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<BookingOrder>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "Code", "BookingDate", "Buyer", "OrderQuantity", "DeliveryDate", "Remark", "DetailConfirms", "Status", "StatusTotalConfirm", "StatusRemainingOrder"
                };
            query = query
                .Select(field => new BookingOrder
                {
                    Id = field.Id,
                    Code = field.Code,
                    BookingDate = field.BookingDate,
                    BuyerId = field.BuyerId,
                    BuyerName = field.BuyerName,
                    OrderQuantity = field.OrderQuantity,
                    DeliveryDate = field.DeliveryDate,
                    Remark = field.Remark,
                    DetailConfirms = new List<BookingOrderDetail>(field.DetailConfirms.Select(d => new BookingOrderDetail {
                        Total = d.Total
                    }))
                });

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<BookingOrder>.Order(query, orderDictionary);

            Pageable<BookingOrder> pageable = new Pageable<BookingOrder>(query, page - 1, size);
            List<BookingOrder> data = pageable.Data.ToList<BookingOrder>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<BookingOrder>(data, totalData, orderDictionary, selectedFields);
        }

        public async Task<int> Create(BookingOrder model)
        {
            int latestSN = this.DbSet
                        .Where(d => d.SectionId.Equals(model.SectionId) && d.BuyerId.Equals(model.BuyerId) && d.BookingDate.Year.Equals(model.BookingDate.Year))
                        .DefaultIfEmpty()
                        .Max(d => d.SerialNumber);
            model.SerialNumber = latestSN != 0 ? latestSN + 1 : 1;
            model.Code = String.Format("{0}-{1}-{2:D2}{3}", model.SectionCode, model.BuyerCode, model.BookingDate.Year, model.SerialNumber);

            BookingOrderLogic.CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<BookingOrder> ReadById(int id)
        {
            return await BookingOrderLogic.ReadModelById(id);
        }
        
        public async Task<int> Update(int id, BookingOrder model)
        {
            BookingOrderLogic.UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            await BookingOrderLogic.DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteDetail(int id)
        {
            await BookingOrderDetailLogic.DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> CancelRemaining(int id)
        {
            BookingOrder model = await BookingOrderLogic.ReadModelById(id);
            int total = 0;
            foreach (BookingOrderDetail item in model.DetailConfirms)
                total += item.Total;
            model.InitialOrderQuantity = model.OrderQuantity;
            model.OrderQuantity = total;
            return await Update(id, model);
        }
    }
}
