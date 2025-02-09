using EZSave.Core.Models;

namespace EZSave.Core.Services
{
    public class JobService
    {
        public void Start(JobModel job, LogService logService, ConfigFileModel configFileModel)
        {
            // Log du début de la sauvegarde
            logService.Write(new LogModel
            {
                Name = job.Name,
                Timestamp = DateTime.Now,
                FileSource = "",
                FileDestination = "",
                FileSize = 0,
                FileTransferTime = 0
            }, configFileModel);

            if (job.Type == "full")
            {
                FullBackup(job, logService, configFileModel);  // Passage du LogService à FullBackup

            }
            else
            {
                DifferentialBackup(job);  // Passage du LogService à DifferentialBackup
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
            //long totalSize = GetTotalSize(job); // Taille totale des fichiers à copier
            long copiedSize = 0; // Taille des fichiers copiés
            var startTime = DateTime.Now;

            foreach (string file in Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories))
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                string? directoryPath = Path.GetDirectoryName(destinationFile);

                // Crée les répertoires nécessaires
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Copie du fichier
                File.Copy(file, destinationFile, true);
                var endTime = DateTime.Now;

                // Calcul du temps de transfert du fichier
                float transferTime = (float)(endTime - startTime).TotalSeconds;

                // Calcul de la taille du fichier copié
                long currentFileSize = new FileInfo(file).Length;

                // Mise à jour de la taille totale copiée
                copiedSize += currentFileSize;

                // Log pour chaque fichier copié
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

            // Log pour la fin du backup
            logService.Write(new LogModel
            {
                Name = job.Name,
                Timestamp = DateTime.Now,
                FileSource = "",
                FileDestination = "",
                FileSize = copiedSize,  // Taille totale des fichiers copiés
                FileTransferTime = (float)(DateTime.Now - startTime).TotalSeconds  // Durée totale du processus de copie
            }, configFileModel);
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
