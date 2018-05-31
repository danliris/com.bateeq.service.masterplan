using AutoMapper;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.WebApi.Helpers
{
    public class BaseController : Controller
    {
        protected readonly IMapper mapper;
        protected readonly IdentityService identityService;
        protected readonly ValidateService validateService;
        public BaseController(IMapper mapper, IdentityService identityService, ValidateService validateService)
        {
            this.mapper = mapper;
            this.identityService = identityService;
            this.validateService = validateService;
        }
    }
}
