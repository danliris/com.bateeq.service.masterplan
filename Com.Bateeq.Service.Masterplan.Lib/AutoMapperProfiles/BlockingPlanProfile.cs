using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Resolvers.BlockingPlan;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BlockingPlan;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class BlockingPlanProfile : Profile
    {
        public BlockingPlanProfile()
        {
            CreateMap<BlockingPlan, BlockingPlanViewModel>()
                .ReverseMap();
        }
    }
}
