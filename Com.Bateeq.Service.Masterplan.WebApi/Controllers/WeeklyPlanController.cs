using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades;
using Com.Bateeq.Service.Masterplan.WebApi.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Services.ValidateService;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/weekly-plans")]
    [Authorize]
    public class WeeklyPlanController : BaseController<WeeklyPlan, WeeklyPlanViewModel, WeeklyPlanFacade>
    {
        private readonly static string apiVersion = "1.0";
        public WeeklyPlanController(IIdentityService identityService, IValidateService validateService, WeeklyPlanFacade weeklyplanFacade, IMapper mapper) : base(identityService, validateService, weeklyplanFacade, mapper, apiVersion)
        {
        }

        [HttpGet("is-exist/{year}/{code}")]
        public async Task<IActionResult> GetByYearAndUnitCode(string year, string code)
        {
            try
            {
                WeeklyPlan model = await Facade.GetByYearAndUnitCode(year, code);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return Ok(Result);
                }
                else
                {
                    WeeklyPlanViewModel viewModel = Mapper.Map<WeeklyPlanViewModel>(model);
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                        .Ok<WeeklyPlanViewModel>(Mapper, viewModel);
                    return Ok(Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("get-by-year/{year}")]
        public async Task<IActionResult> GetWeeklyPlanByYear(string year)
        {
            try
            {
                List<WeeklyPlan> read = await Facade.ReadByYear(year);

                List<WeeklyPlanViewModel> dataVM = this.Mapper.Map<List<WeeklyPlanViewModel>>(read);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<List<WeeklyPlanViewModel>>(this.Mapper, dataVM);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}