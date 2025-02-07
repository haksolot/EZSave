using System.Text.Json;
using EZSave.Core.Models;

namespace EZSave.Core.Services
{
  public class ConfigService
  {
    public void SetConfigDestination(string dest, ConfigModel conf)
    {
      conf.ConfDestination = dest;
    }

    public void SetLogDestination(string dest, ConfigModel conf)
    {
      conf.LogDestination = dest;
    }

    public void SaveJob(JobModel job, ConfigModel conf)
    {
      List<JobModel> jobs = new List<JobModel>();
      if (File.Exists(conf.ConfDestination))
      {
        string json = File.ReadAllText(conf.ConfDestination);
        jobs = JsonSerializer.Deserialize<List<JobModel>>(json) ?? new List<JobModel>();
      }
      JobModel? existingJob = jobs.FirstOrDefault(j => j.Name == job.Name);
      if (existingJob != null)
      {
        jobs.Remove(existingJob);
      }

      jobs.Add(job);

      string newJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfDestination, newJson);
    }

    public void DeleteJob(JobModel job, ConfigModel conf)
    {
      if (!File.Exists(conf.ConfDestination))
      {
        throw new FileNotFoundException("The config file does not seem to be here");
      }
      string json = File.ReadAllText(conf.ConfDestination);
      List<JobModel> jobs = JsonSerializer.Deserialize<List<JobModel>>(json) ?? new List<JobModel>();
      int initialCount = jobs.Count;
      jobs.RemoveAll(j => j.Name == job.Name);
      if (jobs.Count == initialCount)
      {
        throw new InvalidOperationException($"The Job does not exist");
      }

      // Réécriture du fichier après suppression du job
      string updatedJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText(conf.ConfDestination, updatedJson);
    }
  }
}

