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
        
        public DbSet<Section> Sections { get; set; }
        public DbSet<WeeklyPlan> WeeklyPlans { get; set; }
        public DbSet<WeeklyPlanItem> WeeklyPlanItems { get; set; }
        public DbSet<BookingOrder> BookingOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SectionConfig());
            modelBuilder.ApplyConfiguration(new WeeklyPlanConfig());
            modelBuilder.ApplyConfiguration(new BookingOrderConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
