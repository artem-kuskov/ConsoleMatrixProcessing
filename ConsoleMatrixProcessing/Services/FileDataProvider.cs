using ConsoleMatrixProcessing.Application.Abstractions;
using ConsoleMatrixProcessing.Application.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleMatrixProcessing.Services
{
    public class FileDataProvider : IDataProvider
    {
        private ILogger Logger { get; set; }

        public FileDataProvider(ILogger<FileDataProvider> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<string> GetDataNamesEnumerator(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = Directory.GetCurrentDirectory();
                }
                Logger.LogInformation("Observed path: {path}", path);
                return Directory.EnumerateFiles(path);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error getting files list in {path}", path);
                throw;
            }
        }

        public async Task<DataContentModel> ReadDataToModelAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Logger.LogError("Can not read file with empty path");
                throw new ArgumentException("Unspecified file name", nameof(filePath));
            }

            DataContentModel model = new DataContentModel
            {
                FilePath = filePath
            };
            try
            {
                model.ContentStrings = await File.ReadAllLinesAsync(filePath);
                Logger.LogInformation("Read file {filePath}", filePath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error reading file {filePath}", filePath);
                throw;
            }
            return model;
        }

        public async Task WriteFileAsync(DataContentModel fileContent)
        {
            if (fileContent == null)
            {
                throw new ArgumentNullException(nameof(fileContent));
            }

            try
            {
                await File.WriteAllLinesAsync(fileContent.FilePath, fileContent.ContentStrings);
                Logger.LogInformation("Write result to file {filePath}", fileContent.FilePath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error writing to file {filePath}", fileContent.FilePath);
                throw;
            }
        }
    }
}