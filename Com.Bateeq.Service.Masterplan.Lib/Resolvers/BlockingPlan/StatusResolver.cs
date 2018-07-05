using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BlockingPlan;
using System;

namespace Com.Bateeq.Service.Masterplan.Lib.Resolvers.BlockingPlan
{
    public class StatusResolver : IValueResolver<Models.BlockingPlan, BlockingPlanViewModel, string>
    {
        public string Resolve(Models.BlockingPlan source, BlockingPlanViewModel destination, string destMember, ResolutionContext context)
        {
            if (source.BookingOrder != null)
            {
                if (source.BookingOrder.DeliveryDate.LocalDateTime <= DateTimeOffset.UtcNow.Date.AddDays(45))
                    return BlockingPlanStatus.EXPIRED;
            }
            return source.Status;
        }
    }
}
