﻿using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
  public class ConfigService
  {
    public void SetConfigDestination(string dest, ConfigFileModel config)
    {
      config.ConfFileDestination = dest;
      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      if (!string.IsNullOrEmpty(config.ConfFileDestination))
      {
        File.WriteAllText(config.ConfFileDestination, newJson);
      }
    }

    public void SetLogDestination(string dest, ConfigFileModel config)
    {
      config.LogFileDestination = dest;
      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      if (!string.IsNullOrEmpty(config.ConfFileDestination))
      {
        File.WriteAllText(config.ConfFileDestination, newJson);
      }
    }

    public void SetLogType(string type, ConfigFileModel config)
    {
      config.LogType = type;
      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      if (!string.IsNullOrEmpty(config.ConfFileDestination))
      {
        File.WriteAllText(config.ConfFileDestination, newJson);
      }
    }

    public void SetStatusDestination(string dest, ConfigFileModel config)
    {
      config.StatusFileDestination = dest;
      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
      if (!string.IsNullOrEmpty(config.ConfFileDestination))
      {
        File.WriteAllText(config.ConfFileDestination, newJson);
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


    public void DeleteJob(JobModel job, ConfigFileModel config)
    {
      if (!string.IsNullOrEmpty(config.ConfFileDestination) && File.Exists(config.ConfFileDestination))
      {
        string json = File.ReadAllText(config.ConfFileDestination);
        ConfigFileModel deserializedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();
        deserializedConfig.Jobs?.Remove(job.Name);
        string updatedJson = JsonSerializer.Serialize(deserializedConfig, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(config.ConfFileDestination, updatedJson);
      }
    }
  }
}

