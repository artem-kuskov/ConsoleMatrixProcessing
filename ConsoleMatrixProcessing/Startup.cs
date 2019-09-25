using ConsoleMatrixProcessing.Application;
using ConsoleMatrixProcessing.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConsoleMatrixProcessing
{
    public class Startup
    {
        private string ParallelismParameterKey { get; } = "parallelism";
        private string BufferSizeParameterKey { get; } = "buffer";
        private string PathParameterKey { get; } = "path";
        private string HelpParameterKey { get; } = "help";
        private int DefaultBufferSize { get; } = 100;
        private int DefaultParallelism { get; } = 4;

        private ILogger Logger { get; set; }
        private IConfigurationProvider Config { get; set; }
        private IDataProvider DataProvider { get; set; }

        public async Task Run(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
            DataProvider = serviceProvider.GetRequiredService<IDataProvider>();
            Config = serviceProvider.GetRequiredService<IConfigurationProvider>();

            //Get parameters
            var (filePath, parallelism, queueSize, showHelp) = ProcessCommandLineParameters();

            if (showHelp)
            {
                Help.Show();
                return;
            }
            Logger.LogInformation("Start process with parallelism={parall}", parallelism);

            //Build and run conveyor
            var conveyor = new Conveyor(Logger, DataProvider)
                .Build(filePath, parallelism, queueSize);
            await conveyor.Run();
            var counters = conveyor.GetCounters();

            Logger.LogInformation("End of processing");
            Logger.LogInformation("Files found: {counterF}. Files read: {counterR}. " +
                "Commands found: {counterP}. Commands successfully calculated: {counterC}. " +
                "Files saved: {counterW}. Time elapsed: {time} milliseconds",
                counters.filesFound, counters.filesRead, counters.parsedCommands, 
                counters.calculatedCommands, counters.filesWrite, counters.elapsedTime);
        }

        private (string filePath, int parallelism, int queueSize, bool showHelp) ProcessCommandLineParameters()
        {
            string filesPath = Config.GetValue(PathParameterKey);
            bool showHelp = false;
            if (!string.IsNullOrWhiteSpace(Config.GetValue(HelpParameterKey)) ||
                string.IsNullOrWhiteSpace(filesPath))
            {
                showHelp = true;
            }
            int.TryParse(Config.GetValue(ParallelismParameterKey), out int parallelism);
            if (parallelism < 1)
            {
                parallelism = DefaultParallelism;
            }
            int.TryParse(Config.GetValue(BufferSizeParameterKey), out int queueSize);
            if (queueSize < 1)
            {
                queueSize = DefaultBufferSize;
            }
            return (filesPath, parallelism, queueSize, showHelp);
        }
    }
}