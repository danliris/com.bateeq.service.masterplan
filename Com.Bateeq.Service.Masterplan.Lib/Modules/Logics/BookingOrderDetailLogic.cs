using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderDetailLogic : BaseLogic<BookingOrderDetail>
    {
        private readonly MasterplanDbContext DbContext;

        public BookingOrderDetailLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
            DbContext = dbContext;
        }

        public HashSet<int> GetBookingOrderDetailIds(int id)
        {

            return new HashSet<int>(DbSet.Where(queryBP => queryBP.BookingOrderId == id).Select(d => d.Id));

        }

        public void UpdateBookingOrderDetailConfirm(int id)
        {
            var isAddNewDetail = false;
            try
            {

                List<BookingOrderDetail> modelBOD = DbContext.BookingOrderDetails
                                                             .Where(queryBP => queryBP.BookingOrderId == id).ToList();

                foreach (var item in modelBOD)
                {
                    if (item.isAddNew == true)
                    {
                        item.isAddNew = isAddNewDetail;
                        base.UpdateModel(id, item);
                    }
                }
            }

            catch (System.Exception Ex)
            {
                throw new System.Exception($"Pesan Error Sebagai Berikut : {Ex}");
            }
        }
    }
}
