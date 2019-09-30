using ConsoleMatrixProcessing.Abstractions;
using ConsoleMatrixProcessing.Application;
using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace ConsoleMatrixProcessing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Add services to DI
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true))
                .AddCommandLineConfiguration(args)
                .AddTransient<IDataProvider, FileDataProvider>()
                .AddTransient<IConveyor, Conveyor>()
                .BuildServiceProvider();

            //Configure services
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //Run
            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            IDataProvider dataProvider = serviceProvider.GetRequiredService<IDataProvider>();
            IConfigurationProvider configurationProvider = serviceProvider.GetRequiredService<IConfigurationProvider>();
            IConveyor conveyor = serviceProvider.GetRequiredService<IConveyor>();
            Startup startup = new Startup(logger, configurationProvider, conveyor);
            startup.RunAsync().Wait();
        }
    }
}