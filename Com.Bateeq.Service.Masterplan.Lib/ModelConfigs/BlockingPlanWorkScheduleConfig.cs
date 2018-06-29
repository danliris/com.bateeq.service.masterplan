using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class BlockingPlanWorkScheduleConfig : IEntityTypeConfiguration<BlockingPlanWorkSchedule>
    {
        public void Configure(EntityTypeBuilder<BlockingPlanWorkSchedule> builder)
        {
            builder.Property(b => b.YearText).HasMaxLength(100);
        }
    }
}
