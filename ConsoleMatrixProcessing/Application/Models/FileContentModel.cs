using System;
using System.Linq;

namespace ConsoleMatrixProcessing.Application.Models
{
    public class DataContentModel : IEquatable<DataContentModel>
    {
        public string FilePath { get; set; }
        public string[] ContentStrings { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DataContentModel);
        }

        public bool Equals(DataContentModel other)
        {
            if (other == null || FilePath != other.FilePath)
            {
                return false;
            }
            if ((ContentStrings is null && other.ContentStrings != null) ||
                (ContentStrings != null && other.ContentStrings is null))
            {
                return false;
            }
            if (ContentStrings != null && other.ContentStrings != null &&
                !ContentStrings.SequenceEqual(other.ContentStrings))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FilePath, ContentStrings);
        }
    }
}