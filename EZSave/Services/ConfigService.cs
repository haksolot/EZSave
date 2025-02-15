using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class ConfigService
    {
        public void SaveConfigFile(ConfigFileModel config)
        {
            if (string.IsNullOrEmpty(config.ConfFileDestination))
            {
                return;
            }

            string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(config.ConfFileDestination, newJson);
        }

        public void EnsureConfigFileExists(ConfigFileModel config)
        {
            if (string.IsNullOrEmpty(config.ConfFileDestination))
            {
                return;
            }

            if (!File.Exists(config.ConfFileDestination))
            {
                SaveConfigFile(new ConfigFileModel { ConfFileDestination = config.ConfFileDestination });
            }
        }

        public bool LoadConfigFile(ConfigFileModel config)
        {
            if (string.IsNullOrEmpty(config.ConfFileDestination) || !File.Exists(config.ConfFileDestination))
            {
                return false;
            }

            string json = File.ReadAllText(config.ConfFileDestination);
            ConfigFileModel? tempConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();

            config.ConfFileDestination = tempConfig.ConfFileDestination;
            config.LogFileDestination = tempConfig.LogFileDestination;
            config.LogType = tempConfig.LogType;
            config.StatusFileDestination = tempConfig.StatusFileDestination;
            config.Jobs = tempConfig.Jobs != null ? new Dictionary<string, JobModel>(tempConfig.Jobs) : new Dictionary<string, JobModel>();

            return true;
        }

        public bool SaveJob(JobModel job, ConfigFileModel config)
        {
            EnsureConfigFileExists(config);

            if (!LoadConfigFile(config))
            {
                return false;
            }

            config.Jobs ??= new Dictionary<string, JobModel>();
            config.Jobs[job.Name] = job;

            SaveConfigFile(config);
            return true;
        }

        public bool DeleteJob(string jobName, ConfigFileModel config)
        {
            EnsureConfigFileExists(config);

            if (!LoadConfigFile(config))
            {
                return false;
            }

            if (config.Jobs != null && config.Jobs.ContainsKey(jobName))
            {
                config.Jobs.Remove(jobName);
                SaveConfigFile(config);
                return true;
            }

            return false;
        }
    }
}
