using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class ManagerService
    {
        
        public void Add(Job job)
        {
            if (ManagerModel.Instance.Jobs.Count >= ManagerModel.Instance.limit)
            {
                Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !");
            }
            else
            {
                ManagerModel.Instance.Jobs.Add(job);
                Console.WriteLine("The job " + job + " has been successfully added to the list !");
            }
        }

        public void Remove(Job job)
        {
            if (ManagerModel.Instance.Jobs.Contains(job))
            {
                ManagerModel.Instance.Jobs.Remove(job);
                Console.WriteLine("The job " + job + " has been deleted from the list !");
            }
            else
            {
                Console.WriteLine("The job " + job + " was not found in the list !");
            }
        }

        public void Execute()
        {
            if (ManagerModel.Instance.Jobs.Count > 0)
            {
                foreach (Job job in ManagerModel.Instance.Jobs)
                {
                    job.Start();
                    Console.WriteLine("The job " + job + " has been started !");
                }
            }
            else
            {
                Console.WriteLine("The list is empty !");

            }
        }
    }
}