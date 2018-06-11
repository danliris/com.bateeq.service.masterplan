using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades;
using Com.Bateeq.Service.Masterplan.WebApi.Helpers;
using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Services;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/weekly-plans")]
    [Authorize]
    public class WeeklyPlanController : BaseController<WeeklyPlan, WeeklyPlanViewModel, WeeklyplanFacade>
    {
        private readonly static string apiVersion = "1.0";
        public WeeklyPlanController(IMapper mapper, IdentityService identityService, ValidateService validateService, WeeklyplanFacade weeklyplanFacade) : base(mapper, identityService, validateService, weeklyplanFacade, apiVersion)
        {
        }
    }
}