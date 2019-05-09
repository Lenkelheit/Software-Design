using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using static Core.Configuration.DbEntityConfiguration;

namespace DataAccess.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // first name
            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(USER_FIRST_NAME_MAX_LENGTH);

            // last name
            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(USER_LAST_NAME_MAX_LENGTH);

            // email
            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(USER_EMAIL_MAX_LENGTH);

            // password
            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(USER_PASSWORD_MAX_LENGTH);
            
            // manager
            builder.HasOne(u => u.Manager).WithMany(u => u.Resident);

            // requests
            builder.HasMany(u => u.Requests)
                    .WithOne(r => r.Resident)
                    .OnDelete(DeleteBehavior.Cascade);

            // notifications
            builder.HasMany(u => u.Notifications)
                    .WithOne(n => n.Resident)
                    .OnDelete(DeleteBehavior.Cascade); 

            // apartments
            builder.HasMany(u => u.Apartments).WithOne(a => a.Renter).OnDelete(DeleteBehavior.SetNull);

            // bills
            builder.HasMany(u => u.Bills)
                .WithOne(b => b.Renter)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
