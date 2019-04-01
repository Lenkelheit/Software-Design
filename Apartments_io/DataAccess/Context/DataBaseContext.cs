using Microsoft.EntityFrameworkCore;

using DataAccess.Entities;
using DataAccess.Configuration;

namespace DataAccess.Context
{
    public class DataBaseContext : DbContext
    {
        // CONSTRUCTORS
        static DataBaseContext()
        {
            Instance = new DataBaseContext();
        }

        // PROPERTIES
        public static DataBaseContext Instance { get; }

        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<User> Users { get; set; }

        // METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ApartmentConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
