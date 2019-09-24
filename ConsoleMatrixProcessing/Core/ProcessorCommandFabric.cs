using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core
{
    public static class ProcessorCommandFabric
    {
        public enum Operator
        {
            Add,
            Bad,
            Multiply,
            Subtract,
            Transpose,
        }

        public static IProcessorCommand GetProcessor(Operator operation)
        {
            switch (operation)
            {
                case Operator.Add:
                    return new AddProcessorCommand();
                case Operator.Multiply:
                    return new MultiplyProcessorCommand();
                case Operator.Transpose:
                    return new TransposeProcessorCommand();
                case Operator.Subtract:
                    return new SubtractProcessorCommand();
                default:
                    return new BadProcessorCommand();
            }
        }

        public static IProcessorCommand GetProcessor(Operator operation, IEnumerable<Matrix<int>> matrices)
        {
            IProcessorCommand command = GetProcessor(operation);
            command.Source = matrices;
            return command;
        }

        public static IProcessorCommand GetBadProcessor()
        {
            return new BadProcessorCommand();
        }

        public static IProcessorCommand GetBadProcessor(string id)
        {
            return new BadProcessorCommand { Id = id };
        }
    }
}