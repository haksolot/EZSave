/*using System;*/
/*using System.Collections.Generic;*/
/*using System.IO;*/
/*using System.Linq;*/
using System.Text.Json;

public class Job
{
  public string Name { get; }
  public string Source { get; }
  public string Destination { get; }
  public string Type { get; }

  public Job(string name, string source, string destination, string type)
  {
    Name = name;
    Source = source;
    Destination = destination;

    if (type.ToLower() == "full" || type.ToLower() == "differential")
    {
      Type = type.ToLower();
    }
    else
    {
      throw new ArgumentException("The backup type should be 'full' or 'differential'.");
    }
  }

  public void start()
  {
    if (Type == "full")
    {
      PerformFullBackup();
    }
    else
    {
      PerformDifferentialBackup();
    }
  }

  public void save(string ConfigFile)
  {
    List<Job> jobs = new List<Job>();
    if (File.Exists(ConfigFile))
    {
      string json = File.ReadAllText(ConfigFile);
      jobs = JsonSerializer.Deserialize<List<Job>>(json) ?? new List<Job>();
    }
    Job? existingJob = jobs.FirstOrDefault(j => j.Name == Name);
    if (existingJob != null)
    {
      jobs.Remove(existingJob);
    }

    jobs.Add(this);

    string newJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(ConfigFile, newJson);
  }

  public void delete(string ConfigFile)
  {
    if (!File.Exists(ConfigFile))
    {
      throw new FileNotFoundException("The config file does not seem to be here");
    }
    string json = File.ReadAllText(ConfigFile);
    List<Job> jobs = JsonSerializer.Deserialize<List<Job>>(json) ?? new List<Job>();
    int initialCount = jobs.Count;
    jobs.RemoveAll(j => j.Name == Name);
    if (jobs.Count == initialCount)
    {
      throw new InvalidOperationException($"The Job does not exist");
    }

    // Réécriture du fichier après suppression du job
    string updatedJson = JsonSerializer.Serialize(jobs, new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(ConfigFile, updatedJson);
  }

  private void PerformFullBackup()
  {
    foreach (string file in Directory.GetFiles(Source, "*", SearchOption.AllDirectories))
    {
      string relativePath = file.Substring(Source.Length).TrimStart(Path.DirectorySeparatorChar);
      string destinationFile = Path.Combine(Destination, relativePath);

      Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
      File.Copy(file, destinationFile, true);
    }
  }

  private void PerformDifferentialBackup()
  {
    foreach (string file in Directory.GetFiles(Source, "*", SearchOption.AllDirectories))
    {
      string relativePath = file.Substring(Source.Length).TrimStart(Path.DirectorySeparatorChar);
      string destinationFile = Path.Combine(Destination, relativePath);
      if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile))
      {
        Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
        File.Copy(file, destinationFile, true);
      }
    }
  }
}
