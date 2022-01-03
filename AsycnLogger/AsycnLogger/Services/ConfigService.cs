using System.IO;
using Newtonsoft.Json;

namespace AsyncLogger
{
    public class ConfigService : IConfigService
    {
        public Config GetConfig()
        {
            var configFile = File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeObject<Config>(configFile);
            return config;
        }
    }
}
