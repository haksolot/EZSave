using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class ManagerService
    {

        public bool Add(JobModel job)
        {
            if (ManagerModel.Jobs.Count >= ManagerModel.Limit)
            {
                return false;
                //Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !");
            }
            else
            {
                ManagerModel.Jobs.Add(job);
                return true;
                //Console.WriteLine("The job " + job + " has been successfully added to the list !");
            }
        }

        public bool Remove(JobModel job)
        {
            if (ManagerModel.Jobs.Contains(job))
            {
                ManagerModel.Jobs.Remove(job);
                //Console.WriteLine("The job " + job + " has been deleted from the list !");
                return true;
            }
            else
            {
                //Console.WriteLine("The job " + job + " was not found in the list !");
                return false;
            }
        }

        public bool Execute()
        {
            if (ManagerModel.Jobs.Count > 0)
            {
                foreach (JobModel job in ManagerModel.Jobs)
                {
                    var service = new JobService();
                    service.Start(job);
                    //Console.WriteLine("The job " + job + " has been started !");
                    return true;
                }
            }
            else
            {
                //Console.WriteLine("The list is empty !");
                return false;
            }
        }
    }
}
