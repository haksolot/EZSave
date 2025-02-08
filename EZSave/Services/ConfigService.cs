using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class ConfigService
    {
        public void SetConfigDestination(string dest, ConfigFileModel config)
        {
            config.ConfFileDestination = dest;
            string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(config.ConfFileDestination, newJson);
        }

        public void SetLogDestination(string dest, ConfigFileModel config)
        {
            config.LogFileDestination = dest;
            string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(config.ConfFileDestination, newJson);
        }

        public bool LoadConfigFile(ConfigFileModel config)
        {
            if (File.Exists(config.ConfFileDestination))
            {
                string json = File.ReadAllText(config.ConfFileDestination);
                config = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SaveJob(JobModel job, ConfigFileModel config)
        {
            if (File.Exists(config.ConfFileDestination))
            {
                string json = File.ReadAllText(config.ConfFileDestination);
                config = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
            }
            else
            {
                config = new ConfigFileModel();
            }

            config.Jobs[job.Name] = job;
            string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(config.ConfFileDestination, newJson);
        }

        public void DeleteJob(JobModel job, ConfigFileModel config)
        {
            if (!File.Exists(config.ConfFileDestination))
            {
                return;
            }
            string json = File.ReadAllText(config.ConfFileDestination);
            ConfigFileModel deserializedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
            deserializedConfig.Jobs.Remove(job.Name);
            string updatedJson = JsonSerializer.Serialize(deserializedConfig, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(config.ConfFileDestination, updatedJson);
        }
    }
}

