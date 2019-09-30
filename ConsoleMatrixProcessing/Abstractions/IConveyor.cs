using System.Threading.Tasks;

namespace ConsoleMatrixProcessing.Abstractions
{
    public interface IConveyor
    {
        bool IsBuilt { get; }

        void Arrange(string filePath, int parallelism, int queueSize);
        ConveyorCounters GetCounters();
        Task RunAsync();
    }
    public struct ConveyorCounters
    {
        public int filesWrite;
        public int calculatedCommands;
        public int parsedCommands;
        public int filesFound;
        public int filesRead;
        public long elapsedTime;
    }
}