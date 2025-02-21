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

        public ICommand RefreshCommand { get; set; }
        public ICommand ExecuteAllJobsCommand { get; set; }
        public ICommand OpenJobWindowCommand { get; set; }
        public ICommand ExecuteJobSelectionCommand { get; set; }
        public ICommand PlayThread { get; set; }
        public ICommand PauseThread { get; set; }
        public ICommand StopThread { get; set; }
        public ICommand ResumeThread { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Thread tempThread; 

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
            ExecuteAllJobsCommand = new RelayCommand(ExecuteJobs);

            OpenConfigCommand = new RelayCommand(OpenConfigWindow);

            PlayThread = new RelayCommand(Play);
            PauseThread = new RelayCommand(Pause);
            ResumeThread = new RelayCommand(Resume);// IDEE Mettre le resume avec le PLay
            StopThread = new RelayCommand(Stop);

            AddToListCommand = new RelayCommand(AddToList);
            RemoveToListCommand = new RelayCommand(DelFromList);
            ExecuteJobSelectionCommand = new RelayCommand<ObservableCollection<string>>(ExecuteJobSelection);
            PlayThread = new RelayCommand(Play);
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
        private void Play() // Button to bind
        {
            string jobName = _elementSelectionne.Name;
            var jobmodel = new JobModel();

            foreach (var job in managerModel.Jobs)
            {
                if (job.Name == jobName)
                {
                    jobmodel = job;
                }
            }
            tempThread = managerService.ExecuteAsThread(jobmodel, managerModel, configFileModel); //TODO methode qui lance le job avec un thread
        }

        private void Stop()
        {
            managerService.StopThread(tempThread); //TODO methode qui arrete totalement le job 
        }

        private void Pause()
        {
            managerService.PauseThread(tempThread); //TODO methode qui pause le job 
        }
        private void Resume()
        {
            managerService.ResumeThread(tempThread); //TODO methode qui relance le job 
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

        private void ExecuteJobs()
        {
            bool result = managerService.Execute(managerModel, configFileModel);

            if (result)
            {
                Message = Properties.Resources.JobsExecutedSuccess;
            }
            else
            {
                Message = Properties.Resources.JobsExecutedFail;
            }
        }

        private void ExecuteJobSelection(ObservableCollection<string> selectedNames)
        {
            bool result = managerService.ExecuteSelected(selectedNames, managerModel, configFileModel);
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
            if (ElementSelectionne != null)
            {
                var valeur = ElementSelectionne.Name;
                List.Add(valeur);
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
    }
}