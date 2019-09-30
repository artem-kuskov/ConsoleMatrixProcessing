using ConsoleMatrixProcessing.Abstractions;
using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Application.Models;
using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ConsoleMatrixProcessing.Application
{
    public class Conveyor : IConveyor
    {
        private TransformManyBlock<string, string> dataNamesEnumeratorBlock;
        private TransformBlock<string, DataContentModel> readDataBlock;
        private TransformBlock<DataContentModel, IProcessorCommand> convertToCommandBlock;
        private TransformBlock<IProcessorCommand, DataContentModel> serializeCommandBlock;
        private ActionBlock<DataContentModel> writeFilesBlock;
        private string filePath;
        private int filesWrite;
        private int calculatedCommands;
        private int parsedCommands;
        private int filesFound;
        private int filesRead;
        private long elapsedTime;

        public bool IsBuilt { get; private set; } = false;

        private IDataProvider DataProvider { get; set; }
        private ILogger<Conveyor> Logger { get; }

        public Conveyor(ILogger<Conveyor> logger, IDataProvider dataProvider)
        {
            Logger = logger;
            DataProvider = dataProvider;
        }

        public void Arrange(string dataSource, int parallelism, int queueSize)
        {
            filePath = dataSource;
            ExecutionDataflowBlockOptions goodParallelismOption = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = queueSize,
                MaxDegreeOfParallelism = parallelism
            };
            //Usually file operations have bad parallel performance
            ExecutionDataflowBlockOptions badParallelismOption = new ExecutionDataflowBlockOptions
            {
                BoundedCapacity = queueSize,
                MaxDegreeOfParallelism = 1
            };
            dataNamesEnumeratorBlock = GetDataIdProducer();
            readDataBlock = GetReadDataProducer(badParallelismOption);
            convertToCommandBlock = GetCalculateProducer(goodParallelismOption);
            serializeCommandBlock = GetSerializeProducer(goodParallelismOption);
            writeFilesBlock = GetWriteDataAction(badParallelismOption);
            DataflowLinkOptions linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            //Stack conveyor
            //Drop errors
            dataNamesEnumeratorBlock.LinkTo(DataflowBlock.NullTarget<string>(), nameof => string.IsNullOrWhiteSpace(nameof));
            //Transfer good data
            dataNamesEnumeratorBlock.LinkTo(readDataBlock, linkOptions);

            readDataBlock.LinkTo(DataflowBlock.NullTarget<DataContentModel>(), model => model == null);
            readDataBlock.LinkTo(convertToCommandBlock, linkOptions);

            convertToCommandBlock.LinkTo(DataflowBlock.NullTarget<IProcessorCommand>(), command => command is BadProcessorCommand || !command.IsCalculated);
            convertToCommandBlock.LinkTo(serializeCommandBlock, linkOptions);

            serializeCommandBlock.LinkTo(DataflowBlock.NullTarget<DataContentModel>(), model => model == null);
            serializeCommandBlock.LinkTo(writeFilesBlock, linkOptions);
            IsBuilt = true;
        }

        public async Task RunAsync()
        {
            if (!IsBuilt)
            {
                Logger.LogError("Error run not arranged conveyor. Use {arrangeMethod}() before {runMethod}().", nameof(Arrange), nameof(RunAsync));
                throw new InvalidOperationException($"Error run not arranged conveyor. Use {nameof(Arrange)}() before {nameof(RunAsync)}().");
            }
            //Run conveyor
            Stopwatch stopWatch = Stopwatch.StartNew();
            try
            {
                dataNamesEnumeratorBlock.Post(filePath);
                dataNamesEnumeratorBlock.Complete();

                //Wait result
                await writeFilesBlock.Completion;
            }
            catch (AggregateException exs)
            {
                foreach (Exception ex in exs.InnerExceptions)
                {
                    Logger.LogError("Errors while processing: {err}", ex.Message);
                }
            }
            stopWatch.Stop();
            elapsedTime = stopWatch.ElapsedMilliseconds;
        }

        public ConveyorCounters GetCounters()
        {
            return new ConveyorCounters
            {
                filesWrite = filesWrite,
                calculatedCommands = calculatedCommands,
                parsedCommands = parsedCommands,
                filesFound = filesFound,
                filesRead = filesRead,
                elapsedTime = elapsedTime
            };
        }
        private ActionBlock<DataContentModel> GetWriteDataAction(ExecutionDataflowBlockOptions options)
        {
            var writeFiles = new ActionBlock<DataContentModel>
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
            var readData = new TransformBlock<string, DataContentModel>
                (
                    async dataPath =>
                    {
                        Interlocked.Increment(ref filesFound);
                        try
                        {
                            DataContentModel result = await DataProvider.ReadDataToModelAsync(dataPath);
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