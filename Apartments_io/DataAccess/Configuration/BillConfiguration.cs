using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        /// <summary>
        /// Configure Bill table's restrictions
        /// </summary>
        /// <param name="builder">
        /// Provides an API to configure <see cref="Bill"/> entity
        /// </param>
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            // renter
            builder.HasOne(b => b.Renter)
                    .WithMany(r => r.Bills)
                    .OnDelete(DeleteBehavior.Cascade);

            // apartment
            builder.HasOne(b => b.Apartment)
                    .WithMany(a => a.Bills)
                    .OnDelete(DeleteBehavior.Cascade);

            // date
            builder.Property(b => b.EndDate)
                    .IsRequired()
                    .HasColumnType("date");
        }
    }
}
