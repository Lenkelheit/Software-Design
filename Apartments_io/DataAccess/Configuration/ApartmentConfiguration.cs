using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Core.Configuration.DbEntityConfiguration;

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
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(APARTMENT_NAME_MAX_LENGTH);

            // description
            builder.Property(a => a.Description)
                   .IsRequired()
                   .HasMaxLength(APARTMENT_DESCRIPTION_MAX_LENGTH);

            // main photo
            builder.Property(a => a.MainPhoto)
                   .HasMaxLength(APARTMENT_PHOTO_PATH_MAX_LENGTH);

            // renter
            builder.HasOne(a => a.Renter)
                    .WithMany(r => r.Apartments)
                    .OnDelete(DeleteBehavior.SetNull);

            // requests
            builder.HasMany(a => a.Requests)
                    .WithOne(r => r.Apartment)
                    .OnDelete(DeleteBehavior.Cascade);

            // bills
            builder.HasMany(a => a.Bills)
                    .WithOne(b => b.Apartment)
                    .OnDelete(DeleteBehavior.Cascade);

            // rent end date
            builder.Property(a => a.RentEndDate).HasColumnType("date");
        }
    }
}
