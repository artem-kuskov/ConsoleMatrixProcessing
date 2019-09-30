using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core
{
    public class SubtractProcessorCommand : BaseProcessorCommand, IProcessorCommand
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
                SubMatrix(resultMatrix, matrix, Id);
            }
            resultList.Add(resultMatrix);
            Result = resultList;
            IsCalculated = true;
        }

        private void SubMatrix(Matrix<int> resultMatrix, Matrix<int> matrix, string id)
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
            }
            else
            {
                if (resultMatrix.Cols != matrix.Cols || resultMatrix.Rows != matrix.Rows)
                {
                    throw new FormatException($"Subtracking matrices have different size in data source {id}");
                }
            }

            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int col = 0; col < matrix.Cols; col++)
                {
                    checked
                    {
                        resultMatrix.Data[row, col] -= matrix.Data[row, col];
                    }
                }
            }
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as SubtractProcessorCommand);
        }

        public bool Equals(SubtractProcessorCommand other)
        {
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().ToString(), base.GetHashCode());
        }
    }
}