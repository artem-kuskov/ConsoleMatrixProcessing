using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMatrixProcessing.Core
{
    public class BaseProcessorCommand : IProcessorCommand, IEquatable<BaseProcessorCommand>
    {
        private IEnumerable<Matrix<int>> _source;
        public virtual string Id { get; set; }
        public virtual IEnumerable<Matrix<int>> Source
        {
            get => _source;
            set
            {
                IsCalculated = false;
                Result = null;
                _source = value;
            }
        }
        public virtual bool IsCalculated { get; protected set; } = false;
        public virtual IEnumerable<Matrix<int>> Result { get; protected set; }

        public virtual void Calculate()
        {
            if (IsCalculated)
            {
                return;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseProcessorCommand);
        }

        public virtual bool Equals(BaseProcessorCommand other)
        {
            if (other == null ||
                   Id != other.Id ||
                   IsCalculated != other.IsCalculated)
            {
                return false;
            }
            if ((Source is null && other.Source != null) || (Source != null && other.Source is null))
            {
                return false;
            }
            if ((Result is null && other.Result != null) || (Result != null && other.Result is null))
            {
                return false;
            }
            if (Source != null && other.Source != null && !Source.SequenceEqual(other.Source))
            {
                return false;
            }
            if (Result != null && other.Result != null && !Result.SequenceEqual(other.Result))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Source, IsCalculated, Result);
        }
    }
}