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
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade
{
    public class BookingOrderFacade : IBookingOrderFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<BookingOrder> DbSet;
        private BookingOrderLogic BookingOrderLogic;
        private BookingOrderDetailLogic BookingOrderDetailLogic;
        private BlockingPlanLogic BlockingPlanLogic;

        public BookingOrderFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BookingOrder>();
            this.BookingOrderLogic = serviceProvider.GetService<BookingOrderLogic>();
            this.BookingOrderDetailLogic = serviceProvider.GetService<BookingOrderDetailLogic>();
            this.BlockingPlanLogic = serviceProvider.GetService<BlockingPlanLogic>();
        }

        public ReadResponse<BookingOrder> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<BookingOrder> query = this.DbSet;

            List<string> searchAttributes = new List<string>()
                { 
                    "Code"
                };
            query = QueryHelper<BookingOrder>.Search(query, searchAttributes, keyword);

            // Filter not show Booking Order Expired and BlockingPlanId NULL to add Blocking Plan Sewing 
            if (filter.Contains("Expired"))
            {
                filter = "{}";
                var today = DateTime.Today;
                var expiredDate = today.AddDays(45);
                query = query.Where(queryBO => queryBO.DeliveryDate >= expiredDate && queryBO.BlockingPlanId == null);
            }

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<BookingOrder>.Filter(query, filterDictionary);

            try
            {
                List<string> selectedFields = new List<string>()
                {
                    "Id", "Code", "BookingDate", "Buyer", "OrderQuantity", "DeliveryDate", "Remark","DetailConfirms", "Status",
                    "StatusTotalConfirm", "StatusRemainingOrder", "BlockingPlanId","IsModified","CanceledBookingOrder",
                    "ExpiredBookingOrder","ExpiredDeletedDate"
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
                    DetailConfirms = new List<BookingOrderDetail>(field.DetailConfirms.Select(d => new BookingOrderDetail
                    {
                        Total = d.Total
                    })),
                    BlockingPlanId = field.BlockingPlanId,
                    IsModified = field.IsModified,
                    CanceledBookingOrder = field.CanceledBookingOrder,
                    ExpiredBookingOrder = field.ExpiredBookingOrder,
                    ExpiredDeletedDate = field.ExpiredDeletedDate

                });

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<BookingOrder>.Order(query, orderDictionary);

            Pageable<BookingOrder> pageable = new Pageable<BookingOrder>(query, page - 1, size);
            List<BookingOrder> data = pageable.Data.ToList<BookingOrder>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<BookingOrder>(data, totalData, orderDictionary, selectedFields);
            }
            catch (Exception Ex)
            {
                throw new Exception($"Terjadi Kesalahan Query Booking Order : {Ex.ToString()}");
            }

        }

        public async Task<int> Create(BookingOrder model)
        {
            int latestSN = this.DbSet
                        .Where(d => d.SectionId.Equals(model.SectionId) && d.BuyerId.Equals(model.BuyerId) && d.BookingDate.Year.Equals(model.BookingDate.Year))
                        .DefaultIfEmpty()
                        .Max(d => d.SerialNumber);
            model.SerialNumber = latestSN != 0 ? latestSN + 1 : 1;
            model.Code = String.Format("{0}-{1}-{2}{3:D5}", model.SectionCode, model.BuyerCode, model.BookingDate.Year % 100, model.SerialNumber);

            BookingOrderLogic.CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<BookingOrder> ReadById(int id)
        {
            return await BookingOrderLogic.ReadModelById(id);
        }

        public async Task<int> Update(int id, BookingOrder model)
        {
            Boolean hasBlockingPlan = false;
            try
            {
                // Before Update Check if Booking Order Have Blocking Plan ?
                hasBlockingPlan = await BlockingPlanLogic.UpdateModelStatus(id, BlockingPlanStatus.CHANGED);

                // IF hasBlockingPlan  = True
                if (hasBlockingPlan)
                {
                    model.IsModified = true;
                    
                    if (model.IsModified == true)
                    {

                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception($"Terjadi Kesalahan Update Booking Order, dengan Kesalahan Sebagai Berikut:  {Ex.ToString()}");
            }
            BookingOrderLogic.UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            await BookingOrderLogic.DeleteModel(id);
            var blockingPlan = await DbContext.BlockingPlans.FirstOrDefaultAsync(d => d.BookingOrderId.Equals(id) && d.IsDeleted.Equals(false));
            if (blockingPlan != null)
                BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.DELETED);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteDetail(int id)
        {
            await BookingOrderDetailLogic.DeleteModel(id);
            var bookingOrderDetail = await BookingOrderDetailLogic.ReadModelById(id);
            var bookingOrder = await BookingOrderLogic.ReadModelById(bookingOrderDetail.BookingOrderId);
            if (bookingOrder.BlockingPlanId != null)
            {
                var blockingPlan = await BlockingPlanLogic.ReadModelById((int)bookingOrder.BlockingPlanId);
                BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.CHANGED);
            }
            bookingOrder.canceledItem = bookingOrder.canceledItem + 1;
            BookingOrderLogic.UpdateModel(bookingOrder.Id, bookingOrder);

            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> SetRemainingOrderQuantity(BookStatusViewModel bookStatus)
        {
            BookingOrder model = await BookingOrderLogic.ReadModelById(bookStatus.BookingOrderId);
            int total = 0;

            foreach (BookingOrderDetail item in model.DetailConfirms)
            {
                total += item.Total;
            }

            if (model.InitialOrderQuantity == null)
            {
                model.InitialOrderQuantity = model.OrderQuantity;
            }

            var orderQuantity = model.OrderQuantity;
            var orderCanceledOrDeleted = Math.Abs(total - orderQuantity);
            var nowDateAndTime = DateTimeOffset.Now;
            model.OrderQuantity = total;
            var blockingPlan = await BlockingPlanLogic.searchByBookingOrderId(bookStatus.BookingOrderId);

            if (blockingPlan != null)
            {
                if (bookStatus.StatusBooking == StatusConst.CANCEL_REMAINING)
                {
                    if (total == 0)
                    {
                        BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.CANCELLED);
                    }
                    else
                    {
                        BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.CHANGED);
                        model.CanceledBookingOrder = orderCanceledOrDeleted;
                        model.CanceledDate = nowDateAndTime;
                    }
                }
                else if (bookStatus.StatusBooking == StatusConst.DELETE_REMAINING)
                {
                    DateTimeOffset expiredDate = new DateTimeOffset().ToLocalTime();
                    if (total == 0)
                    {
                        BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.EXPIRED);
                    }
                    else
                    {
                        BlockingPlanLogic.UpdateModelStatus(blockingPlan.Id, blockingPlan, BlockingPlanStatus.CHANGED);
                        model.ExpiredBookingOrder = orderCanceledOrDeleted;
                        model.ExpiredDeletedDate = nowDateAndTime;
                    }
                }
            }

            return await Update(bookStatus.BookingOrderId, model);
        }
    }
}
