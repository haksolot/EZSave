﻿using EZSave.Core.Models;

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
      if (manager.Jobs.Count >= manager.Limit)
      {
        return false;
      }
      else
      {
        manager.Jobs.Add(job);
        return true;
      }
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

    public bool Execute(ManagerModel manager, ConfigFileModel configFileModel)
    {
      if (manager.Jobs.Count > 0)
      {
        foreach (JobModel job in manager.Jobs)
        {
          var service = new JobService();
          var logService = new LogService();
          var statusService = new StatusService();

          service.Start(job, statusService, logService, configFileModel);
        }
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool ExecuteSelected(List<string> listeSelected, ManagerModel manager, ConfigFileModel configFileModel)
    {
      if (!listeSelected.Any() || manager?.Jobs == null || configFileModel == null)
        return false;

      var service = new JobService();
      var logService = new LogService();
      var statusService = new StatusService();

      var jobsToExecute = manager.Jobs.Where(job => listeSelected.Contains(job.Name)).ToList();

      if (jobsToExecute.Count != listeSelected.Count)
        return false;

      foreach (var job in jobsToExecute)
      {
        service.Start(job, statusService, logService, configFileModel);
      }

      return true;
    }

  }
}
