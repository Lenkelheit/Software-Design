using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Infrastructure
{
    /// <summary>
    /// Provides mechanism to configure data access project on ASP project
    /// </summary>
    public static class AspInfrastructure
    {
        /// <summary>
        /// Register services to container
        /// </summary>
        /// <param name="services">
        /// A set of key/value application' services
        /// </param>
        /// <param name="configuration">
        /// A set of key/value application configuration properties
        /// </param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("ApartmentIoDatabase");
            services.AddDbContext<Context.DataBaseContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });


            // add services
            services.AddScoped(typeof(DbContext), typeof(Context.DataBaseContext));

            services.AddScoped(typeof(Interfaces.IUnitOfWork), typeof(Context.UnitOfWork));

            services.AddScoped(typeof(Interfaces.IFileService), typeof(Services.FileService));
        }

        /// <summary>
        /// Configures HTTP request pipeline
        /// </summary>
        /// <param name="app">
        /// An instance of class that can configure an application's request pipeline.
        /// </param>
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }
    }
}
