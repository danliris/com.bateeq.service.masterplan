using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.AutoMapperProfiles
{
    public class WeeklyPlanProfile : Profile
    {
        public WeeklyPlanProfile()
        {
            CreateMap<WeeklyPlan, WeeklyPlanViewModel>()
                .ReverseMap();
            CreateMap<WeeklyPlanItem, WeeklyPlanItemViewModel>()
                .ReverseMap();
        }
    }
}
