using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using EZSave.Core.Services;

namespace EZSave.Client.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel MainWindowViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        private SocketClientService _socket;
        private Thread? _updateProgressThread;
        private bool _updateProgressIsRunning;
        public string JobName { get; set; }

        public Dictionary<string, int> Progressions { get; set; }

        private int progression;

        public int Progression
        {
            get => progression;
            set
            {
                if (progression != value)
                {
                    Debug.WriteLine($"[DEBUG] Mise à jour de la progression dans {JobName} : {value}%");
                    progression = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProgressViewModel(string jobName, MainWindowViewModel mainWindowViewModel, SocketClientService socket = null)
        {
            _socket = socket;

            JobName = jobName;
            //MainWindowViewModel = mainWindowViewModel;

            //if (MainWindowViewModel.Progressions.ContainsKey(JobName))
            //{
            //    Progression = MainWindowViewModel.Progressions[JobName];
            //}
            //MainWindowViewModel.PropertyChanged += (sender, args) =>
            //{
            //    if (args.PropertyName == "Progression")
            //    {
            //        Progression = MainWindowViewModel.Progression;
            //    }
            //};
            _socket.StartProgressUpdate();
            _updateProgressIsRunning = true;
            _updateProgressThread = new Thread(() =>
            {
                while (_updateProgressIsRunning)
                {
                    Progressions = _socket.GetLastProgress();
                    foreach (var progression in Progressions)
                    {
                        Debug.WriteLine("Coté client :" + progression.ToString());
                    }
                    Thread.Sleep(500);
                    try
                    {
                        Progression = Progressions[JobName];
                    }
                    catch { Progression = 0; }
                }
            })
            { IsBackground = true };
            _updateProgressThread.Start();
        }

        public void UpdateProgress(int value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                //Progressions[JobName] = value;
                Progression = Progressions[JobName];
                Debug.WriteLine("Thingd :" + Progression);
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void closingProgressWindow()
        {
            _updateProgressIsRunning = false;
        }
    }
}