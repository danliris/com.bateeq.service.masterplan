using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.SectionFacade;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Bateeq.Service.Masterplan.Test.Controller.Utils;
using Com.Bateeq.Service.Masterplan.WebApi.Controllers;

namespace Com.Bateeq.Service.Masterplan.Test.WebApi.Controllers
{
    public class SectionControllerTest : BaseControllerTest<SectionController, Section, SectionViewModel, ISectionFacade>
    {
    }
}
