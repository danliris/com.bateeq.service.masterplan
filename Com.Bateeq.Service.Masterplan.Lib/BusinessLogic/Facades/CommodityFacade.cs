using Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Implementation;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Masterplan.Lib.BusinessLogic.Facades
{
    public class CommodityFacade
    {
        private CommodityLogic commodityLogic;
        public string Username { get; set; }
        public string Token { get; set; }


        public CommodityFacade(IServiceProvider provider)
        {
            commodityLogic = new CommodityLogic(provider);
        }

        public Tuple<List<Commodity>, int, Dictionary<string, string>, List<string>> ReadModel(int Page, int Size, string Order, List<string> Select, string Keyword, string Filter)
        {
            return commodityLogic.ReadModel(Page, Size, Order, Select, Keyword, Filter);
        }

        public CommodityViewModel MapToViewModel(Commodity model)
        {
            return commodityLogic.MapToViewModel(model);
        }

        public Commodity MapToModel(CommodityViewModel viewModel)
        {
            return commodityLogic.MapToModel(viewModel);
        }

        public async Task<Commodity> ReadModelById(int id)
        {
            return await commodityLogic.GetAsync(id);
        }

        public void Validate(Commodity model)
        {
            commodityLogic.Validate(model);
        }

        public void Validate(CommodityViewModel viewModel)
        {
            commodityLogic.Validate(viewModel);
        }

        public IDbContextTransaction beginTransaction()
        {
            return commodityLogic.DbContext.Database.BeginTransaction();
        }

        public async Task<int> UpdateModel(int id, Commodity model)
        {
            commodityLogic.Username = Username;
            return await commodityLogic.UpdateAsync(id, model);
        }

        public bool IsExists(int Id)
        {
            return commodityLogic.IsExists(Id);
        }

        public async Task<int> CreateModel(Commodity model)
        {
            commodityLogic.Username = Username;
            return await commodityLogic.CreateAsync(model);
        }

        public async Task<int> DeleteModel(int id)
        {
            commodityLogic.Username = Username;
            return await commodityLogic.DeleteAsync(id);
        }
    }
}
