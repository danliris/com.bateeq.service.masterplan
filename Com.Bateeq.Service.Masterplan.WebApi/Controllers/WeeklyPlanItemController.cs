using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Services.ValidateService;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Bateeq.Service.Masterplan.WebApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/weekly-plan-items")]
    [Authorize]
    public class WeeklyPlanItemController : Controller
    {
        protected IIdentityService IdentityService;
        protected readonly IValidateService ValidateService;
        protected readonly WeeklyPlanItemFacade Facade;
        protected readonly IMapper Mapper;
        protected readonly string ApiVersion = "1.0";

        public WeeklyPlanItemController(IIdentityService identityService, IValidateService validateService, WeeklyPlanItemFacade facade, IMapper mapper)
        {
            this.IdentityService = identityService;
            this.ValidateService = validateService;
            this.Facade = facade;
            this.Mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            try
            {
                ReadResponse<WeeklyPlanItem> read = Facade.Read(page, size, order, select, keyword, filter);

                List<WeeklyPlanItemViewModel> dataVM = this.Mapper.Map<List<WeeklyPlanItemViewModel>>(read.Data);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utils.General.OK_STATUS_CODE, Utils.General.OK_MESSAGE)
                    .Ok<WeeklyPlanItemViewModel>(this.Mapper, dataVM, page, size, read.Count, dataVM.Count, read.Order, read.Selected);
                return Ok(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, Utils.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(Utils.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }
    }
}
