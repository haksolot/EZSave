using CryptoSoft;
using EZSave.Core.Models;
using System.Diagnostics;

namespace EZSave.Core.Services
{
    public class JobService
    {
        private const long FileSizeThreshold = 1024 * 1024;

        public bool Start(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel, string name, ManualResetEvent pauseEvent, CancellationToken cancellationToken) // TODO Enlever le param name de la fonction et vérifier les appels

        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            Debug.WriteLine(name);
            bool check = ProcessesService.CheckProcess("CalculatorApp");
            if (check == true)
            {
                return false;
            }

            if (job.Type == "full")
            {
                check = FullBackup(job, logService, statusService, configFileModel, pauseEvent, cancellationToken);
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
                check = DifferentialBackup(job, logService, statusService,  configFileModel, pauseEvent, cancellationToken);
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

        private bool FullBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);
            return CopyFiles(job, filesToCopy, logService, statusService, configFileModel, pauseEvent, cancellationToken);
        }

        private bool DifferentialBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories)
                      .Where(file =>
                          !File.Exists(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))) ||
                          File.GetLastWriteTime(file) > File.GetLastWriteTime(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))))
                      .ToArray();

            return CopyFiles(job, filesToCopy, logService, statusService,  configFileModel, pauseEvent, cancellationToken);
        }
        private bool CopyFiles(JobModel job,string[]FilesToCopy, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            using var fileLock = new SemaphoreSlim(1, 1); 

            long copiedSize = 0;
            long currentFileSize = 0;
            var startTime = DateTime.Now;

            long totalSize = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Sum(file => new FileInfo(file).Length);
            int totalFiles = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Length;

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

            foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
            {
                if (cancellationToken.IsCancellationRequested) return false;
                pauseEvent.WaitOne();
                cancellationToken.ThrowIfCancellationRequested();

                Thread.Sleep(4000); // Thread.Sleep pour mieux voir le pause et stop

                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                string? directoryPath = Path.GetDirectoryName(destinationFile);
                long fileSize = new FileInfo(file).Length;

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
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
                else
                {
                    cipheringTime = 0;
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

                cancellationToken.ThrowIfCancellationRequested();

                if (fileSize >= FileSizeThreshold)
                {
                    fileLock.Release();
                }
            }
            return true;
        }
    }
}