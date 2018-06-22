using AutoMapper;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class StatusTotalConfirmResolver : IValueResolver<Models.BookingOrder, BookingOrderViewModel, string>
    {
        public string Resolve(Models.BookingOrder source, BookingOrderViewModel destination, string destMember, ResolutionContext context)
        {
            
            if (source.DetailConfirms.Count <= 0)
                return StatusTotalConfirmConst.NOT_CONFIRMED;
            else
            {
                int total = 0;
                foreach (Models.BookingOrderDetail item in source.DetailConfirms)
                {
                    total += item.Total;
                }
                int diff = total - source.OrderQuantity;
                if (diff > 0)
                    return "+" + diff;
                return diff.ToString();
            }
        }
    }
}
