using EZSave.Core.Models;
using System.Runtime.InteropServices;

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
                //Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !");
            }
            else
            {
                manager.Jobs.Add(job);
                return true;
                //Console.WriteLine("The job " + job + " has been successfully added to the list !");
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
                    logService.Write(new LogModel
                    {
                        Name = job.Name,
                        Timestamp = DateTime.Now,
                        FileSource = job.Name,
                        FileSize = 0,
                        FileTransferTime = 0
                    }, configFileModel);

                    service.Start(job, logService, configFileModel);
                    //Console.WriteLine("The job " + job + " has been started !");
                    // Log du début de la sauvegarde
                    logService.Write(new LogModel
                    {
                        Name = job.Name,
                        Timestamp = DateTime.Now,
                        FileSource = "",
                        FileDestination = "",
                        FileSize = 0,
                        FileTransferTime = 0
                    }, configFileModel);

                }
                return true;
            }
            else
            {
                //Console.WriteLine("The list is empty !");
                return false;
            }
        }
    }
}
