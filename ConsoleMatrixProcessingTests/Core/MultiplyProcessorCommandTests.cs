using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace ConsoleMatrixProcessingTests.Core
{
    public class MultiplyProcessorCommandTests
    {
        [Fact]
        public void MultiplyProcessorCommand_CalculateReturnMultipliedMatrix()
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
                Data = new int[2, 2]
                {
                    {6, 7},
                    {8, 9},
                }
            };
            Matrix<int> thirdMatrix = new Matrix<int>
            {
                Data = new int[2, 3]
                {
                    {12, 13, 14},
                    {15, 16, 17},
                }
            };
            Matrix<int> assertMatrix = new Matrix<int>
            {
                Data = new int[3, 3]
                {
                    {231, 248, 265},
                    {1047, 1124, 1201},
                    {1863, 2000, 2137}
                }
            };
            List<Matrix<int>> assertResult = new List<Matrix<int>>
            {
                assertMatrix
            };
            MultiplyProcessorCommand processor = new MultiplyProcessorCommand
            {
                Source = new List<Matrix<int>>
                {
                    firstMatrix,
                    secondMatrix,
                    thirdMatrix
                }
            };

            //Act
            processor.Calculate();

            //Assert
            Assert.True(processor.IsCalculated);
            Assert.Equal(assertResult, processor.Result);
        }
    }
}