using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderDetailLogic : BaseLogic<BookingOrderDetail>
    {
        private BookingOrderDetail BookingOrderDetail;
        private readonly MasterplanDbContext DbContext;

        public BookingOrderDetailLogic( IIdentityService identityService, MasterplanDbContext dbContext, BookingOrderDetail bookingOrderDetail) : base(identityService, dbContext)
        {
            DbContext = dbContext;
            BookingOrderDetail = bookingOrderDetail;
        }

        public HashSet<int> GetBookingOrderDetailIds(int id)
        {

            return new HashSet<int>(DbSet.Where(queryBP => queryBP.BookingOrderId == id).Select(d => d.Id));

        }

        public async void UpdateBookingOrderDetailConfirm(int id)
        {
            try
            {
                List<BookingOrderDetail> modelBOD = DbContext.BookingOrderDetails.Where(Querybod => Querybod.BookingOrderId == id).ToList();

                foreach (var itemBOD in modelBOD)
                {
                    if (itemBOD.IsAddNew == true)
                    {
                        if (itemBOD.IsDeleted == true)
                        {
                            itemBOD.IsConfirmDelete = false;
                        }
                        itemBOD.IsAddNew = false;
                        base.UpdateModel(id, itemBOD);
                    }
                }
              await DbContext.SaveChangesAsync();

            }

            catch (System.Exception Ex)
            {
                throw new System.Exception($"Pesan Error Sebagai Berikut : {Ex}");
            }
        }
    }
}
