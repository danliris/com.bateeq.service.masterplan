using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class WeeklyPlanConfig : IEntityTypeConfiguration<WeeklyPlan>
    {
        public void Configure(EntityTypeBuilder<WeeklyPlan> builder)
        {
            builder
                .Property(weeklyPlan => weeklyPlan.Year)
                .HasMaxLength(4);
            builder
                .HasMany(weeklyPlan => weeklyPlan.Items)
                .WithOne(weeklyPlanItem => weeklyPlanItem.WeeklyPlan)
                .HasForeignKey(weeklyPlanItem => weeklyPlanItem.WeeklyPlanId)
                .IsRequired();
        }
    }
}
