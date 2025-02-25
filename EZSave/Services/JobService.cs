using CryptoSoft;
using EZSave.Core.Models;
using System.Diagnostics;

namespace EZSave.Core.Services
{
    public class JobService
    {
        long fileSizeThreshold = JobModel.FileSizeThreshold;

        public bool Start(JobModel job, StatusService statusService, LogService logService, ConfigFileModel configFileModel, string name, ManualResetEvent pauseEvent, CancellationToken cancellationToken, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            Debug.WriteLine($"Lancement du job {name}");

            while (ProcessesService.CheckProcess("CalculatorApp"))
            {
                if (jobStates.TryGetValue(name, out var jobState) && jobState.Status != "Paused")
                {
                    Debug.WriteLine($"{name} mis en pause à cause de CalculatorApp");
                    jobStates[name] = (jobState.Thread, jobState.Cts, jobState.PauseEvent, "Paused");
                   
                    jobState.PauseEvent.Reset(); 
                }

            }

            if (jobStates.TryGetValue(name, out var resumedJob) && resumedJob.Status == "Paused")
            {
                Debug.WriteLine($"CalculatorApp fermé, reprise du job {name}");
                jobStates[name] = (resumedJob.Thread, resumedJob.Cts, resumedJob.PauseEvent, "Running");
               
                resumedJob.PauseEvent.Set();
            }

            if (job.Type == "full")
            {
                return FullBackup(job, logService, statusService, configFileModel, pauseEvent, cancellationToken, jobStates);
            }
            else if (job.Type == "diff")
            {
                return DifferentialBackup(job, logService, statusService, configFileModel, pauseEvent, cancellationToken, jobStates);
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
                    Debug.WriteLine("Le dossier source n'existe pas !");
                    return false;
                }

                var prioFiles = Directory.GetFiles(job.Source, "*.prio", SearchOption.AllDirectories);
                bool hasPrio = prioFiles.Any();

                if (hasPrio)
                {
                    Debug.WriteLine($"{prioFiles.Length} fichiers .prio détectés ! Exécution en priorité après pause.");
                }

                return hasPrio;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Impossible de vérifier les fichiers .prio : {ex.Message}");
                return false;
            }
        }

        private bool FullBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);
            filesToCopy = SortPriorityFilesFirst(filesToCopy);

            if (HasPendingPriorityFiles(job))
            {
                Debug.WriteLine("Pause de 5 secondes pour signaler que des fichiers .prio vont être exécutés en priorité.");
                Thread.Sleep(5000);
            }

            return CopyFiles(job, filesToCopy, logService, statusService, configFileModel, pauseEvent, cancellationToken, jobStates);
        }


        private bool DifferentialBackup(JobModel job, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
        {
            var filesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories)
                      .Where(file =>
                          !File.Exists(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))) ||
                          File.GetLastWriteTime(file) > File.GetLastWriteTime(Path.Combine(job.Destination, file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar))))
                      .ToArray();

            filesToCopy = SortPriorityFilesFirst(filesToCopy);

            if (HasPendingPriorityFiles(job))
            {
                Debug.WriteLine("Pause de 5 secondes pour signaler que des fichiers .prio vont être exécutés en priorité.");
                Thread.Sleep(5000);
            }

            return CopyFiles(job, filesToCopy, logService, statusService, configFileModel, pauseEvent, cancellationToken, jobStates);
        }


        private bool CopyFiles(JobModel job, string[] filesToCopy, LogService logService, StatusService statusService, ConfigFileModel configFileModel, ManualResetEvent pauseEvent, CancellationToken cancellationToken, Dictionary<string, (Thread Thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> jobStates)
        {
            using var fileLock = new SemaphoreSlim(1, 1);
            StatusModel statusModel = new StatusModel();

            long copiedSize = 0;
            long currentFileSize = 0;
            var startTime = DateTime.Now;

            statusModel.Name = job.Name;
            statusModel.SourceFilePath = job.Source;
            statusModel.TargetFilePath = job.Destination;
            statusModel.State = "Activate";
            statusModel.Progression = 0;
            statusModel.TotalFilesSize = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Sum(file => new FileInfo(file).Length); ;
            statusModel.TotalFilesToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Length;
            statusModel.FilesLeftToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Length;
            statusModel.FilesSizeLeftToCopy = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories).Sum(file => new FileInfo(file).Length);
            statusService.SaveStatus(statusModel, configFileModel);

            foreach (string file in filesToCopy)
            {
                while (ProcessesService.CheckProcess("CalculatorApp"))
                {
                    if (jobStates.TryGetValue(job.Name, out var jobState) && jobState.Status != "PausedByProcess")
                    {
                        Debug.WriteLine($"{job.Name} mis en pause à cause de CalculatorApp");
                        jobStates[job.Name] = (jobState.Thread, jobState.Cts, jobState.PauseEvent, "PausedByProcess");
                        jobState.PauseEvent.Reset();
                    }
  
                }

                if (jobStates.TryGetValue(job.Name, out var resumedJob) && resumedJob.Status == "PausedByProcess")
                {
                    Debug.WriteLine($"CalculatorApp fermé, reprise du job {job.Name}");
                    jobStates[job.Name] = (resumedJob.Thread, resumedJob.Cts, resumedJob.PauseEvent, "Running");
                    resumedJob.PauseEvent.Set();
                }

                if (cancellationToken.IsCancellationRequested) return false;
                pauseEvent.WaitOne();
                cancellationToken.ThrowIfCancellationRequested();


                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                statusModel.TargetFilePath = Path.Combine(job.Destination, relativePath);
                string? directoryPath = Path.GetDirectoryName(statusModel.TargetFilePath);
                long fileSize = new FileInfo(file).Length;

                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (fileSize >= JobModel.FileSizeThreshold)

                {
                    fileLock.Wait();
                }

                float cipheringTime = 0f;
                if (Path.GetExtension(file) == ".crypto")
                {
                    var crypto = new Cipher(file, "key");
                    cipheringTime = crypto.TransformFile(file);
                }

                File.Copy(file, statusModel.TargetFilePath, true);

                statusModel.FilesSizeLeftToCopy -= fileSize;
                statusModel.FilesLeftToCopy--;
                if (statusModel.TotalFilesSize != 0)
                {
                    var temp1 = statusModel.TotalFilesSize - statusModel.FilesSizeLeftToCopy;
                    var temp2 = (decimal)temp1 / statusModel.TotalFilesSize;
                    var temp3 = temp2 * 100;
                    decimal progression_temp = (decimal)temp3;
                    statusModel.Progression = (int)Math.Floor(progression_temp);
                }
                else
                {
                    statusModel.Progression = 100;
                }

                statusModel.State = "En Cours";

                statusService.SaveStatus(statusModel, configFileModel);

                var endTime = DateTime.Now;
                float transferTime = (float)(endTime - startTime).TotalSeconds;
                currentFileSize = new FileInfo(statusModel.TargetFilePath).Length;
                copiedSize += currentFileSize;

              

                logService.Write(new LogModel
                {
                    Name = job.Name,
                    Timestamp = DateTime.Now,
                    FileSource = file,
                    FileDestination = statusModel.TargetFilePath,
                    FileSize = currentFileSize,
                    FileTransferTime = transferTime,
                    FileCipherTime = cipheringTime,
                }, configFileModel);

                cancellationToken.ThrowIfCancellationRequested();

                if (fileSize >= JobModel.FileSizeThreshold)

                {
                    fileLock.Release();
                }
            }

            statusModel.State = "End";
            statusService.SaveStatus(statusModel, configFileModel);
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
