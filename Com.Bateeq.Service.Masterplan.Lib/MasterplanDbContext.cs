using Com.Bateeq.Service.Masterplan.Lib.ModelConfigs;
using Com.Bateeq.Service.Masterplan.Lib.Models;
using Com.Moonlay.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Com.Bateeq.Service.Masterplan.Lib
{
    public class MasterplanDbContext : StandardDbContext
    {
        public MasterplanDbContext(DbContextOptions<MasterplanDbContext> options) : base(options)
        {
        }

        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Section> Sections { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommodityConfig());
            modelBuilder.ApplyConfiguration(new SectionConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
