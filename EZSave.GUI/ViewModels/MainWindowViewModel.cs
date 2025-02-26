using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.Views;

namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }

        private JobService jobService;
        public ICommand OpenConfigCommand { get; }

        private ConfigFileModel configFileModel { get; set; }

        private ManagerService managerService;
        private ConfigService configService;
        private StatusService statusService;

        public ManagerModel managerModel;

        private JobModel _elementSelectionne;

        private SocketServerService _socketServer;
        private Thread _serverThread;
        public JobModel ElementSelectionne
        {
            get => _elementSelectionne;
            set => SetProperty(ref _elementSelectionne, value);
        }

        private string _elementSelectionneList;

        public string ElementSelectionneList
        {
            get => _elementSelectionneList;
            set => SetProperty(ref _elementSelectionneList, value);
        }

        private bool _hasPendingPriorityFiles;

        public bool HasPendingPriorityFiles
        {
            get => _hasPendingPriorityFiles;
            set
            {
                if (_hasPendingPriorityFiles != value)
                {
                    _hasPendingPriorityFiles = value;
                    Debug.WriteLine($"[DEBUG] Mise à jour de HasPendingPriorityFiles : {value}");
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasPendingPriorityFiles)));
                }
            }
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private int progression;

        public int Progression
        {
            get => progression;
            set => SetProperty(ref progression, value);
        }

        private readonly StatusService _statusService;
        private List<Thread> threads = new List<Thread>();
        private Dictionary<string, (Thread thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> JobStates = new();
        public ObservableCollection<string> List { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<JobModel> Jobs { get; set; } = new ObservableCollection<JobModel>();

        public Dictionary<string, int> progressions = new();

        public Dictionary<string, int> Progressions
        {
            get => progressions;
            set => SetProperty(ref progressions, value);
        }

        //nouvel methode 
        private Dictionary<string, ProgressViewModel> progressWindows = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand UpdateProgressionCommand { get; }
        public ICommand AddToListCommand { get; }
        public ICommand RemoveToListCommand { get; }
        public ICommand RemoveAllToListCommand { get; }
        public ICommand AddAllToListCommand { get; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ExecuteAllJobsCommand { get; set; }
        public ICommand OpenJobWindowCommand { get; set; }
        public ICommand ExecuteJobSelectionCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public MainWindowViewModel()
        {
            BaseViewModel.MainWindowViewModel = this;

            Initialize();
            LanguageViewModel = new LanguageViewModel();

            RefreshCommand = new RelayCommand(RefreshJobs);
            OpenJobWindowCommand = new RelayCommand(OpenAddJobWindow);

            OpenConfigCommand = new RelayCommand(OpenConfigWindow);

            AddToListCommand = new RelayCommand(AddToList);
            RemoveToListCommand = new RelayCommand(DelFromList);
            RemoveAllToListCommand = new RelayCommand(DelAllFromList);
            AddAllToListCommand = new RelayCommand(AddAllToList);
            PauseCommand = new RelayCommand(Pause);
            StopCommand = new RelayCommand(Stop);
            ExecuteJobSelectionCommand = new RelayCommand<ObservableCollection<string>>(ExecuteJobSelection);
        }

        private void SetProperty<T>(ref T old, T @new, [CallerMemberName] string name = "")
        {
            old = @new;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OpenConfigWindow()
        {
            var configWindow = new ConfigWindow(managerModel, configFileModel);
            configWindow.ShowDialog();
            RefreshJobs();
        }

        private void Initialize()
        {
            configFileModel = new ConfigFileModel();
            managerModel = new ManagerModel();
            configService = new ConfigService();
            managerService = new ManagerService();
            configService.SetConfigDestination("conf.json", configFileModel);
            configService.LoadConfigFile(configFileModel);
            managerService.Read(managerModel, configFileModel);

            _socketServer = new SocketServerService(RefreshJobs, UpdateJobProgress, Progressions, 6969, managerModel, configFileModel);
            _serverThread = new Thread(_socketServer.Start) { IsBackground = true };
            _serverThread.Start();
        }

        public void RefreshJobs()
        {
            Jobs.Clear();
            HasPendingPriorityFiles = Jobs.Any(job => jobService.HasPendingPriorityFiles(job));
            Debug.WriteLine($"[DEBUG] Vérification fichiers .prio : {HasPendingPriorityFiles}");

            if (managerModel.Jobs != null && managerModel.Jobs.Any())
            {
                foreach (var job in managerModel.Jobs)
                {
                    Jobs.Add(job);
                }
            }
            else
            {
                Debug.WriteLine("Aucun job à changer dans les listes.");
            }
        }

        private void OpenAddJobWindow()
        {
            var window = new AddJobWindow(managerModel, configFileModel);
            window.ShowDialog();
            RefreshJobs();
        }

        private void OpenProgressJobWindow(string jobName)
        {
            //var window = new ProgressionJobWindow(jobName, this);
            //window.Show();

            var window = new ProgressionJobWindow(jobName, this);
            var progressViewModel = new ProgressViewModel(jobName, this);

            window.DataContext = progressViewModel;
            
            window.Title = jobName;
            window.Show();

            progressWindows[jobName] = progressViewModel;
        }


        private void ExecuteJobSelection(ObservableCollection<string> selectedNames)
        {
            Debug.WriteLine($"element selectionné {ElementSelectionneList}");
            
            foreach (var jobName in selectedNames)
            {
                if (!IsProgressJobWindowOpen(jobName))
                {
                    OpenProgressJobWindow(jobName);
                }
                var progress = new Progress<int>(value => UpdateJobProgress(jobName, value));
            }
                
            bool result = managerService.ExecuteSelected(
                JobStates, 
                selectedNames, 
                managerModel, 
                configFileModel, 
                ElementSelectionneList, 
                UpdateJobProgress);

            if (result)
            {
               
                Message = Properties.Resources.JobsExecutedSuccess;
            }
            else
            {
                Message = Properties.Resources.JobsExecutedFail;
            }
        }

        private void AddToList()
        {
            if (ElementSelectionne != null && !List.Contains(ElementSelectionne.Name))
            {
                var valeur = ElementSelectionne.Name;
                List.Add(valeur);
            }
        }

        private void AddAllToList()
        {
            if (managerModel.Jobs.Count != 0)
            {
                foreach (var item in managerModel.Jobs)
                {
                    if (!List.Contains(item.Name))
                    {
                        List.Add(item.Name);
                    }
                }
            }
        }

        public void DelFromList()
        {
            if (ElementSelectionneList != null)
            {
                List.Remove(ElementSelectionneList);
            }

            foreach (var item in List)
            {
                Debug.WriteLine(item);
            }
        }

        public void DelAllFromList()
        {
            if (List.Count != 0)
            {
                List.Clear();
            }
        }

        public void Pause()
        {
            managerService.Pause(ElementSelectionneList, JobStates);
            ElementSelectionneList = null;
        }

        public void Stop()
        {
            Debug.WriteLine($"{ElementSelectionneList} mis en arret");
            managerService.Stop(ElementSelectionneList, JobStates);
            ElementSelectionneList = null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateJobProgress(string jobName, int value)
        {
            Debug.WriteLine($"Mise à jour du job {jobName} avec une progression de {value}%");

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Progressions.ContainsKey(jobName))
                {
                    Progressions[jobName] = value;
                }
                else
                {
                    Progressions.Add(jobName, value);
                }

                OnPropertyChanged(nameof(Progressions));

                if (progressWindows.ContainsKey(jobName))
                {
                    progressWindows[jobName].UpdateProgress(value);
                }
            });
            //foreach (var kvp in progressions)
            //{
            //    Debug.WriteLine($"[DEBUG] {kvp.Key}: {kvp.Value}%");

            //}
        }

        private bool IsProgressJobWindowOpen(string jobName)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is ProgressionJobWindow && window.Title == jobName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}