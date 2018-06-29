using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class BlockingPlanConfig : IEntityTypeConfiguration<BlockingPlan>
    {
        public void Configure(EntityTypeBuilder<BlockingPlan> builder)
        {
            builder
                .HasMany(b => b.WorkSchedules)
                .WithOne(d => d.BlockingPlan)
                .HasForeignKey(d => d.BlockingPlanId)
                .IsRequired();
        }
    }
}
