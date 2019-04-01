using DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configuration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // first name
            builder.Property(u => u.FirstName).IsRequired();

            // last name
            builder.Property(u => u.LastName).IsRequired();

            // email
            builder.Property(u => u.Email).IsRequired();

            // password
            builder.Property(u => u.Password).IsRequired();
            
            // manager
            builder.HasOne(u => u.Manager).WithMany(u => u.Resident);

            // requests
            builder.HasMany(u => u.Requests).WithOne(r => r.Resident);

            // notifications
            builder.HasMany(u => u.Notifications).WithOne(n => n.Resident);

            // apartments
            builder.HasMany(u => u.Apartments).WithOne(a => a.Renter);

            // bills
            builder.HasMany(u => u.Bills).WithOne(b => b.Renter);
        }
    }
}
