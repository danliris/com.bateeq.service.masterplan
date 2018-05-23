using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.WebApi.Helpers;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Masterplan.Lib.Exceptions;
using AutoMapper;
using Com.Moonlay.Models;

namespace Com.Bateeq.Service.Masterplan.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/sections")]
    [Authorize]
    public class SectionController : Controller
    {
        private SectionFacade sectionFacade;
        private static readonly string ApiVersion = "1.0";

        public SectionController(SectionFacade sectionFacade)
        {
            this.sectionFacade = sectionFacade;
        }


        [HttpGet]
        public IActionResult Get(int Page = 1, int Size = 25, string Order = "{}", [Bind(Prefix = "Select[]")]List<string> Select = null, string Keyword = null, string Filter = "{}")
        {
            try
            {
                Tuple<List<Section>, int, Dictionary<string, string>, List<string>> Data = sectionFacade.ReadModel(Page, Size, Order, Select, Keyword, Filter);

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<Section, SectionViewModel>(Data.Item1, sectionFacade.MapToViewModel, Page, Size, Data.Item2, Data.Item1.Count, Data.Item3, Data.Item4);

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

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = await sectionFacade.ReadModelById(Id);

            if (model == null)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                return NotFound(Result);
            }

            try
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok<Section, SectionViewModel>(model, sectionFacade.MapToViewModel);
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

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute] int Id, [FromBody] SectionViewModel ViewModel)
        {
            try
            {
                sectionFacade.Validate(ViewModel);
                sectionFacade.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                sectionFacade.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");

                Section model = sectionFacade.MapToModel(ViewModel);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (Id != model.Id)
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                        .Fail();
                    return BadRequest(Result);
                }

                using (var transaction = this.sectionFacade.beginTransaction())
                {
                    try
                    {
                        await sectionFacade.UpdateModel(Id, model);
                        transaction.Commit();
                    }
                    catch (ServiceValidationExeption e)
                    {
                        transaction.Rollback();
                        throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                return NoContent();
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!sectionFacade.IsExists(Id))
                {
                    Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.NOT_FOUND_STATUS_CODE, General.NOT_FOUND_MESSAGE)
                    .Fail();
                    return NotFound(Result);
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                        .Fail();
                    return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SectionViewModel ViewModel)
        {
            try
            {
                sectionFacade.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");
                sectionFacade.Validate(ViewModel);
                sectionFacade.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<StandardEntity, StandardEntity>()
                       .Include<SectionViewModel, Section>();
                    cfg.CreateMap<SectionViewModel, Section>();
                });

                Section model = (Section)Mapper.Map(ViewModel, ViewModel.GetType(), typeof(Commodity));

                using (var transaction = sectionFacade.beginTransaction())
                {
                    try
                    {
                        await sectionFacade.CreateModel(model);
                        transaction.Commit();
                    }
                    catch (ServiceValidationExeption e)
                    {
                        transaction.Rollback();
                        throw new ServiceValidationExeption(e.ValidationContext, e.ValidationResults);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(String.Concat(HttpContext.Request.Path, "/", model.Id), Result);
            }
            catch (ServiceValidationExeption e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (var transaction = sectionFacade.beginTransaction())
                {
                    try
                    {
                        sectionFacade.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                        sectionFacade.Token = Request.Headers["Authorization"].First().Replace("Bearer ", "");
                        await sectionFacade.DeleteModel(Id);
                        transaction.Commit();
                    }
                    catch (DbReferenceNotNullException e)
                    {
                        transaction.Rollback();
                        throw new DbReferenceNotNullException(e.Message);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message, e.InnerException);
                    }
                }

                return NoContent();
            }
            catch (DbReferenceNotNullException e)
            {
                Dictionary<string, object> Result =
                    new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(Result);
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