using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade
{
    public interface IBookingOrderFacade : IBaseFacade<BookingOrder>
    {
        Task<int> DeleteDetail(int id);
        Task<int> CancelRemaining(int id);
    }
}
