using Com.Bateeq.Service.Masterplan.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Masterplan.Lib.ModelConfigs
{
    public class BookingOrderConfig : IEntityTypeConfiguration<BookingOrder>
    {
        public void Configure(EntityTypeBuilder<BookingOrder> builder)
        {
            builder.Property(b => b.Code).HasMaxLength(100);
            builder.Ignore(c => c.SectionCode);
            builder.Property(b => b.SectionName).HasMaxLength(300);
            builder.Ignore(c => c.BuyerCode);
            builder.Property(b => b.BuyerName).HasMaxLength(300);
        }
    }
}
