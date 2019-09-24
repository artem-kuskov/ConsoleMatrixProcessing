using System;

namespace ConsoleMatrixProcessing.Core.Models
{
    public class Matrix<T> : IEquatable<Matrix<T>>
    {
        private T[,] _data;
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        public T[,] Data
        {
            get => _data;
            set
            {
                Rows = value?.GetLength(0) ?? 0;
                Cols = value?.GetLength(1) ?? 0;
                _data = value;
            }
        }

        public Matrix()
        {
            Data = null;
        }

        public Matrix(int rows, int cols)
        {
            Data = new T[rows, cols];
        }

        public Matrix(T[,] data)
        {
            Data = data;
        }

        public T[] GetRow(int row)
        {
            if (_data is null)
            {
                return null;
            }
            if (row > Rows - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            T[] result = new T[Cols];
            for (int i = 0; i < Cols; i++)
            {
                result[i] = _data[row, i];
            }
            return result;
        }

        public T[] GetCol(int col)
        {
            if (_data is null)
            {
                return null;
            }
            if (col > Cols - 1)
            {
                throw new ArgumentOutOfRangeException(nameof(col));
            }

            T[] result = new T[Rows];
            for (int i = 0; i < Rows; i++)
            {
                result[i] = _data[i, col];
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Matrix<T>);
        }

        public bool Equals(Matrix<T> other)
        {
            if (other == null ||
               Rows != other.Rows ||
               Cols != other.Cols)
            {
                return false;
            }
            //Default Array equal compare only references
            for (int row = 0; row < Data.GetLength(0); row++)
            {
                for (int col = 0; col < Data.GetLength(1); col++)
                {
                    if (!Data[row, col].Equals(other.Data[row, col]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rows, Cols, Data);
        }
    }
}