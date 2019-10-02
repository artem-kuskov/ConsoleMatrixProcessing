using ConsoleMatrixProcessing;
using ConsoleMatrixProcessing.Abstractions;
using ConsoleMatrixProcessing.Application;
using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleMatrixProcessingTests.Integration.Tests
{
    public class ProgramTests
    {
        [Fact]
        public async Task Program_CreateResultFiles()
        {
            //Arrange
            const string manualTestFilesDirectoryName = "Manual.Tests";
            const string testDataDirectoryName = "test_data";
            const string referenceDirectoryName = "reference_result";

            //Copy test files to temp directory
            string path = Path.GetTempPath();
            string tempDirectory = Path.Combine(path, testDataDirectoryName);
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            Directory.CreateDirectory(tempDirectory);
            string testFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, manualTestFilesDirectoryName, testDataDirectoryName);
            string referenceFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, manualTestFilesDirectoryName, referenceDirectoryName);
            foreach (var fromFilePath in Directory.EnumerateFiles(testFilesPath))
            {
                string toFilePath = Path.Combine(tempDirectory, Path.GetFileName(fromFilePath));
                File.Copy(fromFilePath, toFilePath);
            }

            //Add services to DI
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true))
                .AddCommandLineConfiguration(new string[] { tempDirectory })
                .AddTransient<IDataProvider, FileDataProvider>()
                .AddTransient<IConveyor, Conveyor>()
                .BuildServiceProvider();

            //Configure services
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            ILogger<ProgramTests> logger = serviceProvider.GetRequiredService<ILogger<ProgramTests>>();
            IDataProvider dataProvider = serviceProvider.GetRequiredService<IDataProvider>();
            IConfigurationProvider configurationProvider = serviceProvider.GetRequiredService<IConfigurationProvider>();
            IConveyor conveyor = serviceProvider.GetRequiredService<IConveyor>();

            //Act
            Startup startup = new Startup(logger, configurationProvider, conveyor);
            startup.RunAsync().Wait();
            
            //Read test files after processing
            var factFilesDict = new Dictionary<string, string[]>();
            foreach (var factFilePath in Directory.GetFiles(tempDirectory))
            {
                factFilesDict[Path.GetFileName(factFilePath)] = await File.ReadAllLinesAsync(factFilePath);
            }
            //Read reference files to compare
            var assertFilesDict = new Dictionary<string, string[]>();
            foreach (var assertFilePath in Directory.GetFiles(referenceFilesPath))
            {
                assertFilesDict[Path.GetFileName(assertFilePath)] = await File.ReadAllLinesAsync(assertFilePath);
            }
            Directory.Delete(tempDirectory, true);

            //Assert
            Assert.Equal(assertFilesDict, factFilesDict);
        }
    }
}