using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace gRPCClientBasic
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = AppStartup();
            var serviceConsumer = ActivatorUtilities.CreateInstance<ServiceConsumer>(host.Services);
                       

            ////call gRPC Service
            //serviceConsumer.GetUserById(590);
            ////call async Service
            //await serviceConsumer.GetUserByIdAsync(800);
            ////call async Service
            //await serviceConsumer.GetPeople();

            //serviceConsumer.CloseClient();

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



        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddTransient<IServiceConsumer,ServiceConsumer>();
                });
        }

    }
}
