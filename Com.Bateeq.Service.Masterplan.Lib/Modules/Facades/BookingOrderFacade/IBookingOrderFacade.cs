﻿using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BookingOrderFacade
{
    public interface IBookingOrderFacade : IBaseFacade<BookingOrder>
    {
        Task<int> DeleteDetail(int id);
        Task<int> SetRemainingOrderQuantity(BookStatusViewModel bookStatus);
    }
}
