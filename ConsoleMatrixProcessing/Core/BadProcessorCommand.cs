using ConsoleMatrixProcessing.Core.Abstractions;
using ConsoleMatrixProcessing.Core.Models;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Core
{
    public class BadProcessorCommand : BaseProcessorCommand, IProcessorCommand
    {
        public override void Calculate()
        {
            IsCalculated = false;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BadProcessorCommand);
        }

        public bool Equals(BadProcessorCommand other)
        {
            return base.Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().ToString(), base.GetHashCode());
        }
    }
}