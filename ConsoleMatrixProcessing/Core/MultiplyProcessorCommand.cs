using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core
{
    public class MultiplyProcessorCommand : BaseProcessorCommand, IProcessorCommand
    {
        public override void Calculate()
        {
            if (IsCalculated)
            {
                return;
            }
            if (Source is null)
            {
                Result = new List<Matrix<int>>();
                return;
            }

            List<Matrix<int>> resultList = new List<Matrix<int>>();
            Matrix<int> resultMatrix = new Matrix<int>();
            bool isFirstMatrix = true;
            foreach (Matrix<int> matrix in Source)
            {
                if (isFirstMatrix)
                {
                    resultMatrix = matrix;
                    isFirstMatrix = false;
                    continue;
                }
                MultiplyMatrix(resultMatrix, matrix, Id);
            }
            resultList.Add(resultMatrix);
            Result = resultList;
            IsCalculated = true;
        }

        private void MultiplyMatrix(Matrix<int> resultMatrix, Matrix<int> matrix, string id)
        {
            if (resultMatrix == null)
            {
                throw new ArgumentNullException(nameof(resultMatrix));
            }
            if (matrix == null)
            {
                throw new ArgumentNullException(nameof(matrix));
            }
            if (resultMatrix.Data is null)
            {
                resultMatrix.Data = new int[matrix.Rows, matrix.Cols];
                return;
            }
            else
            {
                if (resultMatrix.Cols != matrix.Rows)
                {
                    throw new FormatException($"Left matrix must has the same number of columns as right matrix has rows in data source {id}");
                }
            }

            Matrix<int> tempMatrix = new Matrix<int>(resultMatrix.Rows, matrix.Cols);
            for (int row = 0; row < tempMatrix.Rows; row++)
            {
                for (int col = 0; col < tempMatrix.Cols; col++)
                {
                    int[] rowArray = resultMatrix.GetRow(row);
                    int[] colArray = matrix.GetCol(col);
                    int intSum = 0;
                    for (int index = 0; index < rowArray.Length; index++)
                    {
                        checked
                        {
                            intSum += rowArray[index] * colArray[index];
                        }
                    }
                    tempMatrix.Data[row, col] = intSum;
                }
            }
            resultMatrix.Data = tempMatrix.Data;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as MultiplyProcessorCommand);
        }

        public bool Equals(MultiplyProcessorCommand other)
        {
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().ToString(), base.GetHashCode());
        }
    }
}