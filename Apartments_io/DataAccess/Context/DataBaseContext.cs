using Microsoft.EntityFrameworkCore;

using DataAccess.Entities;
using DataAccess.Configuration;

namespace DataAccess.Context
{
    public class DataBaseContext : DbContext
    {
        // CONSTRUCTORS
        public DataBaseContext()
        {
            Database.EnsureCreated();
        }
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning temporary solution, should be removed later on with migration
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Apartment_io_TempDB;Trusted_Connection=True;");
        }
    }
}
