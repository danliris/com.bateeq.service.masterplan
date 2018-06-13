using Com.Bateeq.Service.Masterplan.Lib.Services;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Implementation;
using Com.Bateeq.Service.Masterplan.Lib.Utils;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade
{
    public class BookingOrderFacade : IBookingOrderFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<BookingOrder> DbSet;
        private IIdentityService IdentityService;
        private BookingOrderLogic BookingOrderLogic;

        public BookingOrderFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BookingOrder>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
            this.BookingOrderLogic = serviceProvider.GetService<BookingOrderLogic>();
        }

        public ReadResponse<BookingOrder> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            return BookingOrderLogic.ReadModel(page, size, order, select, keyword, filter);
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
    }
}
