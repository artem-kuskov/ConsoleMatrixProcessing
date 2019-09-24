using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Application
{
    public static class SerializeExtensions
    {
        public static bool TryMapTo(this List<string> fromModel, out Matrix<int> toModel)
        {
            if (fromModel == null)
            {
                throw new ArgumentNullException(nameof(fromModel));
            }

            //Matrix contains minimum 1 lines
            if (fromModel.Count == 0)
            {
                toModel = null;
                return false;
            }

            //Calculate array width when filling first row
            if (!fromModel[0].TryMapTo(out int[] rowToModel))
            {
                toModel = null;
                return false;
            }
            int[,] arrToModel = new int[fromModel.Count, rowToModel.Length];
            FillRow(arrToModel, 0, rowToModel);

            //Fill other rows
            for (int rowIndex = 1; rowIndex < fromModel.Count; rowIndex++)
            {
                if (!fromModel[rowIndex].TryMapTo(out rowToModel))
                {
                    toModel = null;
                    return false;
                }
                //Count of columns must be equal in all rows
                if (rowToModel.Length != arrToModel.GetLength(1))
                {
                    toModel = null;
                    return false;
                }
                FillRow(arrToModel, rowIndex, rowToModel);
            }
            toModel = new Matrix<int>(arrToModel);
            return true;
        }

        public static bool TryMapTo(this Matrix<int> fromModel, out IEnumerable<string> toModel)
        {
            if (fromModel == null)
            {
                throw new ArgumentNullException(nameof(fromModel));
            }

            List<string> lstToModel = new List<string>();
            for (int rowIndex = 0; rowIndex < fromModel.Rows; rowIndex++)
            {
                int[] row = fromModel.GetRow(rowIndex);
                lstToModel.Add(string.Join<int>(' ', row));
            }
            toModel = lstToModel;
            return true;
        }

        private static void FillRow(int[,] fillingArray, int rowIndex, int[] rowArray)
        {
            for (int colIndex = 0; colIndex < fillingArray.GetLength(1); colIndex++)
            {
                fillingArray[rowIndex, colIndex] = rowArray[colIndex];
            }
        }
    }
}