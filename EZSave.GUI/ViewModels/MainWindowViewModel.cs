using EZSave.Core.Models;
using EZSave.Core.Services;

using EZSave.GUI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }
        public ICommand OpenConfigCommand { get; }

        private ConfigFileModel configFileModel { get; set; }

        private ManagerService managerService;
        private ConfigService configService;

        public ManagerModel managerModel;

        private JobModel _elementSelectionne;

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

        List<Thread> threads = new List<Thread>();
        Dictionary<string, (Thread thread, CancellationTokenSource Cts, ManualResetEvent PauseEvent, string Status)> JobStates = new();
        //Dictionary<string, Thread> dictionary = new Dictionary<string, Thread>();
        //Dictionary<string, CancellationTokenSource> cancellationTokens = new Dictionary<string, CancellationTokenSource>();
        //Dictionary<string, ManualResetEvent> pauseEvents = new Dictionary<string, ManualResetEvent>();
        public ObservableCollection<string> List { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<JobModel> Jobs { get; set; } = new ObservableCollection<JobModel>();

        //public IEnumerable<JobModel> jobs;
        //public IEnumerable<JobModel> Jobs
        //{
        //    get => jobs;
        //    set => SetProperty(ref jobs, value);
        //}
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowViewModel()
        {
            BaseViewModel.MainWindowViewModel = this;

            Initialize();

            LanguageViewModel = new LanguageViewModel();

            //             configService = new ConfigService();
            //             managerService = new ManagerService();

            //configFileModel = new ConfigFileModel();

            //managerModel = new ManagerModel();

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
            //RefreshJobs();
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
            RefreshJobs(); // Rafraîchir après fermeture de la config
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
        }

        public void RefreshJobs()
        {
            Jobs.Clear();
            //List.Clear();

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

        private void ExecuteJobSelection(ObservableCollection<string> selectedNames)
        {
            Debug.WriteLine($"element selectionné { ElementSelectionneList}");

            bool result = managerService.ExecuteSelected(JobStates, selectedNames, managerModel, configFileModel, ElementSelectionneList);
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
            if (ElementSelectionneList != null)
            {
                managerService.Pause(ElementSelectionneList, JobStates);
            }
        }

        public void Stop()
        {
            if (ElementSelectionneList != null)
            {
                Debug.WriteLine($"{ElementSelectionneList} mis en arret");
                managerService.Stop(ElementSelectionneList, JobStates);
            }
        }
    }
    
}