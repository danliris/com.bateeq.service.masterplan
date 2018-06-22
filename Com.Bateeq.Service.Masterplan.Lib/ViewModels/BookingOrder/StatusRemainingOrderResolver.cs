using System;
using AutoMapper;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class StatusRemainingOrderResolver : IValueResolver<Models.BookingOrder, BookingOrderViewModel, string>
    {
        public string Resolve(Models.BookingOrder source, BookingOrderViewModel destination, string destMember, ResolutionContext context)
        {
            int total = 0;
            foreach (Models.BookingOrderDetail item in source.DetailConfirms)
                total += item.Total;
            if (total != source.OrderQuantity)
            {
                if (source.DeliveryDate.LocalDateTime > DateTimeOffset.UtcNow.Date.AddDays(45))
                    return StatusRemainingOrderConst.ON_PROCESS;
                else
                    return StatusRemainingOrderConst.EXPIRED;
            }
            return StatusRemainingOrderConst.DASH;
        }
    }
}
