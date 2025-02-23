using System.Diagnostics;
using EZSave.Core.Models;
using EZSave.Core.Services;
using CryptoSoft;
using System.Runtime.InteropServices;


namespace EZSave.Core.Services
{
  public class JobService
  {
        private const long FileSizeThreshold = 1024 * 1024;
        public bool Start(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel)
    {
      bool check = ProcessesService.CheckProcess("CalculatorApp");
      if (check == true)
      {
        return false;
      }

      if (job.Type == "full")
      {
        check = FullBackup(job, logService, statusService, configFileModel);
        if (check == false)
        {
          return false;
        }
        else
        {
          return true;
        }
      }
      else if (job.Type == "diff")
      {
        check = DifferentialBackup(job, statusService, logService, configFileModel);
        if (check == false)
        {
          return false;
        }
        else
        {
          return true;
        }
      }
      else
      {
        return false;
      }
    }

    private bool FullBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel)
    {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);
            return CopyFiles(job, filesToCopy, statusService, logService, configFileModel);
        }

    private bool DifferentialBackup(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel)
    {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories)
                      .Where(file =>
                          !File.Exists(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))) ||
                          File.GetLastWriteTime(file) > File.GetLastWriteTime(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))))
                      .ToArray();

            return CopyFiles(job, filesToCopy, statusService, logService, configFileModel);
        }
    private bool CopyFiles(JobModel job, string[] filesToCopy, StatusService statusService, LogService logService, ConfigFileModel configFileModel)
        {
            using var fileLock = new SemaphoreSlim(1, 1); // Verrou local, pas d’attribut !

            long copiedSize = 0;
            long currentFileSize = 0;
            var startTime = DateTime.Now;

            long totalSize = filesToCopy.Sum(file => new FileInfo(file).Length);
            int totalFiles = filesToCopy.Length;

            statusService.SaveStatus(new StatusModel
            {
                Name = job.Name,
                SourceFilePath = job.Source,
                TargetFilePath = job.Destination,
                State = "Activate",
                TotalFilesSize = totalSize,
                TotalFilesToCopy = totalFiles,
                FilesLeftToCopy = totalFiles,
                FilesSizeLeftToCopy = totalSize
            }, configFileModel);

            foreach (string file in filesToCopy)
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                long fileSize = new FileInfo(file).Length;

                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);

                if (fileSize >= FileSizeThreshold)
                {
                    fileLock.Wait();
                }

                float cipheringTime = 0f;
                if (Path.GetExtension(file) == ".crypto")
                {
                    var crypto = new Cipher(file, "key");
                    cipheringTime = crypto.TransformFile(file);
                }

                File.Copy(file, destinationFile, true);

                var endTime = DateTime.Now;
                float transferTime = (float)(endTime - startTime).TotalSeconds;
                currentFileSize = new FileInfo(destinationFile).Length;
                copiedSize += currentFileSize;

                totalSize -= fileSize;
                totalFiles--;

                statusService.SaveStatus(new StatusModel
                {
                    Name = job.Name,
                    SourceFilePath = job.Source,
                    TargetFilePath = job.Destination,
                    State = "End",
                    TotalFilesSize = totalSize + fileSize,
                    TotalFilesToCopy = totalFiles + 1,
                    FilesLeftToCopy = totalFiles,
                    FilesSizeLeftToCopy = totalSize
                }, configFileModel);

                logService.Write(new LogModel
                {
                    Name = job.Name,
                    Timestamp = DateTime.Now,
                    FileSource = file,
                    FileDestination = destinationFile,
                    FileSize = currentFileSize,
                    FileTransferTime = transferTime,
                    FileCipherTime = cipheringTime,
                }, configFileModel);

                if (fileSize >= FileSizeThreshold)
                {
                    fileLock.Release();
                }
            }

            return true;
        }
    }
}
