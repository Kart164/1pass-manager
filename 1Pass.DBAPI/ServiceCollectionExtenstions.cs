using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace _1Pass.DBAPI
{
    public static class ServiceCollectionExtenstions
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString("DbPath");
            services.AddSingleton(new Database(connection));
            services.AddSingleton<ServiceRepo>();
            services.AddSingleton<AccountRepo>();
            return services;
        }
    }
}
