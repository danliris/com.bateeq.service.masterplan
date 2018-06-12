using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class SectionProfile : Profile
    {
        public SectionProfile()
        {
            CreateMap<Section, SectionViewModel>()
                .ReverseMap();
        }
    }
}