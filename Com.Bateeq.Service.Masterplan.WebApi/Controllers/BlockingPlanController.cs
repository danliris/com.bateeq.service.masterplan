using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.BlockingPlanFacade;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Services.ValidateService;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels.BlockingPlan;
using Com.Bateeq.Service.Masterplan.WebApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/blocking-plans")]
    [Authorize]
    public class BlockingPlanController : BaseController<BlockingPlan, BlockingPlanViewModel, IBlockingPlanFacade>
    {
        private static readonly string apiVersion = "1.0";
        public BlockingPlanController(IIdentityService identityService, IValidateService validateService, IBlockingPlanFacade sectionFacade, IMapper mapper) :base(identityService, validateService, sectionFacade, mapper, apiVersion)
        {
        }
    }
}
