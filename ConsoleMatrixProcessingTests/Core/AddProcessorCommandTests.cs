using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Models;
using System.Collections.Generic;
using Xunit;

namespace ConsoleMatrixProcessingTests.Core
{
    public class AddProcessorCommandTests
    {
        [Fact]
        public void AddProcessorCommand_CalculateReturnAddingMatrix()
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
            Matrix<int> assertMatrix = new Matrix<int>
            {
                Data = new int[3, 2]
                {
                    {18, 21},
                    {24, 27},
                    {30, 33}
                }
            };
            List<Matrix<int>> assertResult = new List<Matrix<int>>
            {
                assertMatrix
            };
            AddProcessorCommand processor = new AddProcessorCommand
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
