using CryptoSoft;
using EZSave.Core.Models;
using System.Diagnostics;

namespace EZSave.Core.Services
{
    public class JobService
    {
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
                check = DifferentialBackup(job, statusService, logService, configFileModel, pauseEvent, cancellationToken);
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
            long copiedSize = 0;
            var startTime = DateTime.Now;
            long currentFileSize = 0;
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
                    FileTransferTime = transferTime,
                    FileCipherTime = cipheringTime,
                }, configFileModel);

                cancellationToken.ThrowIfCancellationRequested();
            }
            return true;
        }

        private bool DifferentialBackup(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            long copiedSize = 0;
            var startTime = DateTime.Now;
            long currentFileSize = 0;

            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories)
                .Select(file => new
                {
                    SourceFile = file,
                    DestinationFile = Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))
                }).Where(f => !File.Exists(f.DestinationFile) || File.GetLastWriteTime(f.SourceFile) > File.GetLastWriteTime(f.DestinationFile))
                .Select(f => f.SourceFile)
                .ToList();

            long totalSize = filesToCopy.Sum(file => new FileInfo(file).Length);

            int totalFiles = filesToCopy.Count;

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
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                long fileSize = new FileInfo(file).Length;
                if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile))
                {
                    pauseEvent.WaitOne();
                    cancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(4000); // Thread.Sleep pour mieux voir le pause et stop

                    string? directoryPath = Path.GetDirectoryName(destinationFile);
                    if (!string.IsNullOrEmpty(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
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
                    using (FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
                    {
                        byte[] buffer = new byte[81920]; 
                        int bytesRead;

                        while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            cancellationToken.ThrowIfCancellationRequested(); 
                            destinationStream.Write(buffer, 0, bytesRead);
                        }
                    }
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
                        FileTransferTime = transferTime,
                        FileCipherTime = cipheringTime,
                    }, configFileModel);
                }

                cancellationToken.ThrowIfCancellationRequested();
            }
            return true;
        }
    }
}