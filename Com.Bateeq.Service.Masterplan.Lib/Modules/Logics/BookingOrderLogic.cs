using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderLogic : BaseLogic<BookingOrder>
    {
        private BookingOrderDetailLogic BookingOrderDetailLogic;
        public BookingOrderLogic(BookingOrderDetailLogic bookingOrderDetailLogic,IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.BookingOrderDetailLogic = bookingOrderDetailLogic;
        }

        public override void CreateModel(BookingOrder model)
        {
            foreach (BookingOrderDetail item in model.DetailConfirms)
            {
                EntityExtension.FlagForCreate(item, IdentityService.Username, "masterplan-service");
            }
            base.CreateModel(model);
        }

        public override Task<BookingOrder> ReadModelById(int id)
        {
            return DbSet.Include(d => d.DetailConfirms).FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public override async void UpdateModel(int id, BookingOrder model)
        {
            if (model.DetailConfirms != null)
            {
                HashSet<int> detailIds = BookingOrderDetailLogic.GetBookingOrderDetailIds(id);

                foreach (int detailId in detailIds)
                {
                    BookingOrderDetail bod = model.DetailConfirms.FirstOrDefault(prop => prop.Id.Equals(detailId));
                    if (bod == null)
                        await BookingOrderDetailLogic.DeleteModel(detailId);
                    else
                        BookingOrderDetailLogic.UpdateModel(detailId, bod);
                }

                foreach (BookingOrderDetail item in model.DetailConfirms)
                {
                    if (item.Id == 0)
                        BookingOrderDetailLogic.CreateModel(item);
                }
            }
            base.UpdateModel(id, model);
        }

        public override async Task DeleteModel(int id)
        {
            BookingOrder model = await ReadModelById(id);

            foreach (var item in model.DetailConfirms)
            {
                EntityExtension.FlagForDelete(item, IdentityService.Username, "masterplan-service");
            }

            EntityExtension.FlagForDelete(model, IdentityService.Username, "masterplan-service", true);
            DbSet.Update(model);
        }
    }
}
