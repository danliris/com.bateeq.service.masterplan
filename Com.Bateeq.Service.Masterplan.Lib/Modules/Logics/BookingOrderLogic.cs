using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class BookingOrderLogic : BaseLogic<BookingOrder>
    {
        public BookingOrderLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
        }
    }
}
