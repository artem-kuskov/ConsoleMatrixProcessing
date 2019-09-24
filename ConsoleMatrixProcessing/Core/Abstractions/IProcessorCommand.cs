using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core.Abstractions
{
    public interface IProcessorCommand
    {
        string Id { get; set; }
        IEnumerable<Matrix<int>> Source { get; set; }
        bool IsCalculated { get; }
        IEnumerable<Matrix<int>> Result { get; }

        void Calculate();
    }
}
