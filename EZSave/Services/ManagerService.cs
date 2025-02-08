using EZSave.Core.Models;

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

        public bool Remove(JobModel job, ManagerModel manager)
        {
            if (manager.Jobs.Contains(job))
            {
                manager.Jobs.Remove(job);
                //Console.WriteLine("The job " + job + " has been deleted from the list !");
                return true;
            }
            else
            {
                //Console.WriteLine("The job " + job + " was not found in the list !");
                return false;
            }
        }

        public bool Execute(ManagerModel manager)
        {
            if (manager.Jobs.Count > 0)
            {
                foreach (JobModel job in manager.Jobs)
                {
                    var service = new JobService();
                    service.Start(job);
                    //Console.WriteLine("The job " + job + " has been started !");
                    
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
