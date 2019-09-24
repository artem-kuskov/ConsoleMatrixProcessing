using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Application.Abstractions
{
    public interface IConfigurationProvider
    {
        string GetValue(string key);
        IEnumerable<string> GetKeys();
        Dictionary<string, string> ToDictionary();
    }
}