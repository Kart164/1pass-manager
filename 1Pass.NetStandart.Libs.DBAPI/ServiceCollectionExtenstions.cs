using _1Pass.NetStandart.Libs.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace _1Pass.NetStandart.Libs.DBAPI
{
    public static class ServiceCollectionExtenstions
    {
        public static IServiceCollection ConfigureDb(this IServiceCollection services, Config configuration)
        {
            services.AddSingleton(new Database(configuration.DbPath));
            services.AddTransient<IDatabase, Database>();
            services.AddSingleton<ServiceRepo>();
            services.AddSingleton<AccountRepo>();
            return services;
        }
    }
}
