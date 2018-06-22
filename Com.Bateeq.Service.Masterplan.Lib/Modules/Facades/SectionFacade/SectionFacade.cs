using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Com.Bateeq.Service.Masterplan.Lib.Utils;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Logics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.SectionFacade
{
    public class SectionFacade : ISectionFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<Section> DbSet;
        private SectionLogic SectionLogic;

        public SectionFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Sections;
            this.SectionLogic = serviceProvider.GetService<SectionLogic>();
        }
        
        public ReadResponse<Section> Read(int page, int size, string order, List<string> select, string keyword, string filter)
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

        public async Task<int> Create(Section model)
        {
            SectionLogic.CreateModel(model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<Section> ReadById(int id)
        {
            return await SectionLogic.ReadModelById(id);
        }

        public async Task<int> Update(int id, Section model)
        {
            SectionLogic.UpdateModel(id, model);
            return await DbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            await SectionLogic.DeleteModel(id);
            return await DbContext.SaveChangesAsync();
        }
    }
}
