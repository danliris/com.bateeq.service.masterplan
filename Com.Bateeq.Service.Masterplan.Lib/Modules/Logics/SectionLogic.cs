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

        public override ReadResponse<Section> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            IQueryable<Section> query = this.DbSet;

            List<string> searchAttributes = new List<string>()
                {
                    "Code","Name"
                };
            query = QueryHelper<Section>.Search(query, searchAttributes, keyword);

            Dictionary<string, object> filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<Section>.Filter(query, filterDictionary);

            List<string> selectedFields = new List<string>()
                {
                    "Id", "Code", "Name", "Remark"
                };
            query = query
                .Select(field => new Section
                {
                    Id = field.Id,
                    Code = field.Code,
                    Name = field.Name,
                    Remark = field.Remark
                });

            Dictionary<string, string> orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<Section>.Order(query, orderDictionary);

            Pageable<Section> pageable = new Pageable<Section>(query, page - 1, size);
            List<Section> data = pageable.Data.ToList<Section>();
            int totalData = pageable.TotalCount;

            return new ReadResponse<Section>(data, totalData, orderDictionary, selectedFields);
        }
    }
}
