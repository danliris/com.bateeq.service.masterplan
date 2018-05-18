using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/commodities")]
    [Authorize]
    public class CommodityController : BasicController<MasterplanDbContext, CommodityService, CommodityViewModel, Commodity>
    {
        private static readonly string ApiVersion = "1.0";

        public CommodityController(CommodityService service) : base(service, ApiVersion)
        {
        }
    }
}