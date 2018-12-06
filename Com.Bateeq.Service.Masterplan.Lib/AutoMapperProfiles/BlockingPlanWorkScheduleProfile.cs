using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BlockingPlan;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class BlockingPlanWorkScheduleProfile : Profile
    {
        public BlockingPlanWorkScheduleProfile()
        {
            CreateMap<BlockingPlanWorkSchedule, BlockingPlanWorkScheduleViewModel>()
                .ForPath(d => d.Unit._id, opt => opt.MapFrom(s => s.UnitId))
                .ForPath(d => d.Unit.code, opt => opt.MapFrom(s => s.UnitText))
                .ForPath(d => d.Year.Id, opt => opt.MapFrom(s => s.YearId))
                .ForPath(d => d.Year.Year, opt => opt.MapFrom(s => s.YearText))
                .ForPath(d => d.Week.Id, opt => opt.MapFrom(s => s.WeekId))
                .ForPath(d => d.Week.WeekText, opt => opt.MapFrom(s => s.WeekText))
                .ReverseMap();
        }
    }
}
