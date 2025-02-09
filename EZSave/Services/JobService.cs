using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class JobService
    {
        public void Start(JobModel job, LogService logService, ConfigFileModel configFileModel)
        {
            //logService.Write(new LogModel
            //{
            //    Name = job.Name,
            //    Timestamp = DateTime.Now,
            //    FileSource = "",
            //    FileDestination = "",
            //    FileSize = 0,
            //    FileTransferTime = 0
            //}, configFileModel);

            if (job.Type == "full")
            {
                FullBackup(job, logService, configFileModel);  

            }
            else
            {
                Console.WriteLine("here");
                DifferentialBackup(job, logService, configFileModel); 
            }
        }


        //good
        //public void Start(JobModel job)
        //{


        //    if (job.Type == "full")
        //    {
        //        FullBackup(job);
        //    }
        //    else
        //    {
        //        DifferentialBackup(job);
        //    }
        //}

        private void FullBackup(JobModel job, LogService logService, ConfigFileModel configFileModel)
        {
            long copiedSize = 0; 
            var startTime = DateTime.Now;
            long currentFileSize = 0;
            foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                string? directoryPath = Path.GetDirectoryName(destinationFile);

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.Copy(file, destinationFile, true);
                var endTime = DateTime.Now;

                float transferTime = (float)(endTime - startTime).TotalSeconds;
               

              
                currentFileSize = new FileInfo(destinationFile).Length;
                  
                copiedSize += currentFileSize;

                logService.Write(new LogModel
                {
                    Name = job.Name,
                    Timestamp = DateTime.Now,
                    FileSource = file,
                    FileDestination = destinationFile,
                    FileSize = currentFileSize,
                    FileTransferTime = transferTime
                }, configFileModel);
            }
        }


        

        private void DifferentialBackup(JobModel job, LogService logService, ConfigFileModel configFileModel)
        {
            long copiedSize = 0;
            var startTime = DateTime.Now;
            long currentFileSize = 0;

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
                    var endTime = DateTime.Now;

                    float transferTime = (float)(endTime - startTime).TotalSeconds;
                    currentFileSize = new FileInfo(destinationFile).Length;

                    copiedSize += currentFileSize;

                    logService.Write(new LogModel
                    {
                        Name = job.Name,
                        Timestamp = DateTime.Now,
                        FileSource = file,
                        FileDestination = destinationFile,
                        FileSize = currentFileSize,
                        FileTransferTime = transferTime
                    }, configFileModel);
                }
            }
        }
    }
}

//good
//private void FullBackup(JobModel job)
//{
//    foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
//    {
//        string d = file.Replace(job.Source, job.Destination); //TODO A tester
//        string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
//        string destinationFile = Path.Combine(job.Destination, relativePath);

//        string? directoryPath = Path.GetDirectoryName(destinationFile);

//        if (!string.IsNullOrEmpty(directoryPath))
//        {
//            Directory.CreateDirectory(directoryPath);
//        }
//        /*Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));*/
//        File.Copy(file, destinationFile, true);
//    }
//}