using ConsoleMatrixProcessing.Application.Abstractions;
using System;
using System.Collections.Generic;

namespace ConsoleMatrixProcessing.Services
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private Dictionary<string, string> Config { get; set; }

        public ConfigurationProvider(Dictionary<string, string> config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IEnumerable<string> GetKeys()
        {
            return Config.Keys;
        }

        public string GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            Config.TryGetValue(key, out string value);
            return value;
        }

        public Dictionary<string, string> ToDictionary()
        {
            return Config;
        }

        public override bool Equals(object obj)
        {
            ConfigurationProvider provider = obj as ConfigurationProvider;
            return provider != null &&
                   EqualityComparer<Dictionary<string, string>>.Default.Equals(Config, provider.Config);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Config);
        }
    }
}
