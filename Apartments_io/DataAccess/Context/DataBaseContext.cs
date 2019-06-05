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
        // FIELDS
        Interfaces.IDbInitializer dbInitializer;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="DataBaseContext"/> with given options
        /// </summary>
        /// <param name="options">
        /// The option to be used by <see cref="DataBaseContext"/>
        /// </param>
        /// <param name="dbInitializer">
        /// Data base initializer
        /// </param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options, Interfaces.IDbInitializer dbInitializer)
            : base(options)
        {
            this.dbInitializer = dbInitializer;
            Database.EnsureCreated();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="DataBaseContext"/> with given options
        /// </summary>
        /// <param name="options">
        /// The option to be used by <see cref="DataBaseContext"/>
        /// </param>
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : this(options, new Initializers.DefaultDbInitializer()) { }

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
            dbInitializer.Seed(modelBuilder);
        }
        /// <summary>
        /// Configure the database to be used for this context.
        /// </summary>
        /// <param name="optionsBuilder">
        /// Creates or modify options for context
        /// </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
    }
}
