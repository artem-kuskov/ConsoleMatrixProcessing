using ConsoleMatrixProcessing.Application.Models;
using ConsoleMatrixProcessing.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleMatrixProcessingTests.Services
{
    public class FileDataProviderTests
    {
        [Fact]
        public void FileDataProvider_ReturnFileNamesList()
        {
            //Arrange
            Mock<ILogger<FileDataProvider>> moq = new Mock<ILogger<FileDataProvider>>(MockBehavior.Loose);

            FileDataProvider fileDataProvider = new FileDataProvider(moq.Object);
            string path = Path.GetTempPath();
            string tempDirectory = Path.Combine(path, "testfiledataprovidertest1");
            List<string> assertFileNamesList = new List<string>
            {
                Path.Combine(tempDirectory, "file1.txt"),
                Path.Combine(tempDirectory, "file2.txt"),
                Path.Combine(tempDirectory, "file3.txt"),
                Path.Combine(tempDirectory, "file4.txt"),
            };
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            Directory.CreateDirectory(tempDirectory);
            assertFileNamesList.ForEach(async fileName => await File.WriteAllTextAsync(fileName, ""));

            //Act
            List<string> resultList = fileDataProvider.GetDataNamesEnumerator(tempDirectory).OrderBy(name => name).ToList();
            Directory.Delete(tempDirectory, true);

            //Assert
            Assert.Equal(assertFileNamesList, resultList);
        }

        [Fact]
        public async Task FileDataProvider_ReturnFileContentModel()
        {
            //Arrange
            Mock<ILogger<FileDataProvider>> moq = new Mock<ILogger<FileDataProvider>>(MockBehavior.Loose);

            FileDataProvider fileDataProvider = new FileDataProvider(moq.Object);
            string path = Path.GetTempPath();
            string tempDirectory = Path.Combine(path, "testfiledataprovidertest2");
            string tempFileName = Path.Combine(tempDirectory, "file1.txt");
            DataContentModel assertFileContentModel = new DataContentModel()
            {
                FilePath = tempFileName,
                ContentStrings = new string[]
                {
                    "Line1",
                    "Line2",
                    "Line3"
                }
            };
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            Directory.CreateDirectory(tempDirectory);
            await File.WriteAllLinesAsync(tempFileName, assertFileContentModel.ContentStrings);

            //Act
            DataContentModel resultModel = await fileDataProvider.ReadDataToModelAsync(tempFileName);
            Directory.Delete(tempDirectory, true);

            //Assert
            Assert.Equal(assertFileContentModel, resultModel);
        }

        [Fact]
        public async Task FileDataProvider_WriteFileContentModel()
        {
            //Arrange
            Mock<ILogger<FileDataProvider>> moq = new Mock<ILogger<FileDataProvider>>(MockBehavior.Loose);

            FileDataProvider fileDataProvider = new FileDataProvider(moq.Object);
            string path = Path.GetTempPath();
            string tempDirectory = Path.Combine(path, "testfiledataprovidertest3");
            string tempFileName = Path.Combine(tempDirectory, "file1.txt_result.txt");
            DataContentModel fileContentModel = new DataContentModel()
            {
                FilePath = tempFileName,
                ContentStrings = new string[]
                {
                    "Line1",
                    "Line2",
                    "Line3"
                }
            };
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            Directory.CreateDirectory(tempDirectory);

            //Act
            await fileDataProvider.WriteFileAsync(fileContentModel);

            string[] result = await File.ReadAllLinesAsync(tempFileName);
            Directory.Delete(tempDirectory, true);

            //Assert
            Assert.Equal(fileContentModel.ContentStrings, result);
        }
    }
}