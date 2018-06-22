using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BookingOrder;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class BookingOrderDetailProfile : Profile
    {
        public BookingOrderDetailProfile()
        {
            CreateMap<BookingOrderDetail, BookingOrderDetailViewModel>()
                .ReverseMap();
        }
    }
}
