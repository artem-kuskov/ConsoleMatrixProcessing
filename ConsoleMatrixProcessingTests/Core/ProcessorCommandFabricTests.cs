using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace ConsoleMatrixProcessingTests.Core
{
    public class ProcessorCommandFabricTests
    {
        [Fact]
        public void ProcessorCommandFabricTests_GetProcessorReturnProcessorCommand()
        {
            //Arrange
            Matrix<int> firstMatrix = new Matrix<int>
            {
                Data = new int[3, 2]
                {
                    {0, 1},
                    {2, 3},
                    {4, 5}
                }
            };
            Matrix<int> secondMatrix = new Matrix<int>
            {
                Data = new int[3, 2]
                {
                    {6, 7},
                    {8, 9},
                    {10, 11}
                }
            };
            Matrix<int> thirdMatrix = new Matrix<int>
            {
                Data = new int[3, 2]
                {
                    {12, 13},
                    {14, 15},
                    {16, 17}
                }
            };
            AddProcessorCommand assertAddResult = new AddProcessorCommand()
            {
                Id = null,
                Source = new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                }
            };
            SubtractProcessorCommand assertSubResult = new SubtractProcessorCommand()
            {
                Id = null,
                Source = new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                }
            };
            TransposeProcessorCommand assertTranResult = new TransposeProcessorCommand()
            {
                Id = null,
                Source = new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                }
            };
            MultiplyProcessorCommand assertMultResult = new MultiplyProcessorCommand()
            {
                Id = null,
                Source = new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                }
            };
            BadProcessorCommand assertBadResult = new BadProcessorCommand();

            //Act
            ConsoleMatrixProcessing.Core.Abstractions.IProcessorCommand addProcessor = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Add,
                new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                });
            ConsoleMatrixProcessing.Core.Abstractions.IProcessorCommand subProcessor = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Subtract,
                new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                });
            ConsoleMatrixProcessing.Core.Abstractions.IProcessorCommand multProcessor = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Multiply,
                new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                });
            ConsoleMatrixProcessing.Core.Abstractions.IProcessorCommand tranProcessor = ProcessorCommandFabric.GetProcessor(ProcessorCommandFabric.Operator.Transpose,
                new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                });
            ConsoleMatrixProcessing.Core.Abstractions.IProcessorCommand badProcessor = ProcessorCommandFabric.GetBadProcessor();

            //Assert
            Assert.Equal(assertAddResult, addProcessor);
            Assert.Equal(assertSubResult, subProcessor);
            Assert.Equal(assertMultResult, multProcessor);
            Assert.Equal(assertTranResult, tranProcessor);
            Assert.Equal(assertBadResult, badProcessor);

            Assert.NotEqual(assertAddResult, subProcessor);
            Assert.NotEqual(assertSubResult, multProcessor);
            Assert.NotEqual(assertMultResult, tranProcessor);
            Assert.NotEqual(assertTranResult, subProcessor);
            Assert.NotEqual(assertBadResult, addProcessor);
        }
    }
}