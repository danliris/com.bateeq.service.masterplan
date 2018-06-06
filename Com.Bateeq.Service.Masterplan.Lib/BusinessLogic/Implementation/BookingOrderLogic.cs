using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation
{
    public class BookingOrderLogic : BaseLogic<BookingOrder>
    {
        public BookingOrderLogic(IServiceProvider serviceProvider, MasterplanDbContext dbContext) : base(serviceProvider, dbContext)
        {
        }

        public override Tuple<List<BookingOrder>, int, Dictionary<string, string>, List<string>> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BookingOrder> Query = this.DbSet;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code"
                };
            Query = QueryHelper.Search(Query, SearchAttributes, keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper.Filter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "BookingDate", "Buyer", "OrderQuantity", "DeliveryDate", "Remark"
                };
            Query = Query
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

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper.Order(Query, OrderDictionary);

            Pageable<BookingOrder> pageable = new Pageable<BookingOrder>(Query, page - 1, size);
            List<BookingOrder> Data = pageable.Data.ToList<BookingOrder>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }
    }
}
