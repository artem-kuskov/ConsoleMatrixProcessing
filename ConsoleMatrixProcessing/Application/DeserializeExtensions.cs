using ConsoleMatrixProcessing.Application.Models;
using ConsoleMatrixProcessing.Core;
using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleMatrixProcessing.Application
{
    public static class DeserializeExtensions
    {
        public static bool TryMapTo(this DataContentModel fromModel, out IProcessorCommand toModel)
        {
            if (fromModel == null)
            {
                throw new ArgumentNullException(nameof(fromModel));
            }

            //Right command contains minimum 3 lines: operation, empty and matrix
            if (fromModel.ContentStrings.Length < 3
                || !string.IsNullOrWhiteSpace(fromModel.ContentStrings[1])
                || string.IsNullOrWhiteSpace(fromModel.ContentStrings[2]))
            {
                //It is not command file, skip
                toModel = null;
                return false;
            }

            string strOperation = fromModel.ContentStrings[0];
            if (!Enum.TryParse(strOperation, true, out ProcessorCommandFabric.Operator operation))
            {
                //It is not command file, skip
                toModel = null;
                return false;
            }

            IProcessorCommand processor = ProcessorCommandFabric.GetProcessor(operation);
            if (!fromModel.TryMapTo(out IEnumerable<Matrix<int>> matrices))
            {
                //It is a command file, but wrong data, return BadCommand
                toModel = ProcessorCommandFabric.GetBadProcessor(fromModel.FilePath);
                return true;
            }
            processor.Source = matrices;
            processor.Id = fromModel.FilePath;
            toModel = processor;
            return true;
        }

        public static bool TryMapTo(this DataContentModel fromModel, out IEnumerable<Matrix<int>> toModel)
        {
            if (fromModel == null)
            {
                throw new ArgumentNullException(nameof(fromModel));
            }

            //Right command contains minimum 3 lines: operation, empty and matrix
            if (fromModel.ContentStrings.Length < 3
                || string.IsNullOrWhiteSpace(fromModel.ContentStrings[0])
                || !string.IsNullOrWhiteSpace(fromModel.ContentStrings[1])
                || string.IsNullOrWhiteSpace(fromModel.ContentStrings[2]))
            {
                toModel = null;
                return false;
            }

            List<string> serializedMatrix = new List<string>();
            List<Matrix<int>> lstMatrices = new List<Matrix<int>>();
            for (int index = 2; index < fromModel.ContentStrings.Length; index++)
            {
                if (string.IsNullOrWhiteSpace(fromModel.ContentStrings[index]))
                {
                    continue;
                }
                serializedMatrix.Add(fromModel.ContentStrings[index]);

                //If this string is last or next string is empty, convert collection to matrix
                if (index == fromModel.ContentStrings.Length - 1
                    || string.IsNullOrWhiteSpace(fromModel.ContentStrings[index + 1]))
                {
                    if (serializedMatrix.TryMapTo(out Matrix<int> matrix))
                    {
                        lstMatrices.Add(matrix);
                        serializedMatrix = new List<string>();
                        continue;
                    }
                    toModel = null;
                    return false;
                }
            }
            toModel = lstMatrices;
            return true;
        }

        public static bool TryMapTo(this IProcessorCommand fromModel, out DataContentModel toModel)
        {
            if (fromModel == null)
            {
                throw new ArgumentNullException(nameof(fromModel));
            }

            if (fromModel is BadProcessorCommand || !fromModel.IsCalculated)
            {
                toModel = null;
                return false;
            }
            toModel = new DataContentModel
            {
                FilePath = fromModel.Id + "_result.txt"
            };
            List<string> lstToModel = new List<string>();
            bool firstMatrix = true;
            foreach (Matrix<int> matrix in fromModel.Result)
            {
                if (!firstMatrix)
                {
                    lstToModel.Add(string.Empty);
                }
                if (!matrix.TryMapTo(out IEnumerable<string> lstMatrix))
                {
                    toModel = null;
                    return false;
                }
                lstToModel.AddRange(lstMatrix);
                firstMatrix = false;
            }
            toModel.ContentStrings = lstToModel.ToArray();
            return true;
        }

        public static bool TryMapTo(this string fromModel, out int[] toModel)
        {
            if (string.IsNullOrWhiteSpace(fromModel))
            {
                toModel = null;
                return false;
            }

            bool hasErrors = false;
            int[] arrToModel = fromModel
                .Trim()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(str =>
                {
                    if (!int.TryParse(str, out int result))
                    {
                        hasErrors = true;
                    };
                    return result;
                })
                .ToArray();
            if (hasErrors || arrToModel.Length == 0)
            {
                toModel = null;
                return false;
            }
            toModel = arrToModel;
            return true;
        }
    }
}