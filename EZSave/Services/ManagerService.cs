using EZSave.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;

namespace EZSave.Core.Services
{
  public class ManagerService
  {
    public void Read(ManagerModel manager, ConfigFileModel config)
    {
      foreach (JobModel job in config.Jobs.Values)
      {
        Add(job, manager);
      }
    }

    public bool Add(JobModel job, ManagerModel manager)
    {
      manager.Jobs.Add(job);
      return true;
    }

    public bool RemoveJob(JobModel job, ManagerModel manager)
    {
      for (int i = 0; i < manager.Jobs.Count; i++)
      {
        var jobList = manager.Jobs[i];
        if (jobList.Name == job.Name)
        {
          manager.Jobs.RemoveAt(i);
          return true;
        }
      }
      return false;
    }

    public bool ExecuteSelected(
 Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates,
 ObservableCollection<string> listeSelected,
 ManagerModel manager,
 ConfigFileModel configFileModel,
 string jobToResume )
    {
      object obj = new object();
      bool hasFailed = false;

      if (listeSelected == null || listeSelected.Count == 0 || manager?.Jobs == null || configFileModel == null)
      {
        return false;
      }

      var service = new JobService();
      var logService = new LogService();
      var statusService = new StatusService();

      var jobsToExecute = manager.Jobs.Where(job => listeSelected.Contains(job.Name)).ToList();

      if (jobsToExecute.Count != listeSelected.Count)
        return false;

      foreach (var job in jobsToExecute)
      {
        if (jobStates.TryGetValue(job.Name, out var jobState))
        {
          if (jobState.Status == "Paused" && (jobToResume == job.Name || jobToResume == null))
          {
            jobState.PauseEvent.Set();
            jobStates[job.Name] = (jobState.Thread, jobState.Cts, jobState.PauseEvent, "Running");
            foreach (var kvp in jobStates)
            {
              Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
            }
            Debug.WriteLine($"{job.Name} a repris.");
          }
          continue;
        }

        var cts = new CancellationTokenSource();
        var pauseEvent = new ManualResetEvent(true);

        Thread thread = null;
        thread = new Thread(() =>
        {
          try
          {
            pauseEvent.WaitOne();

            if (!cts.Token.IsCancellationRequested)
            {
              bool success = service.Start(job, statusService, logService, configFileModel, job.Name, pauseEvent, cts.Token);
              if (!success)
              {
                lock (obj)
                {
                  hasFailed = true;
                  jobStates[job.Name] = (thread, cts, pauseEvent, "Stopped");
                  foreach (var kvp in jobStates)
                  {
                    Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
                  }
                }
              }
            }
          }
          catch
          {
            Console.WriteLine("Erreur");
          }
        });
        jobStates[job.Name] = (thread, cts, pauseEvent, "Running");

                thread.Start();
      }
      return !hasFailed;
        }
        
    public bool Pause(string jobName, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
    {
            if (jobName != null)
            {
                if (jobStates.TryGetValue(jobName, out var jobState) && jobState.Status == "Running")
                {
                    jobState.PauseEvent.Reset();
                    jobStates[jobName] = (jobState.Thread, jobState.Cts, jobState.PauseEvent, "Paused");

                    foreach (var kvp in jobStates)
                    {
                        Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
                    }
                    return true;
                }
                return false;
            }
            else
            {
                bool found = false;
                foreach (var kvp in jobStates)
                {
                    if (kvp.Value.Status == "Running")
                    {
                        jobStates[kvp.Key].PauseEvent.Reset();
                        jobStates[kvp.Key] = (kvp.Value.Thread, kvp.Value.Cts, kvp.Value.PauseEvent, "Paused");
                        found = true;
                    }
                }
                foreach (var kvp in jobStates)
                {
                    Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
                }
                return found;
            }
    }

    public bool Stop(string jobName, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
    {
            if (jobName != null)
            {
                if (jobStates.TryGetValue(jobName, out var jobState))
                {
                    jobState.Cts.Cancel();

                    if (jobState.Thread.IsAlive)
                    {
                        jobState.Thread.Join();
                    }
                    jobStates[jobName] = (jobState.Thread, jobState.Cts, jobState.PauseEvent, "Stopped");

                    foreach (var kvp in jobStates)
                    {
                        Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
                    }   
                    return true;
                }
                return false;
            }
            else
                {
                bool found = false;
                foreach (var kvp in jobStates.ToList())
                {
                    if (kvp.Value.Status == "Running" || kvp.Value.Status == "Paused")
                    {
                        kvp.Value.Cts.Cancel();
                        if (kvp.Value.Thread.IsAlive)
                        {
                            kvp.Value.Thread.Join();
                        }
                        jobStates[kvp.Key] = (kvp.Value.Thread, kvp.Value.Cts, kvp.Value.PauseEvent, "Stopped");
                        found = true;
                    }
                }
                foreach (var kvp in jobStates)
                {
                    Debug.WriteLine($"Job: {kvp.Key}, Status: {kvp.Value.Status}");
                }
                return found;
            }
        }
    

  }
}
