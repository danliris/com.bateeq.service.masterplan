using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.Moonlay.NetCore.Lib.Service;
using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using System.Reflection;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation
{
    public class CommodityLogic : StandardEntityService<MasterplanDbContext, Commodity> , IBusiness<Commodity, CommodityViewModel>
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public CommodityLogic(IServiceProvider provider) : base(provider)
        {
        }

        public IQueryable<Commodity> ConfigureFilter(IQueryable<Commodity> Query, Dictionary<string, object> FilterDictionary)
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

        public IQueryable<Commodity> ConfigureOrder(IQueryable<Commodity> Query, Dictionary<string, string> OrderDictionary)
        {
            /* Default Order */
            if (OrderDictionary.Count.Equals(0))
            {
                OrderDictionary.Add("_LastModifiedUtc", General.DESCENDING);

                Query = Query.OrderByDescending(b => b._LastModifiedUtc);
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

        public IQueryable<Commodity> ConfigureSearch(IQueryable<Commodity> Query, List<string> SearchAttributes, string Keyword)
        {
            if (Keyword != null)
            {
                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }
            return Query;
        }

        public Commodity MapToModel(CommodityViewModel viewModel)
        {
            Commodity model = new Commodity();
            PropertyCopier<CommodityViewModel, Commodity>.Copy(viewModel, model);
            return model;
        }

        public CommodityViewModel MapToViewModel(Commodity model)
        {
            CommodityViewModel viewModel = new CommodityViewModel();
            PropertyCopier<Commodity, CommodityViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Tuple<List<Commodity>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            IQueryable<Commodity> Query = this.DbContext.Commodities;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code","Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name"
                };
            Query = Query
                .Select(field => new Commodity
                {
                    Id = field.Id,
                    Code = field.Code,
                    Name = field.Name
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Commodity> pageable = new Pageable<Commodity>(Query, Page - 1, Size);
            List<Commodity> Data = pageable.Data.ToList<Commodity>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public void Validate(Commodity model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, this.ServiceProvider, null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }

        public void Validate(CommodityViewModel viewModel)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(viewModel, this.ServiceProvider, null);

            if (!Validator.TryValidateObject(viewModel, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }

        public override void OnCreating(Commodity model)
        {
            base.OnCreating(model);
            model._CreatedAgent = "masterplan-service";
            model._CreatedBy = this.Username;
            model._LastModifiedAgent = "masterplan-service";
            model._LastModifiedBy = this.Username;
        }

        public override void OnUpdating(int id, Commodity model)
        {
            base.OnUpdating(id, model);
            model._LastModifiedAgent = "masterplan-service";
            model._LastModifiedBy = this.Username;
        }

        public override void OnDeleting(Commodity model)
        {
            base.OnDeleting(model);
            model._DeletedAgent = "masterplan-service";
            model._DeletedBy = this.Username;
        }
    }
}
