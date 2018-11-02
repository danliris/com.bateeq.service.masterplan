using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderLogic : BaseLogic<BookingOrder>
    {
        private BookingOrderDetailLogic BookingOrderDetailLogic;
        private MasterplanDbContext DbContext;

        public BookingOrderLogic(BookingOrderDetailLogic bookingOrderDetailLogic, IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            this.BookingOrderDetailLogic = bookingOrderDetailLogic;
            DbContext = dbContext;
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
            return DbSet
                  .Include(d => d.DetailConfirms)
                  .FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public override async void UpdateModel(int id, BookingOrder modelBO)
        {
            bool isAddNew = false;

            // Check apakah Booking Order memiliki Blocking PLan 
            if (modelBO.DetailConfirms != null)
            {
                if (modelBO.IsModified == true)
                {
                    isAddNew = true;
                }
                HashSet<int> detailIds = BookingOrderDetailLogic.GetBookingOrderDetailIds(id);

                foreach (int detailId in detailIds)
                {
                    BookingOrderDetail modelbod = modelBO.DetailConfirms.FirstOrDefault(prop => prop.Id.Equals(detailId));
                    if (modelbod == null)
                    {
                        await BookingOrderDetailLogic.DeleteModel(detailId);
                    }  
                    else
                    {
                        if (modelbod.IsDeleted)
                        {
                            modelbod.IsConfirmDelete = true;
                            BookingOrderDetailLogic.UpdateModel(detailId, modelbod);
                        }
                        
                    }
                        
                }

                foreach (BookingOrderDetail bodItem in modelBO.DetailConfirms)
                {
                    if (bodItem.Id == 0)
                    {
                        bodItem.IsAddNew = isAddNew;
                        BookingOrderDetailLogic.CreateModel(bodItem);
                    }
                }
            }
            base.UpdateModel(id, modelBO);
        }

        public void UpdateModelBlockingPlanId(int id, BookingOrder model, int? blockingPlanId)
        {
            model.BlockingPlanId = blockingPlanId;
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
