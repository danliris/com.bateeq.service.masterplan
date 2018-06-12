using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades;
using Com.Bateeq.Service.Masterplan.WebApi.Helpers;
using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/weekly-plans")]
    [Authorize]
    public class WeeklyPlanController : BaseController<WeeklyPlan, WeeklyPlanViewModel, WeeklyplanFacade>
    {
        private readonly static string apiVersion = "1.0";
        public WeeklyPlanController(IIdentityService identityService, IValidateService validateService, WeeklyplanFacade weeklyplanFacade, IMapper mapper) : base(identityService, validateService, weeklyplanFacade, mapper, apiVersion)
        {
        }

        [HttpGet("is-exist/{year}/{code}")]
        public async Task<IActionResult> GetByYearAndUnitCode(int year, string code)
        {
            try
            {
                WeeklyPlan model = await Facade.GetByYearAndUnitCode(year, code);

                if (model == null)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                        .Fail();
                    return NotFound(Result);
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
    }

    public class FilterObject
    {
        public int Year { get; set; }
        public string Code { get; set; }
    }
}