using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using System.Linq.Dynamic.Core;
using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using System.Reflection;
using Com.Moonlay.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Com.Moonlay.NetCore.Lib.Service;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation
{
    public class WeeklyPlanLogic : IBusiness<WeeklyPlan, WeeklyPlanViewModel>
    {
        public string Username { get; set; }
        public string Token { get; set; }
        private MasterplanDbContext dbContext;
        private IServiceProvider serviceProvider;

        public WeeklyPlanLogic(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
        }

        public IQueryable<WeeklyPlan> ConfigureFilter(IQueryable<WeeklyPlan> Query, Dictionary<string, object> FilterDictionary)
        {
            if (FilterDictionary != null && !FilterDictionary.Count.Equals(0))
            {
                foreach (var f in FilterDictionary)
                {
                    string Key = f.Key;
                    object Value = f.Value;
                    string filterQuery = string.Concat(string.Empty, Key, " == @0");

                    Query = Query.Where(filterQuery, Value);
                }
            }
            return Query;
        }

        public IQueryable<WeeklyPlan> ConfigureOrder(IQueryable<WeeklyPlan> Query, Dictionary<string, string> OrderDictionary)
        {
            /* Default Order */
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("LastModifiedUtc", General.DESCENDING);

                Query = Query.OrderByDescending(b => b.LastModifiedUtc);
            }
            /* Custom Order */
            else
            {
                string Key = OrderDictionary.Keys.First();
                string OrderType = OrderDictionary[Key];
                string TransformKey = General.TransformOrderBy(Key);

                BindingFlags IgnoreCase = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;

                Query = OrderType.Equals(General.ASCENDING) ?
                    Query.OrderBy(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b)) :
                    Query.OrderByDescending(b => b.GetType().GetProperty(TransformKey, IgnoreCase).GetValue(b));
            }
            return Query;
        }

        public IQueryable<WeeklyPlan> ConfigureSearch(IQueryable<WeeklyPlan> Query, List<string> SearchAttributes, string Keyword)
        {
            if (Keyword != null)
            {
                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }
            return Query;
        }

        public async Task<int> CreateAsync(WeeklyPlan model)
        {
            using (var context = this.dbContext)
            {
                try
                {
                    EntityExtension.FlagForCreate(model, this.Username, "masterplan-service");
                    context.WeeklyPlans.Add(model);
                    await context.SaveChangesAsync();

                    return await Task.FromResult(201);
                }
                catch (Exception exception)
                {

                    return await Task.FromResult(500);
                }
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var context = this.dbContext)
            {
                try
                {
                    var model = await GetAsync(id);
                    EntityExtension.FlagForDelete(model, this.Username, "masterplan-service");
                    context.WeeklyPlans.Update(model);
                    return await context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    return await Task.FromResult(500);
                }
            }
        }

        public async Task<WeeklyPlan> GetAsync(int id)
        {
            using (var context = this.dbContext)
            {
                var model = await context.WeeklyPlans.FindAsync(id);

                return model;
            }
        }

        public bool IsExists(int id)
        {
            using (var context = this.dbContext)
            {
                var model = context.WeeklyPlans.Find(id);

                if (model != null)
                {
                    return true;
                }

                return false;
            }
        }

        public WeeklyPlan MapToModel(WeeklyPlanViewModel viewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WeeklyPlanViewModel, WeeklyPlan>();
            });

            IMapper mapper = config.CreateMapper();
            var model = mapper.Map<WeeklyPlan>(viewModel);
            return model;
        }

        public WeeklyPlanViewModel MapToViewModel(WeeklyPlan model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WeeklyPlan, WeeklyPlanViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var viewModel = mapper.Map<WeeklyPlanViewModel>(model);
            return viewModel;
        }

        public Tuple<List<WeeklyPlan>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            using (var context = dbContext)
            {
                IQueryable<WeeklyPlan> Query = context.WeeklyPlans;
                List<string> SearchAttributes = new List<string>() { "Year", "UnitCode", "UnitName" };
                Query = ConfigureSearch(Query, SearchAttributes, Keyword);

                Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
                Query = ConfigureFilter(Query, FilterDictionary);

                Query = Query.Select(field => new WeeklyPlan
                {
                    Year = field.Year,
                    UnitCode = field.UnitCode,
                    UnitName = field.UnitName
                });

                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
                Query = ConfigureOrder(Query, OrderDictionary);

                Pageable<WeeklyPlan> pageable = new Pageable<WeeklyPlan>(Query, Page - 1, Size);
                List<WeeklyPlan> Data = pageable.Data.ToList<WeeklyPlan>();
                int TotalData = pageable.TotalCount;

                return Tuple.Create(Data, TotalData, OrderDictionary, SearchAttributes);
            }
        }

        public async Task<int> UpdateAsync(int id, WeeklyPlan model)
        {
            using (var context = this.dbContext)
            {
                try
                {
                    EntityExtension.FlagForUpdate(model, this.Username, "masterplan-service");
                    context.WeeklyPlans.Update(model);
                    return await context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    return await Task.FromResult(500);
                }
            }
        }

        public void Validate(WeeklyPlan model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(model, serviceProvider, null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                throw new ServiceValidationExeption(validationContext, validationResults);
            }
        }

        public void Validate(WeeklyPlanViewModel viewModel)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel, serviceProvider, null);

            if (!Validator.TryValidateObject(viewModel, validationContext, validationResults, true))
            {
                throw new ServiceValidationExeption(validationContext, validationResults);
            }
        }
    }
}
