using ConsoleMatrixProcessing.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleMatrixProcessing.Application.Abstractions
{
    public interface IDataProvider
    {
        IEnumerable<string> GetDataNamesEnumerator(string path);
        Task<DataContentModel> ReadDataToModelAsync(string fileNamePath);
        Task WriteFileAsync(DataContentModel fileContent);
    }
}