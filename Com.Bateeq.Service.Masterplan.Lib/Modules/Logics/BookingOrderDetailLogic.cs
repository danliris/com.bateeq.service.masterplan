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
        private readonly MasterplanDbContext _dbContext;
        private readonly DbSet<BookingOrder> _bookingOrder;

        public BookingOrderDetailLogic(IIdentityService identityService, MasterplanDbContext dbContext, BookingOrderDetail bookingOrderDetail) : base(identityService, dbContext)
        {
            _dbContext = dbContext;
            BookingOrderDetail = bookingOrderDetail;
            _bookingOrder = _dbContext.Set<BookingOrder>();
        }

        public HashSet<int> GetBookingOrderDetailIds(int id)
        {

            return new HashSet<int>(DbSet
                                    .Where(queryBP => queryBP.BookingOrderId == id && queryBP.IsConfirmDelete == false)
                                    .Select(d => d.Id));

        }

        public async void UpdateBookingOrderDetailConfirm(int id)
        {
            try
            {
                //List<BookingOrderDetail> modelBOD = _dbContext
                //                                   .BookingOrderDetails
                //                                   .Where(Querybod => Querybod.BookingOrderId == id && Querybod.IsDeleted == false)
                //                                   .ToList();

                var bookingOrder =  await _bookingOrder.Where(item => item.IsDeleted == false && item.Id == id).FirstOrDefaultAsync();

                #region Looping Item on model Booking Order Detail
                foreach (var itemBOD in bookingOrder.DetailConfirms)
                {
                    if (itemBOD.IsConfirmDelete == true)
                    {
                        itemBOD.IsDeleted = true;
                    }
                    else
                    {
                        if (itemBOD.IsAddNew == true)
                        {
                            itemBOD.IsAddNew = false;
                        }
                    }
                }
                #endregion

                _bookingOrder.Update(bookingOrder);
            }
            catch (System.Exception Ex)
            {
                throw new System.Exception($"Terjadi kesalahan saat melakukan update ,Pesan Error Sebagai Berikut : {Ex}");
            }
        }
    }
}
