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

    public bool LoadConfigFile(ConfigModel conf)
    {
      if (!File.Exists(conf.ConfFileDestination))
      {
        return false;
      }
      string data = File.ReadAllText(conf.ConfFileDestination);
      using JsonDocument doc = JsonDocument.Parse(data);
      JsonElement root = doc.RootElement;

      foreach (JsonProperty property in root.EnumerateObject())
      {
        if (property.Name == "ConfFileDestination")
        {
          SetConfigDestination(property.Value.GetString()!, conf);
        }
        else if (property.Name == "LogFileDestination")
        {
          SetLogDestination(property.Value.GetString()!, conf);
        }
        else if (property.Name == "Jobs")
        {

        }
        else
        {

        }
      }
      return true;
    }


    public void SaveJob(JobModel job, ConfigModel conf)
    {
      List<JobModel> jobs = new List<JobModel>();
      if (File.Exists(conf.ConfFileDestination))
      {
        string json = File.ReadAllText(conf.ConfFileDestination);
        jobs = JsonSerializer.Deserialize<List<JobModel>>(json) ?? new List<JobModel>();
      }
      JobModel? existingJob = jobs.FirstOrDefault(j => j.Name == job.Name);
      if (existingJob != null)
      {
        jobs.Remove(existingJob);
      }

      jobs.Add(job);

      string newJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfFileDestination, newJson);
    }

    public void DeleteJob(JobModel job, ConfigModel conf)
    {
      if (!File.Exists(conf.ConfFileDestination))
      {
        throw new FileNotFoundException("The config file does not seem to be here");
      }
      string json = File.ReadAllText(conf.ConfFileDestination);
      List<JobModel> jobs = JsonSerializer.Deserialize<List<JobModel>>(json) ?? new List<JobModel>();
      int initialCount = jobs.Count;
      jobs.RemoveAll(j => j.Name == job.Name);
      if (jobs.Count == initialCount)
      {
        throw new InvalidOperationException($"The Job does not exist");
      }

      // Réécriture du fichier après suppression du job
      string updatedJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfFileDestination, updatedJson);
    }
  }
}

