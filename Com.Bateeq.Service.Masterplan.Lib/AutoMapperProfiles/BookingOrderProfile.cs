using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class BookingOrderProfile : Profile
    {
        public BookingOrderProfile()
        {
            CreateMap<BookingOrder, BookingOrderViewModel>()
                .ForPath(d => d.Section.Id, opt => opt.MapFrom(s => s.SectionId))
                .ForPath(d => d.Section.Name, opt => opt.MapFrom(s => s.SectionName))
                .ForPath(d => d.Buyer.Id, opt => opt.MapFrom(s => s.BuyerId))
                .ForPath(d => d.Buyer.Name, opt => opt.MapFrom(s => s.BuyerName))
                .ForMember(d => d.StatusRemainingOrder, opt => opt.ResolveUsing<StatusRemainingOrderResolver>())
                .ForMember(d => d.Status, opt => opt.ResolveUsing<StatusResolver>())
                .ForMember(d => d.StatusTotalConfirm, opt => opt.ResolveUsing<StatusTotalConfirmResolver>())
                .ReverseMap();
        }
    }
}
