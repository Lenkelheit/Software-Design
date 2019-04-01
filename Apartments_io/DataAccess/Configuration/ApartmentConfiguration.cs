using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        /// <summary>
        /// Configure Apartment table's restrictions
        /// </summary>
        /// <param name="builder">
        /// Provides an API to configure <see cref="Apartment"/> entity
        /// </param>
        public void Configure(EntityTypeBuilder<Apartment> builder)
        {
            // name
            builder.Property(a => a.Name).IsRequired();

            // description
            builder.Property(a => a.Description).IsRequired();

            // renter
            builder.HasOne(a => a.Renter).WithMany(r => r.Apartments);

            // requests
            builder.HasMany(a => a.Requests).WithOne(r => r.Apartment);

            // bills
            builder.HasMany(a => a.Bills).WithOne(b => b.Apartment);
        }
    }
}
