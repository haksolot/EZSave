namespace EZSave.Progress.Services
{
    using System;
    using System.Threading.Tasks;
    using EZSave.Progress.Models;

    public class ProgressViewModel
    {
        public ProgressModel Progress { get; set; }

        public ProgressViewModel()
        {
            Progress = new ProgressModel
            {
                FilesLeft = 10,                          //j'initialise les 2 afin d'avoir un état initial
                 TotalFileSizeLeft = 100.0f,
                Source = "C:\\Backup\\Source",
                Destination = "D:\\Backup\\Destination"
            };
        }

        public async Task StartProgressAsync()           // simu d'un process qui réduit progressivement le nb de fichiers           //pour l'exp user pour pas bloquer l'UI
        {
            while (Progress.FilesLeft > 0)
            {
                await Task.Delay(1000);  // normalement ca fait le trnasfert //att1 sec
                Progress.FilesLeft--;        //diminue de 1 le nb de fichiers restants        
                   Progress.TotalFileSizeLeft -= 10.0f;   // réduit la taille des fichiers restants
            }
        }
    }
}
