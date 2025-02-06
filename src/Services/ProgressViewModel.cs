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
                FilesLeft = 10,
                 TotalFileSizeLeft = 100.0f,
                Source = "C:\\Backup\\Source",
                Destination = "D:\\Backup\\Destination"
            };
        }

        public async Task StartProgressAsync()
        {
            while (Progress.FilesLeft > 0)
            {
                await Task.Delay(1000);  // normalement ca fait le trnasfert
                Progress.FilesLeft--;
                   Progress.TotalFileSizeLeft -= 10.0f;
            }
        }
    }
}
