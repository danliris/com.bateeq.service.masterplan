using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class BookingOrderDetailConfig : IEntityTypeConfiguration<BookingOrderDetail>
    {
        public void Configure(EntityTypeBuilder<BookingOrderDetail> builder)
        {
            builder.Property(b => b.RO).HasMaxLength(100);
            builder.Property(b => b.Article).HasMaxLength(500);
            builder.Property(b => b.Style).HasMaxLength(500);
            builder.Property(b => b.Counter).HasMaxLength(500);
        }
    }
}
