using Com.Bateeq.Service.Masterplan.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using System.Linq.Dynamic.Core;
using Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Logics
{
    public class SectionLogic : BaseLogic<Section>
    {
        public SectionLogic(IIdentityService identityService, MasterplanDbContext dbContext) : base(identityService, dbContext)
        {
        }
    }
}
