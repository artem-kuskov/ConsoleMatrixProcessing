using ConsoleMatrixProcessing.Abstractions;
using ConsoleMatrixProcessing.Application.Abstractions;
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

        private ILogger Logger { get; }
        private IConfigurationProvider Config { get; }
        private IConveyor Conveyor { get; }

        public Startup(ILogger logger, IConfigurationProvider configurationProvider, IConveyor conveyor)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Config = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            Conveyor = conveyor ?? throw new ArgumentNullException(nameof(conveyor));
        }

        public async Task RunAsync()
        {
            //Get parameters
            (string filePath, int parallelism, int queueSize, bool showHelp) = ProcessCommandLineParameters();

            if (showHelp)
            {
                Help.Show();
                return;
            }
            Logger.LogInformation("Start process with parallelism={parall}", parallelism);

            //Arrange and run conveyor
            Conveyor.Arrange(filePath, parallelism, queueSize);
            await Conveyor.RunAsync();
            ConveyorCounters counters = Conveyor.GetCounters();

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