using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace ConsoleMatrixProcessingTests.Core
{
    public class TransposeProcessorCommandTests
    {
        [Fact]
        public void TransposeProcessorCommand_CalculateReturnTransposedMatrix()
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
                Data = new int[3, 1]
                {
                    {12},
                    {14},
                    {16}
                }
            };
            Matrix<int> assertFirstMatrix = new Matrix<int>
            {
                Data = new int[2, 3]
                {
                    {0, 2, 4},
                    {1, 3, 5},
                }
            };
            Matrix<int> assertSecondMatrix = new Matrix<int>
            {
                Data = new int[2, 2]
                {
                    {6, 8},
                    {7, 9},
                }
            };
            Matrix<int> assertThirdMatrix = new Matrix<int>
            {
                Data = new int[1, 3]
                {
                    {12, 14, 16},
                }
            };
            List<Matrix<int>> assertResult = new List<Matrix<int>>
            {
                assertFirstMatrix,
                assertSecondMatrix,
                assertThirdMatrix
            };
            TransposeProcessorCommand processor = new TransposeProcessorCommand
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