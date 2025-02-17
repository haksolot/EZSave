using EZSave.Core.Models;
using System.Text.Json;

namespace EZSave.Core.Services
{
  /*<<<<<<< HEAD*/
  //  public class ConfigService
  //  {

  //    public void SaveConfigFile(ConfigFileModel config)
  //    {
  //      if (string.IsNullOrEmpty(config.ConfFileDestination))
  //      {
  //        return;
  //      }
  //      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
  //      File.WriteAllText(config.ConfFileDestination, newJson);
  //    }

  //    public void EnsureConfigFileExists(ConfigFileModel config)
  //    {
  //      if (string.IsNullOrEmpty(config.ConfFileDestination))
  //      {
  //        return;
  //      }

  //      if (!File.Exists(config.ConfFileDestination))
  //      {
  //        SaveConfigFile(new ConfigFileModel { ConfFileDestination = config.ConfFileDestination });
  //      }
  //    }

  //    public void SetConfigDestination(string dest, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);
  //      LoadConfigFile(config);
  //      config.ConfFileDestination = dest;
  //      SaveConfigFile(config);
  //    }

  //    public void SetLogDestination(string dest, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);
  //      LoadConfigFile(config);
  //      config.LogFileDestination = dest;
  //      SaveConfigFile(config);
  //    }

  //    public void SetLogType(string type, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);
  //      LoadConfigFile(config);
  //      config.LogType = type;
  //      SaveConfigFile(config);
  //    }

  //    public void SetStatusDestination(string dest, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);
  //      LoadConfigFile(config);
  //      config.StatusFileDestination = dest;
  //      SaveConfigFile(config);
  //    }

  //    public bool LoadConfigFile(ConfigFileModel config)
  //    {
  //      if (string.IsNullOrEmpty(config.ConfFileDestination))
  //      {
  //        /*Console.WriteLine("Erreur: Chemin de configuration invalide.");*/
  //        return false;
  //      }

  //      if (!File.Exists(config.ConfFileDestination))
  //      {
  //        /*Console.WriteLine("Fichier de configuration introuvable. Création d'un nouveau fichier...");*/
  //        config = new ConfigFileModel(); // On réinitialise la config
  //        SaveConfigFile(config); // On le sauvegarde immédiatement
  //        return true;
  //      }

  //      string json = File.ReadAllText(config.ConfFileDestination);
  //      ConfigFileModel? tempConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();

  //      config.ConfFileDestination = tempConfig.ConfFileDestination;
  //      config.LogFileDestination = tempConfig.LogFileDestination;
  //      config.LogType = tempConfig.LogType;
  //      config.StatusFileDestination = tempConfig.StatusFileDestination;
  //      config.Jobs = tempConfig.Jobs != null ? new Dictionary<string, JobModel>(tempConfig.Jobs) : new Dictionary<string, JobModel>();

  //      return true;
  //    }

  //    public bool SaveJob(JobModel job, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);  // Vérifie et crée le fichier si nécessaire
  //      if (!LoadConfigFile(config))
  //      {
  //        return false;
  //      }

  //      config.Jobs ??= new Dictionary<string, JobModel>();
  //      config.Jobs[job.Name] = job;

  //      string newJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

  //      if (!string.IsNullOrEmpty(config.ConfFileDestination))
  //      {
  //        File.WriteAllText(config.ConfFileDestination, newJson);
  //        return true;
  //      }
  //      return false;
  //    }

  //    public void DeleteJob(JobModel job, ConfigFileModel config)
  //    {
  //      EnsureConfigFileExists(config);  // Vérifie et crée le fichier si nécessaire

  //      if (!string.IsNullOrEmpty(config.ConfFileDestination) && File.Exists(config.ConfFileDestination))
  //      {
  //        string json = File.ReadAllText(config.ConfFileDestination);
  //        ConfigFileModel deserializedConfig = JsonSerializer.Deserialize<ConfigFileModel>(json) ?? new ConfigFileModel();

  //        deserializedConfig.Jobs?.Remove(job.Name);

  //        string updatedJson = JsonSerializer.Serialize(deserializedConfig, new JsonSerializerOptions { WriteIndented = true });
  //        File.WriteAllText(config.ConfFileDestination, updatedJson);
  //      }
  //    }
  //  }
  //}


  /*=======*/
  public class ConfigService
  {
    public void SetConfigDestination(string dest, ConfigFileModel config)
    {
      EnsureConfigFileExists(config);
      LoadConfigFile(config);
      config.ConfFileDestination = dest;
      SaveConfigFile(config);
    }

    public void SetLogDestination(string dest, ConfigFileModel config)
    {
      EnsureConfigFileExists(config);
      LoadConfigFile(config);
      config.LogFileDestination = dest;
      SaveConfigFile(config);
    }

    public void SetLogType(string type, ConfigFileModel config)
    {
      EnsureConfigFileExists(config);
      LoadConfigFile(config);
      config.LogType = type;
      SaveConfigFile(config);
    }

    public void SetStatusDestination(string dest, ConfigFileModel config)
    {
      EnsureConfigFileExists(config);
      LoadConfigFile(config);
      config.StatusFileDestination = dest;
      SaveConfigFile(config);
    }
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
/*>>>>>>> MergeGUIs*/
