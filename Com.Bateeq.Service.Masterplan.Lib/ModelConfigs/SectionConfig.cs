using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class SectionConfig : IEntityTypeConfiguration<Section>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Section> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.Name).HasMaxLength(255);
            builder.Property(c => c.Remark).HasMaxLength(300);
        }
    }
}
