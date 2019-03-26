using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Infrastructure
{
    public static class AspInfrastructure
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            throw new System.NotImplementedException();
        }

        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            throw new System.NotImplementedException();
        }
    }
}
