using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using EZSave.Client.ViewModels;
using EZSave.Client;
using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.Client.Views;
using System.Text.Json;
using System.Windows;

namespace EZSave.Client.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }
        public ICommand OpenConfigCommand { get; }

        private ConfigFileModel configFileModel { get; set; }

        private ManagerService managerService;
        private ConfigService configService;
        private StatusService statusService;

        public ManagerModel managerModel;

        private JobModel _elementSelectionne;

        private SocketClientService _socketClient;
        private Thread _serverThread;

        private Thread? _updateProgressThread;
        private bool _updateProgressIsRunning;
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


        List<Thread> threads = new List<Thread>();
        Dictionary<string, (Thread thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> JobStates = new();
        public ObservableCollection<string> List { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<JobModel> Jobs { get; set; } = new ObservableCollection<JobModel>();

        public Dictionary<string, int> progressions = new();

        public Dictionary<string, int> Progressions
        {
            get => progressions;
            set => SetProperty(ref progressions, value);
        }

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

        //public event PropertyChangedEventHandler? PropertyChanged;

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
            //UpdateProgressionCommand = new RelayCommand(UpdateProgression);
            ExecuteJobSelectionCommand = new RelayCommand<ObservableCollection<string>>(ExecuteJobSelection);
        }

        private void SetProperty<T>(ref T old, T @new, [CallerMemberName] string name = "")
        {
            old = @new;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void OpenConfigWindow()
        {
            var configWindow = new ConfigWindow(managerModel, configFileModel, _socketClient);
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

            _socketClient = new SocketClientService();

            var result = _socketClient.SendCommand("getjoblist");
            managerModel.Jobs = JsonSerializer.Deserialize<ObservableCollection<JobModel>>(result);
            RefreshJobs();

            result = _socketClient.SendCommand("getconf");
            ConfigFileModel newConfFile = JsonSerializer.Deserialize<ConfigFileModel>(result);
            configFileModel = newConfFile;

            
        }

        public void RefreshJobs()
        {
            Jobs.Clear();

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
            var window = new AddJobWindow(managerModel, configFileModel, _socketClient);
            window.ShowDialog();
            RefreshJobs();
        }

        private void OpenProgressJobWindow(string jobName)
        {
            //var window = new ProgressionJobWindow(jobName, this);
            //window.Show();

            var window = new ProgressionJobWindow(jobName, this);
            var progressViewModel = new ProgressViewModel(jobName, this, _socketClient);

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
            //bool result = managerService.ExecuteSelected(JobStates, selectedNames, managerModel, configFileModel, ElementSelectionneList);
            var playJobData = new PlayJobModel();
            playJobData.Name = ElementSelectionneList;
            playJobData.selected = selectedNames;
            var jsonData = JsonSerializer.Serialize(playJobData);
            var result = _socketClient.SendCommand("playjob", jsonData);
            if (result == "Success")
            {
                Message = Properties.Resources.JobsExecutedSuccess;
            }
            else if (result == "Error")
            {
                Message = Properties.Resources.JobsExecutedFail;
            }
            else
            {
                Message = "Problem";
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
            if (ElementSelectionneList != null)
            {
                var jop2pauseControll = new JobControlModel();
                jop2pauseControll.Name = ElementSelectionneList;
                jop2pauseControll.selected = JobStates;
                var jsonJob2Pause = JsonSerializer.Serialize(jop2pauseControll);
                _socketClient.SendCommand("pausejob", jsonJob2Pause);
                //managerService.Pause(ElementSelectionneList, JobStates);
            }
        }

        public void Stop()
        {
            if (ElementSelectionneList != null)
            {
                //Debug.WriteLine($"{ElementSelectionneList} mis en arret");
                //managerService.Stop(ElementSelectionneList, JobStates);
                var jop2stopControll = new JobControlModel();
                jop2stopControll.Name = ElementSelectionneList;
                jop2stopControll.selected = JobStates;
                var jsonJob2Stop = JsonSerializer.Serialize(jop2stopControll);
                _socketClient.SendCommand("stopjob", jsonJob2Stop);
            }
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
