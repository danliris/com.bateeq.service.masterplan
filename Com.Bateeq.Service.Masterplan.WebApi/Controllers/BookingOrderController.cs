using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades;
using Com.Bateeq.Service.Masterplan.WebApi.Helpers;
using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/booking-orders")]
    [Authorize]
    public class BookingOrderController : BaseController<BookingOrder, BookingOrderViewModel, IBookingOrderFacade>
    {
        private readonly static string apiVersion = "1.0";
        public BookingOrderController(IIdentityService identityService, IValidateService validateService, IBookingOrderFacade bookingOrderFacade, IMapper mapper) : base(identityService, validateService, bookingOrderFacade, mapper, apiVersion)
        {
        }
    }
}
