using EZSave.Core.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
      //if (manager.Jobs.Count >= manager.Limit)
      //{
      //  return false;
      //}
      //else
      //{
        manager.Jobs.Add(job);
        return true;
      //}
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
          bool check = service.Start(job, statusService, logService, configFileModel);
          if (check == false)
          {
            return false;
          }
        }
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool ExecuteSelected(ObservableCollection<string> listeSelected, ManagerModel manager, ConfigFileModel configFileModel)
    {
            if (listeSelected== null || listeSelected.Count == 0 || manager?.Jobs == null || configFileModel == null)
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
        bool check = service.Start(job, statusService, logService, configFileModel);
        if (check == false)
        {
          return false;
        }
      }
      return true;
    }

        public Thread ExecuteAsThread(JobModel job, ManagerModel managerModel, ConfigFileModel configModel)
        {
            JobService service = new JobService();
            LogService logService = new LogService();   
            StatusService statusService = new StatusService();
            var t = new Thread (() =>
                {
                Thread.CurrentThread.IsBackground = true;
                service.Start(job, statusService, logService, configModel);
            });
            t.Start();
            return t;
        }

        public void PauseThread(Thread thread)
        {
            thread.Suspend(); // à voir wait et notify pour pause et resume
        }
        public void ResumeThread(Thread thread)
        {
            thread.Resume(); // à voir wait et notify pour pause et resume
        }

        public void StopThread(Thread thread)
        {
            thread.Interrupt();
        }
    }
}
