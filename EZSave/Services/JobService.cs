using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class JobService
    {
        public void Start(JobModel job)
        {
            if (job.Type == "full")
            {
                FullBackup(job);
            }
            else
            {
                DifferentialBackup(job);
            }
        }

        private void FullBackup(JobModel job)
        {
            foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
            {
                string d = file.Replace(job.Source, job.Destination); //TODO A tester
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);

                string? directoryPath = Path.GetDirectoryName(destinationFile);

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                /*Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));*/
                File.Copy(file, destinationFile, true);
            }
        }

        private void DifferentialBackup(JobModel job)
        {
            foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile))
                {
                    string? directoryPath = Path.GetDirectoryName(destinationFile);

                    if (!string.IsNullOrEmpty(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    /*Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));*/
                    File.Copy(file, destinationFile, true);
                }
            }
        }
    }
}
