using System;
using AutoMapper;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class StatusRemainingOrderResolver : IValueResolver<Models.BookingOrder, BookingOrderViewModel, string>
    {
        public string Resolve(Models.BookingOrder source, BookingOrderViewModel destination, string destMember, ResolutionContext context)
        {
            if (source.DeliveryDate.LocalDateTime > DateTimeOffset.UtcNow.Date.AddDays(45))
            {
                if (source.DetailConfirms.Count > 0)
                    return StatusRemainingOrderConst.DONE;
                else
                    return StatusRemainingOrderConst.ON_PROCESS;
            }
            else
                return StatusRemainingOrderConst.EXPIRED;
        }
    }
}
