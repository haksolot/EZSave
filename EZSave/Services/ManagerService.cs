using EZSave.Core.Models;
using System.Collections.ObjectModel;

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
            object obj = new object();
            List<Thread> threads = new List<Thread>();
            bool hasFailed = false;

            if (manager.Jobs.Count == 0)
            {
                return false;
            }

            foreach (JobModel job in manager.Jobs)
            {
                Thread thread = new Thread(() =>
                {
                    var service = new JobService();
                    var logService = new LogService();
                    var statusService = new StatusService();
                    bool check = service.Start(job, statusService, logService, configFileModel, job.Name);

                    if (!check)
                    {
                        lock (obj)
                        {
                            hasFailed = true;
                        }
                    }
                });

                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return !hasFailed;
        }

        public bool ExecuteSelected(ObservableCollection<string> listeSelected, ManagerModel manager, ConfigFileModel configFileModel)
        {
            object obj = new object();
            List<Thread> threads = new List<Thread>();
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
                Thread thread = new Thread(() =>
                {
                    bool check = service.Start(job, statusService, logService, configFileModel, job.Name);
                    if (!check)
                    {
                        lock (obj)
                        {
                            hasFailed = true;
                        }
                    }
                });

                thread.Start();
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return !hasFailed;
        }
    }
}