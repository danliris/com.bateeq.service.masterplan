using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Masterplan.Lib.Services;
using Com.Bateeq.Service.Masterplan.Lib.Modules.Implementation;
using Com.Bateeq.Service.Masterplan.Lib.Utils;

namespace Com.Bateeq.Service.Masterplan.Lib.Modules.Facades.SectionFacade
{
    public class SectionFacade : ISectionFacade
    {
        private readonly MasterplanDbContext DbContext;
        private readonly DbSet<BookingOrder> DbSet;
        private IIdentityService IdentityService;
        private SectionLogic SectionLogic;

        public SectionFacade(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = this.DbContext.Set<BookingOrder>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
            this.SectionLogic = serviceProvider.GetService<SectionLogic>();
        }
        
        public ReadResponse<Section> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            return SectionLogic.ReadModel(page, size, order, select, keyword, filter);
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
