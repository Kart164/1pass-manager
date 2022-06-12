using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace _1Pass.UI.XamarinUi.UWP
{
    public static class StartUwpExtensions
    {
        public static void InitUWP(Config config)
        {
            var servicecollection = new ServiceCollection().ConfigureServices(config).BuildServiceProvider();

            Startup.ServiceProvider = servicecollection;
            Startup.Configuration = config;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, Config configuration)
        {
            services.AddScoped<IDatabase>(_ => new DatabaseUWP(configuration.DbPath));
            services.AddSingleton<AccountRepo>();
            services.AddSingleton<ServiceRepo>();
            return services;
        }
    }
}
