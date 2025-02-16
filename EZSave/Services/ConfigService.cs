using System.Collections.ObjectModel;
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
     if (!string.IsNullOrEmpty(config.ConfFileDestination) && File.Exists(config.ConfFileDestination))
     {
         string json = File.ReadAllText(config.ConfFileDestination);
         ConfigFileModel? tempConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
         if (tempConfig == null)
         {
             return false;
         }
         config.ConfFileDestination = tempConfig.ConfFileDestination;
         config.LogFileDestination = tempConfig.LogFileDestination;
         config.Jobs = tempConfig.Jobs != null ? new Dictionary<string, JobModel>(tempConfig.Jobs) : new Dictionary<string, JobModel>();
         return true;
     }
     else
     {
         return false;
     }
 }



        public bool SaveJob(JobModel job, ConfigFileModel config)
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

            config.Jobs ??= new Dictionary<string, JobModel>();
            config.Jobs[job.Name] = job;
            string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

            if (!string.IsNullOrEmpty(config.ConfFileDestination))
            {
                File.WriteAllText(config.ConfFileDestination, newJson);
                return true;
            }
            return false;
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
