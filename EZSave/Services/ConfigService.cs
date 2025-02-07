using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
  public class ConfigService
  {
    public void SetConfigDestination(string dest, ConfigModel conf)
    {
      conf.ConfFileDestination = dest;
    }

    public void SetLogDestination(string dest, ConfigModel conf)
    {
      conf.LogFileDestination = dest;
    }

    public bool LoadConfigFile(ConfigModel conf, ConfigFileModel config)
    {
      if (File.Exists(conf.ConfFileDestination))
      {
        string json = File.ReadAllText(conf.ConfFileDestination);
        config = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
        return true;
      }
      else
      {
        return false;
      }
    }

    public void SaveJob(JobModel job, ConfigModel conf)
    {
      ConfigFileModel config;

      if (File.Exists(conf.ConfFileDestination))
      {
        string json = File.ReadAllText(conf.ConfFileDestination);
        config = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
      }
      else
      {
        config = new ConfigFileModel();
      }

      config.Jobs[job.Name] = job;
      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfFileDestination, newJson);
    }

    public void DeleteJob(JobModel job, ConfigModel conf)
    {
      if (!File.Exists(conf.ConfFileDestination))
      {
        return;
      }
      string json = File.ReadAllText(conf.ConfFileDestination);
      ConfigFileModel config = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
      config.Jobs.Remove(job.Name);
      string updatedJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfFileDestination, updatedJson);
    }
  }
}

