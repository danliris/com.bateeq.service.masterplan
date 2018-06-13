using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.SectionFacade;
using Com.Bateeq.Service.Masterplan.WebApi.Utils;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/sections")]
    [Authorize]
    public class SectionController : BaseController<Section, SectionViewModel, ISectionFacade>
    {
        private static readonly string apiVersion = "1.0";
        public SectionController(IIdentityService identityService, IValidateService validateService, ISectionFacade sectionFacade, IMapper mapper) : base(identityService, validateService, sectionFacade, mapper, apiVersion)
        {
        }
    }
}