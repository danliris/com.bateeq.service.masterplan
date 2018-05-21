using Com.Bateeq.Service.Masterplan.Lib.Helpers;
using Com.Bateeq.Service.Masterplan.Lib.Interfaces;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Bateeq.Service.Masterplan.Lib.ViewModels;
using Com.Moonlay.NetCore.Lib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.Bateeq.Service.Masterplan.Lib.Services
{
    public class CommodityService : BasicService<MasterplanDbContext, Commodity>, IMap<Commodity, CommodityViewModel>
    {
        public CommodityService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        public override Tuple<List<Commodity>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", List<string> Select = null, string Keyword = null, string Filter = "{}")
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
                .Select(b => new Commodity
                {
                    Id = b.Id,
                    Code = b.Code,
                    Name = b.Name
                });

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = ConfigureOrder(Query, OrderDictionary);

            Pageable<Commodity> pageable = new Pageable<Commodity>(Query, Page - 1, Size);
            List<Commodity> Data = pageable.Data.ToList<Commodity>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary, SelectedFields);
        }

        public override void OnCreating(Commodity model)
        {
            base.OnCreating(model);
        }

        public CommodityViewModel MapToViewModel(Commodity model)
        {
            CommodityViewModel viewModel = new CommodityViewModel();
            PropertyCopier<Commodity, CommodityViewModel>.Copy(model, viewModel);
            return viewModel;
        }

        public Commodity MapToModel(CommodityViewModel viewModel)
        {
            Commodity model = new Commodity();
            PropertyCopier<CommodityViewModel, Commodity>.Copy(viewModel, model);
            return model;
        }
    }
}
