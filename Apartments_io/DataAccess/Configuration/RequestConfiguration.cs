using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        /// <summary>
        /// Configure Request table's restrictions
        /// </summary>
        /// <param name="builder">
        /// Provides an API to configure <see cref="Request"/> entity
        /// </param>
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            // resident
            builder.HasOne(r => r.Resident).WithMany(r => r.Requests);

            // apartment
            builder.HasOne(r => r.Apartment)
                    .WithMany(a => a.Requests)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
