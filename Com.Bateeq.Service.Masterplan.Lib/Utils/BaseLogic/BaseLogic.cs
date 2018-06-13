using Com.Bateeq.Service.Masterplan.Lib.Services;
using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic
{
    public abstract class BaseLogic<TModel> : IBaseLogic<TModel>
        where TModel : StandardEntity, IValidatableObject
    {
        protected DbSet<TModel> DbSet;
        protected IIdentityService IdentityService;
        protected QueryHelper<TModel> QueryHelper;

        public BaseLogic(IServiceProvider serviceProvider, MasterplanDbContext dbContext)
        {
            this.DbSet = dbContext.Set<TModel>();
            this.IdentityService = serviceProvider.GetService<IIdentityService>();
            this.QueryHelper = new QueryHelper<TModel>();
        }

        public abstract ReadResponse<TModel> ReadModel(int page, int size, string order, List<string> select, string keyword, string filter);

        public virtual void CreateModel(TModel model)
        {
            EntityExtension.FlagForCreate(model, IdentityService.Username, "masterplan-service");
            DbSet.Add(model);
        }

        public virtual Task<TModel> ReadModelById(int id)
        {
            return DbSet.FirstOrDefaultAsync(d => d.Id.Equals(id) && d.IsDeleted.Equals(false));
        }

        public virtual void UpdateModel(int id, TModel model)
        {
            EntityExtension.FlagForUpdate(model, IdentityService.Username, "masterplan-service");
            DbSet.Update(model);
        }

        public virtual async Task DeleteModel(int id)
        {
            TModel model = await ReadModelById(id);
            EntityExtension.FlagForDelete(model, IdentityService.Username, "masterplan-service", true);
            DbSet.Update(model);
        }
    }
}
