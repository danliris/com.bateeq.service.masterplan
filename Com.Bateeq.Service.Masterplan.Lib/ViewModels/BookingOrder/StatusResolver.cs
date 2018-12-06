﻿using AutoMapper;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder
{
    public class StatusResolver : IValueResolver<Models.BookingOrder, BookingOrderViewModel, string>
    {
        public string Resolve(Models.BookingOrder BOsource, BookingOrderViewModel destination, string destMember, ResolutionContext context)
        {
            try
            {
                if (BOsource.BlockingPlanId != null)
                    return StatusConst.BLOCKING_PLAN_IS_CREATED;
                else if (BOsource.DetailConfirms.Count <= 0)
                    return StatusConst.BOOKING;
                else
                    return StatusConst.CONFIRMED;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
