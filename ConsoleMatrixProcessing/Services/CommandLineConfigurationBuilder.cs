using System.Collections.Generic;
using System.Linq;

namespace ConsoleMatrixProcessing.Services
{
    public class CommandLineConfigurationBuilder
    {
        private string[] Args { get; }

        public CommandLineConfigurationBuilder(string[] args)
        {
            Args = args;
        }

        public ConfigurationProvider BuildConfigurationProvider()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (Args is null || Args.Length == 0)
            {
                return new ConfigurationProvider(dict);
            }
            if (!string.IsNullOrWhiteSpace(Args[0]))
            {
                if (Args[0].ToLower() == "help" ||
                    Args[0] == "/?" ||
                    Args[0].ToLower() == "-h")
                {
                    dict["help"] = "show";
                }
                else
                {
                    dict["path"] = Args[0];
                }
            }
            foreach (string arg in Args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }
                string[] keyPair = arg.Split('=').ToArray();
                if (keyPair.Length != 2)
                {
                    continue; ;
                }
                dict[keyPair[0].Trim().ToLower()] = keyPair[1].Trim();
            }
            return new ConfigurationProvider(dict);
        }
    }
}