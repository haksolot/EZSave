using EZSave.Core.Models;
using System.Diagnostics;
using System.IO;

namespace EZSave.Core.Services
{
    public static class JobService
    {
        public static void ExecuteJob(JobModel job)
        {
            if (Directory.Exists(job.Source) && Directory.Exists(job.Destination))
            {
                // Simulation de l'exécution du job
                Debug.WriteLine($"Exécution du job: {job.Name}");
            }
        }
    }
}
