using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades
{
    public class SectionFacade
    {
        private SectionLogic sectionLogic;
        public string Username { get; set; }
        public string Token { get; set; }

        public SectionFacade(IServiceProvider provider)
        {
            sectionLogic = new SectionLogic(provider);
        }

        public Tuple<List<Section>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            return sectionLogic.ReadModel(Page, Size, Order, Select, Keyword, Filter);
        }

        public SectionViewModel MapToViewModel(Section model)
        {
            return sectionLogic.MapToViewModel(model);
        }

        public Section MapToModel(SectionViewModel viewModel)
        {
            return sectionLogic.MapToModel(viewModel);
        }

        public async Task<Section> ReadModelById(int id)
        {
            return await sectionLogic.GetAsync(id);
        }

        public void Validate(Section model)
        {
            sectionLogic.Validate(model);
        }

        public void Validate(SectionViewModel viewModel)
        {
            sectionLogic.Validate(viewModel);
        }

        public IDbContextTransaction beginTransaction()
        {
            return sectionLogic.DbContext.Database.BeginTransaction();
        }

        public async Task<int> UpdateModel(int id, Section model)
        {
            sectionLogic.Username = Username;
            return await sectionLogic.UpdateAsync(id, model);
        }

        public bool IsExists(int Id)
        {
            return sectionLogic.IsExists(Id);
        }

        public async Task<int> CreateModel(Section model)
        {
            sectionLogic.Username = Username;
            return await sectionLogic.CreateAsync(model);
        }

        public async Task<int> DeleteModel(int id)
        {
            sectionLogic.Username = Username;
            return await sectionLogic.DeleteAsync(id);
        }
    }
}
