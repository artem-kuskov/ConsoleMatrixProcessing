using ConsoleMatrixProcessing.Application;
using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Application.Models;
using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

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

        private int filesFound = 0;
        private int filesRead = 0;
        private int parsedCommands = 0;
        private int calculatedCommands = 0;
        private int filesWrite = 0;

        public async Task Run(IServiceProvider serviceProvider)
        {
            Logger = serviceProvider.GetRequiredService<ILogger<Startup>>();
            DataProvider = serviceProvider.GetRequiredService<IDataProvider>();
            Config = serviceProvider.GetRequiredService<IConfigurationProvider>();

            //Get parameters
            var parameters = ProcessCommandLineParameters();

            if (parameters.showHelp)
            {
                Help.Show();
                return;
            }
            Logger.LogInformation("Start process with parallelism={parall}", parameters.parallelism);

            var goodParallelismOption = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = parameters.queueSize,
                MaxDegreeOfParallelism = parameters.parallelism
            };

            //Usually file operations have bad parallel performance
            var badParallelismOption = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = parameters.queueSize,
                MaxDegreeOfParallelism = 1
            };

            TransformManyBlock<string, string> dataNamesEnumerator = GetDataIdProducer();
            TransformBlock<string, DataContentModel> readData = GetReadDataProducer(badParallelismOption);
            TransformBlock<DataContentModel, IProcessorCommand> convertToCommand = GetCalculateProducer(goodParallelismOption);
            TransformBlock<IProcessorCommand, DataContentModel> serializeCommand = GetSerializeProducer(goodParallelismOption);
            ActionBlock<DataContentModel> writeFiles = GetWriteDataAction(badParallelismOption);
            DataflowLinkOptions linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            //Stack conveyor
            //Drop errors
            dataNamesEnumerator.LinkTo(DataflowBlock.NullTarget<string>(), nameof => string.IsNullOrWhiteSpace(nameof));
            //Transfer good data
            dataNamesEnumerator.LinkTo(readData, linkOptions);

            readData.LinkTo(DataflowBlock.NullTarget<DataContentModel>(), model => model == null);
            readData.LinkTo(convertToCommand, linkOptions);

            convertToCommand.LinkTo(DataflowBlock.NullTarget<IProcessorCommand>(), command => command is BadProcessorCommand || !command.IsCalculated);
            convertToCommand.LinkTo(serializeCommand, linkOptions);

            serializeCommand.LinkTo(DataflowBlock.NullTarget<DataContentModel>(), model => model == null);
            serializeCommand.LinkTo(writeFiles, linkOptions);

            //Run conveyor
            Stopwatch stopWatch = Stopwatch.StartNew();
            try
            {
                dataNamesEnumerator.Post(parameters.filePath);
                dataNamesEnumerator.Complete();

                //Wait result
                await writeFiles.Completion;
            }
            catch (AggregateException exs)
            {
                foreach (Exception ex in exs.InnerExceptions)
                {
                    Logger.LogError("Errors while processing: {err}", ex.Message);
                }
            }
            stopWatch.Stop();

            Logger.LogInformation("End of processing");
            Logger.LogInformation("Files found: {counterF}. Files read: {counterR}. " +
                "Commands found: {counterP}. Commands successfully calculated: {counterC}. " +
                "Files saved: {counterW}. Time elapsed: {time} milliseconds",
                filesFound, filesRead, parsedCommands, calculatedCommands, filesWrite, stopWatch.ElapsedMilliseconds);
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

        private ActionBlock<DataContentModel> GetWriteDataAction(ExecutionDataflowBlockOptions options)
        {
            ActionBlock<DataContentModel> writeFiles = new ActionBlock<DataContentModel>
                (
                    async fileContent =>
                    {
                        try
                        {
                            await DataProvider.WriteFileAsync(fileContent);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Error writing file {id} ({error})", fileContent.FilePath, ex.Message);
                            return;
                        }
                        Interlocked.Increment(ref filesWrite);
                    },
                    options
                );
            return writeFiles;
        }

        private TransformBlock<IProcessorCommand, DataContentModel> GetSerializeProducer(ExecutionDataflowBlockOptions options)
        {
            var serializer = new TransformBlock<IProcessorCommand, DataContentModel>
                (
                    result =>
                    {
                        Interlocked.Increment(ref calculatedCommands);
                        return result.TryMapTo(out DataContentModel fileModel) ? fileModel : null;
                    },
                    options
                );
            return serializer;
        }

        private TransformBlock<DataContentModel, IProcessorCommand> GetCalculateProducer(ExecutionDataflowBlockOptions options)
        {
            var convertToCommand = new TransformBlock<DataContentModel, IProcessorCommand>
                (
                    dataContent =>
                    {
                        if (dataContent.TryMapTo(out IProcessorCommand processorCommand))
                        {
                            if (processorCommand is BadProcessorCommand)
                            {
                                Logger.LogError("Data format error in {id}", processorCommand.Id);
                                return processorCommand;
                            }
                            Interlocked.Increment(ref parsedCommands);

                            try
                            {
                                processorCommand.Calculate();
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError("Error processing command in {id} ({error})", processorCommand.Id, ex.Message);
                                return ProcessorCommandFabric.GetBadProcessor(processorCommand.Id);
                            }

                            return processorCommand;
                        }
                        //There is no command in file, skip without error message
                        return ProcessorCommandFabric.GetBadProcessor(dataContent.FilePath);
                    },
                    options
                );
            return convertToCommand;
        }

        private TransformBlock<string, DataContentModel> GetReadDataProducer(ExecutionDataflowBlockOptions options)
        {
            TransformBlock<string, DataContentModel> readData = new TransformBlock<string, DataContentModel>
                (
                    async dataPath =>
                    {
                        Interlocked.Increment(ref filesFound);
                        try
                        {
                            var result = await DataProvider.ReadDataToModelAsync(dataPath);
                            Interlocked.Increment(ref filesRead);
                            return result;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Error reading file {id} ({error})", dataPath, ex.Message);
                            return null;
                        }
                    },
                    options
                );
            return readData;
        }

        private TransformManyBlock<string, string> GetDataIdProducer()
        {
            return new TransformManyBlock<string, string>(
                path =>
                {
                    return DataProvider.GetDataNamesEnumerator(path);
                });
        }
    }
}