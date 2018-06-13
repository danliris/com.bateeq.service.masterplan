using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;
using Com.Bateeq.Service.Masterplan.Test.Controller.Utils;
using Com.Bateeq.Service.Masterplan.WebApi.Controllers;

namespace Com.Bateeq.Service.Masterplan.Test.Controller.Modules
{
    public class BookingOrderControllerTest : BaseControllerTest<BookingOrderController, BookingOrder, BookingOrderViewModel, IBookingOrderFacade>
    {
    }
}
