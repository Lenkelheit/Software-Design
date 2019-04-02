using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        /// <summary>
        /// Configure Notification table's restrictions
        /// </summary>
        /// <param name="builder">
        /// Provides an API to configure <see cref="Notification"/> entity
        /// </param>
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // description
            builder.Property(n => n.Description).IsRequired();

            // resident
            builder.HasOne(n => n.Resident).WithMany(r => r.Notifications);
        }
    }
}
