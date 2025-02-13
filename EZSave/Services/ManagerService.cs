using EZSave.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public void Add(JobModel job, ManagerModel manager)
        {
            manager.Jobs.Add(job);
        }

        public bool RemoveJob(JobModel job, ManagerModel manager)
        {
            var jobToRemove = manager.Jobs.FirstOrDefault(j => j.Name == job.Name);
            if (jobToRemove != null)
            {
                manager.Jobs.Remove(jobToRemove);
                return true;
            }
            return false;
        }

        public async Task<bool> Execute(ManagerModel manager)
        {
            if (manager.Jobs.Count > 0)
            {
                foreach (JobModel job in manager.Jobs)
                {
                    await JobService.Start(job);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> ExecuteSelected(List<string> listeSelected, ManagerModel manager)
        {
            if (!listeSelected.Any() || manager?.Jobs == null)
                return false;

            var jobsToExecute = manager.Jobs.Where(job => listeSelected.Contains(job.Name)).ToList();

            if (!jobsToExecute.Any())
                return false;

            foreach (var job in jobsToExecute)
            {
                await JobService.Start(job);
            }

            return true;
        }
    }
}
