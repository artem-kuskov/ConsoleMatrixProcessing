using System;

namespace ConsoleMatrixProcessing
{
    public class Help
    {
        public static void Show()
        {
            Console.WriteLine(@"This is utility for batch matrix processing.
Command line parameters:
    {<path> | help | -help | /?} [parallelism=<number>] [buffer=<number>]
Where:
    <path>:                 Path to the directory with files contained matrix operation data. 
                            Result files will be saved to the same path.
    help:                   Command to show this help info.
    parallelism=<number>:   Maximum number of parallel threads using for processing. Default parallelism=4.
    buffer=<number>:        Maximum number of batches in memory. Default buffer=100.");
        }
    }
}