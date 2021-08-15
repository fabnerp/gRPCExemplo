using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace gRPCClientBasic
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = AppStartup();
            var serviceConsumer = ActivatorUtilities.CreateInstance<IServiceConsumer>(host.Services);

            //call gRPC Service
            serviceConsumer.GetUserById(590);

        }

        static void ConfigSetup(IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        }

        static IHost AppStartup()
        {
            var builder = new ConfigurationBuilder();

            ConfigSetup(builder);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IServiceConsumer, ServiceConsumer>();
                })
                .Build();

            return host;
        }
    }
}
