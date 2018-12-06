using Com.Moonlay.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Collections.Generic;
using Com.Bateeq.Service.Masterplan.Lib.Services.IdentityService;
using Com.Bateeq.Service.Masterplan.Lib.Models;

namespace Com.Bateeq.Service.Masterplan.Lib.Utils.BaseLogic
{
    public abstract class BaseLogic<TModel> : IBaseLogic<TModel>
        where TModel : StandardEntity, IValidatableObject
    {
        protected DbSet<TModel> DbSet;
        protected IIdentityService IdentityService;

        public BaseLogic(IIdentityService identityService, MasterplanDbContext dbContext)
        {
            this.DbSet = dbContext.Set<TModel>();
            this.IdentityService = identityService;
        }

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

        internal void UpdateModel(int id, BookingOrderDetail itemBOD)
        {
            throw new NotImplementedException();
        }
    }
}
