using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core
{
    public class TransposeProcessorCommand : BaseProcessorCommand, IProcessorCommand
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
            foreach (Matrix<int> matrix in Source)
            {
                Matrix<int> resultMatrix = new Matrix<int>(matrix.Cols, matrix.Rows);
                for (int row = 0; row < matrix.Rows; row++)
                {
                    for (int col = 0; col < matrix.Cols; col++)
                    {
                        resultMatrix.Data[col, row] = matrix.Data[row, col];
                    }
                }
                resultList.Add(resultMatrix);
            }
            Result = resultList;
            IsCalculated = true;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as TransposeProcessorCommand);
        }

        public bool Equals(TransposeProcessorCommand other)
        {
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().ToString(), base.GetHashCode());
        }
    }
}