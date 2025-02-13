using EZSave.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EZSave.Core.Services
{
    public static class JobService
    {
        public static async Task Start(JobModel job)
        {
            if (string.IsNullOrWhiteSpace(job.Source) || string.IsNullOrWhiteSpace(job.Destination))
            {
                Console.WriteLine($"❌ ERREUR : Chemins de source ou de destination non valides !");
                return;
            }

            if (!Directory.Exists(job.Source))
            {
                Console.WriteLine($"❌ ERREUR : Le dossier source '{job.Source}' n'existe pas !");
                return;
            }

            if (!Directory.Exists(job.Destination))
            {
                Console.WriteLine($"⚠️ Création du dossier destination : {job.Destination}");
                Directory.CreateDirectory(job.Destination);
            }

            if (job.Type == "full")
            {
                await FullBackup(job);
            }
            else if (job.Type == "diff")
            {
                await DifferentialBackup(job);
            }
            else
            {
                Console.WriteLine($"❌ ERREUR : Type de job inconnu '{job.Type}' !");
            }
        }

        private static async Task FullBackup(JobModel job)
        {
            Console.WriteLine($"📁 Démarrage de la sauvegarde FULL pour {job.Name}");

            string[] files = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);

                Console.WriteLine($"📂 Copie : {file} → {destinationFile}");
                await Task.Run(() => File.Copy(file, destinationFile, true));
            }

            Console.WriteLine($"✅ Sauvegarde FULL terminée pour {job.Name}");
        }

        private static async Task DifferentialBackup(JobModel job)
        {
            Console.WriteLine($"📁 Démarrage de la sauvegarde DIFF pour {job.Name}");

            string[] files = Directory.GetFiles(job.Source, "*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string relativePath = file.Substring(job.Source.Length).TrimStart(Path.DirectorySeparatorChar);
                string destinationFile = Path.Combine(job.Destination, relativePath);

                if (!File.Exists(destinationFile) || File.GetLastWriteTime(file) > File.GetLastWriteTime(destinationFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);

                    Console.WriteLine($"📂 Copie DIFF : {file} → {destinationFile}");
                    await Task.Run(() => File.Copy(file, destinationFile, true));
                }
            }

            Console.WriteLine($"✅ Sauvegarde DIFF terminée pour {job.Name}");
        }
    }
}
