using Microsoft.EntityFrameworkCore;

using DataAccess.Entities;
using DataAccess.Configuration;

namespace DataAccess.Context
{
    /// <summary>
    /// Contains DbSets
    /// </summary>
    public class DataBaseContext : DbContext
    {
        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="DataBaseContext"/> with given options
        /// </summary>
        /// <param name="options">
        /// The option to be used by <see cref="DataBaseContext"/>
        /// </param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        // PROPERTIES
        /// <summary>
        /// Gets an apartment set
        /// </summary>
        public DbSet<Apartment> Apartments { get; set; }
        /// <summary>
        /// Gets a bills set
        /// </summary>
        public DbSet<Bill> Bills { get; set; }
        /// <summary>
        /// Gets a notification set
        /// </summary>
        public DbSet<Notification> Notifications { get; set; }
        /// <summary>
        /// Gets a request set
        /// </summary>
        public DbSet<Request> Requests { get; set; }
        /// <summary>
        /// Gets a user set
        /// </summary>
        public DbSet<User> Users { get; set; }

        // METHODS
        /// <summary>
        /// Configures the entities
        /// </summary>
        /// <param name="modelBuilder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configure entities
            modelBuilder.ApplyConfiguration(new ApartmentConfiguration());
            modelBuilder.ApplyConfiguration(new BillConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // adds data

            User manager = new User()
            {
                Id = 2,
                FirstName = "Manager",
                LastName = "Manager",
                Email = "manager@gmail.com",
                Password = "1111",
                Role = Enums.Role.Manager,
            };
            User administrator = new User()
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Password = "1111",
                Role = Enums.Role.Administrator,
            };

            modelBuilder.Entity<User>().HasData(manager, administrator);
        }
        /// <summary>
        /// Configure the database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// Creates or modify options for context
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
