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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Com.Moonlay.Models;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation
{
    public class SectionLogic : IBusiness<Section, SectionViewModel>
    {
        public string Username { get; set; }
        public string Token { get; set; }
        private DbSet<Section> dbSet;
        private MasterplanDbContext dbContext;
        private IServiceProvider provider;

        public SectionLogic(IServiceProvider provider, MasterplanDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Sections;
            this.provider = provider;
        }

        public IQueryable<Section> ConfigureFilter(IQueryable<Section> Query, Dictionary<string, object> FilterDictionary)
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

        public IQueryable<Section> ConfigureOrder(IQueryable<Section> Query, Dictionary<string, string> OrderDictionary)
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

        public IQueryable<Section> ConfigureSearch(IQueryable<Section> Query, List<string> SearchAttributes, string Keyword)
        {
            if (Keyword != null)
            {
                Query = Query.Where(General.BuildSearch(SearchAttributes), Keyword);
            }
            return Query;
        }

        public Section MapToModel(SectionViewModel viewModel)
        {
            Section model = new Section();
            PropertyCopier<SectionViewModel, Section>.Copy(viewModel, model);
            return model;
        }

        public SectionViewModel MapToViewModel(Section model)
        {
            SectionViewModel viewModel = new SectionViewModel();
            PropertyCopier<Section, SectionViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Tuple<List<Section>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            IQueryable<Section> Query = this.dbSet;

            List<string> SearchAttributes = new List<string>()
                {
                    "Code","Name"
                };
            Query = ConfigureSearch(Query, SearchAttributes, Keyword);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(Filter);
            Query = ConfigureFilter(Query, FilterDictionary);

            List<string> SelectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Remark"
                };
            Query = Query
                .Select(field => new Section
                {
                    Id = field.Id,
                    Code = field.Code,
                    Name = field.Name,
                    Remark = field.Remark
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Section> pageable = new Pageable<Section>(Query, Page - 1, Size);
            List<Section> Data = pageable.Data.ToList<Section>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public void Validate(Section model)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(model, this.provider, null);

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }

        public void Validate(SectionViewModel viewModel)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            ValidationContext validationContext = new ValidationContext(viewModel, this.provider, null);

            if (!Validator.TryValidateObject(viewModel, validationContext, validationResults, true))
                throw new ServiceValidationExeption(validationContext, validationResults);
        }

        public async Task<Section> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<int> UpdateAsync(int id, Section model)
        {
            EntityExtension.FlagForUpdate(model, this.Username, "masterplan-service");
            dbContext.Update(model);
            return await dbContext.SaveChangesAsync();
        }

        public bool IsExists(int Id)
        {
            var section = GetAsync(Id);
            var isExist = false;

            if (section != null)
            {
                isExist = true;
            }

            return isExist;
        }

        public async Task<int> CreateAsync(Section model)
        {
            EntityExtension.FlagForCreate(model, this.Username, "masterplan-service");
            dbContext.Add(model);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var model = await GetAsync(id);
            EntityExtension.FlagForDelete(model, this.Username, "masterplan-service", true);
            dbContext.Update(model);
            return await dbContext.SaveChangesAsync();
        }
    }
}
