using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderDetailLogic : BaseLogic<BookingOrderDetail>
    {
        public BookingOrderDetailLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {   
        }

        public HashSet<int> GetBookingOrderDetailIds(int id)
        {

         return new HashSet<int>(DbSet.Where(queryBP => queryBP.BookingOrderId == id).Select(d => d.Id));

        }
    }
}
