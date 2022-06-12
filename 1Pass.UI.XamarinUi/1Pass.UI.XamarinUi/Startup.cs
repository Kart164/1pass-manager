using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;

namespace _1Pass.UI.XamarinUi
{
    public static class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static Config Configuration { get; set; }

        public static void Init(Config configuration)
        {
            var serviceCollection = new ServiceCollection()
                .ConfigureServices(configuration)
                .BuildServiceProvider();
            ServiceProvider = serviceCollection;
            Configuration = configuration;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services, Config configuration)
        {
            return services.ConfigureDb(configuration);
        }
    }   
}
