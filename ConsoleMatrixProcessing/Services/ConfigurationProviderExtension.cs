using ConsoleMatrixProcessing.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleMatrixProcessing.Services
{
    public static class ConfigurationProviderExtension
    {
        public static IServiceCollection AddCommandLineConfiguration(this IServiceCollection services, string[] args)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<IConfigurationProvider, ConfigurationProvider>
                (
                    (service) =>
                    {
                        CommandLineConfigurationBuilder builder = new CommandLineConfigurationBuilder(args);
                        return builder.BuildConfigurationProvider();
                    }
                );
            return services;
        }
    }
}