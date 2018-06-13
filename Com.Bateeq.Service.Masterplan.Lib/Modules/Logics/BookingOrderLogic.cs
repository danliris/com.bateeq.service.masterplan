using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Implementation
{
    public class BookingOrderLogic : BaseLogic<BookingOrder>
    {
        public BookingOrderLogic(IServiceProvider serviceProvider, MasterplanDbContext dbContext) : base(serviceProvider, dbContext)
        {
        }

        public override ReadResponse<BookingOrder> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter)
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
                    "Id", "Code", "BookingDate", "Buyer", "OrderQuantity", "DeliveryDate", "Remark"
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
                    Remark = field.Remark
                });

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<BookingOrder>.Order(query, orderDictionary);

            Pageable<BookingOrder> pageable = new Pageable<BookingOrder>(query, page - 1, size);
            List<BookingOrder> data = pageable.Data.ToList<BookingOrder>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<BookingOrder>(data, totalData, orderDictionary, selectedFields);
        }
    }
}
