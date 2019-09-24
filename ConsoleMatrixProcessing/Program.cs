using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Services;
using Microsoft.Extensions.DependencyInjection;
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
                .BuildServiceProvider();

            //Configure services
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //Run
            Startup startup = new Startup();
            startup.Run(serviceProvider).Wait();
        }
    }
}