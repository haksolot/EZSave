using CryptoSoft;
using EZSave.Core.Models;
using System.Diagnostics;

namespace EZSave.Core.Services
{
    public class JobService
    {
        private const long FileSizeThreshold = 1024 * 1024;

        public bool Start(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel, string name, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            Debug.WriteLine(name);
            bool check = ProcessesService.CheckProcess("CalculatorApp");
            if (check)
            {
                return false;
            }

            if (job.Type == "full")
            {
                check = FullBackup(job, logService, statusService, configFileModel, pauseEvent, cancellationToken);
                return check;
            }
            else if (job.Type == "diff")
            {
                check = DifferentialBackup(job, logService, statusService, configFileModel, pauseEvent, cancellationToken);
                return check;
            }
            else
            {
                return false;
            }
        }

        public bool HasPendingPriorityFiles(JobModel job)
        {
            try
            {
                if (!Directory.Exists(job.Source))
                {
                    Debug.WriteLine("[DEBUG] Le dossier source n'existe pas !");
                    return false;
                }

                var prioFiles = Directory.GetFiles(job.Source, "*.prio", SearchOption.AllDirectories);
                bool hasPrio = prioFiles.Any();

                if (hasPrio)
                {
                    Debug.WriteLine($"[ALERTE] {prioFiles.Length} fichiers .prio détectés ! Exécution en priorité après pause.");
                }

                return hasPrio;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ERREUR] Impossible de vérifier les fichiers .prio : {ex.Message}");
                return false;
            }
        }

        private bool FullBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);
            filesToCopy = SortPriorityFilesFirst(filesToCopy);

            
            if (HasPendingPriorityFiles(job))
            {
                Debug.WriteLine("[INFO] Pause de 5 secondes pour signaler que des fichiers .prio vont être exécutés en priorité.");
                Thread.Sleep(5000);
            }

            return CopyFiles(job, filesToCopy, logService, statusService, configFileModel, pauseEvent, cancellationToken);
        }

        private bool DifferentialBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories)
                      .Where(file =>
                          !File.Exists(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))) ||
                          File.GetLastWriteTime(file) > File.GetLastWriteTime(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))))
                      .ToArray();

            filesToCopy = SortPriorityFilesFirst(filesToCopy);

            
            if (HasPendingPriorityFiles(job))
            {
                Debug.WriteLine("[INFO] Pause de 5 secondes pour signaler que des fichiers .prio vont être exécutés en priorité.");
                Thread.Sleep(5000);
            }

            return CopyFiles(job, filesToCopy, logService, statusService, configFileModel, pauseEvent, cancellationToken);
        }

        private bool CopyFiles(JobModel job, string[] filesToCopy, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken)
        {
            using var fileLock = new SemaphoreSlim(1, 1);

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
                if (cancellationToken.IsCancellationRequested) return false;
                pauseEvent.WaitOne();
                cancellationToken.ThrowIfCancellationRequested();

                Thread.Sleep(4000);

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

        private string[] SortPriorityFilesFirst(string[] files)
        {
            var priorityFiles = files.Where(f => f.EndsWith(".prio", StringComparison.OrdinalIgnoreCase)).ToArray();
            var nonPriorityFiles = files.Except(priorityFiles).ToArray();

            Debug.WriteLine($"[DEBUG] Tri des fichiers: {priorityFiles.Length} fichiers prioritaires trouvés.");

            return priorityFiles.Concat(nonPriorityFiles).ToArray();
        }
    }
}
