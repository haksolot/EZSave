using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class ManagerService
    {
        private readonly ManagerModel _managerModel;

        public ManagerService()
        {
            _managerModel = ManagerModel.Instance;
        }

        public void Add(Job job)
        {
            if (_managerModel.jobs.Count >= _managerModel.limit)
            {
                Console.WriteLine("You have exceeded the maximum number of allowed backups (max 5 jobs) !");
            }
            else
            {
                _managerModel.jobs.Add(job);
                Console.WriteLine("The job " + job + " has been successfully added to the list !");
            }
        }

        public void Remove(Job job)
        {
            if (_managerModel.jobs.Contains(job))
            {
                _managerModel.jobs.Remove(job);
                Console.WriteLine("The job " + job + " has been deleted from the list !");
            }
            else
            {
                Console.WriteLine("The job " + job + " was not found in the list !");
            }
        }

        public void Execute()
        {
            if (_managerModel.jobs.Count > 0)
            {
                foreach (Job job in _managerModel.jobs)
                {
                    job.start();
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