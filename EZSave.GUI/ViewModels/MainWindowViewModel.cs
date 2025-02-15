using EZSave.Core.Models;
using EZSave.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }

        private readonly ManagerService managerService;
        private readonly ConfigService configService;

        private readonly ManagerModel managerModel;

        private JobModel _elementSelectionne;
        public JobModel ElementSelectionne
        {
            get => _elementSelectionne;
            set => SetProperty(ref _elementSelectionne, value);
        }

        private string _elementSelectionneList;
        public string ElementSelectionneList
        {
            get =>  _elementSelectionneList;
            set => SetProperty(ref _elementSelectionneList, value);
        }
        public ObservableCollection<string> List { get; set; } = new ObservableCollection<string>();

        public IEnumerable<JobModel> jobs;
        public IEnumerable<JobModel> Jobs
        {
            get => jobs;
            set => SetProperty(ref jobs, value);
        }
        public ICommand AddToListCommand { get; }
        public ICommand RemoveToListCommand { get; }

        public ICommand RefreshCommand { get; set; }
        public ICommand AddJobCommand { get; set; }
        public ICommand ExecuteAllJobsCommand { get; set; }
        public ICommand OpenJobWindowCommand { get; set; }
        public ICommand ExecuteJobSelectionCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public MainWindowViewModel()
        {
            LanguageViewModel = new LanguageViewModel();
            configFileModel = new ConfigFileModel();
            configService = new ConfigService();
            managerService = new ManagerService();
            managerModel = new ManagerModel();

            RefreshCommand = new RelayCommand(RefreshJobs);
            OpenJobWindowCommand = new RelayCommand(OpenAddJobWindow);
            ExecuteAllJobsCommand = new RelayCommand(ExecuteJobs);
            AddToListCommand = new RelayCommand(AddToList);
            RemoveToListCommand = new RelayCommand(DelFromList);
            ExecuteJobSelectionCommand = new RelayCommand<ObservableCollection<string>>(ExecuteJobSelection);
        }

        

        private void SetProperty<T>(ref T old, T @new, [CallerMemberName] string name = "")
        {
            old = @new;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void RefreshJobs()
        {
            Jobs = managerModel.Jobs.ToList();
        }

        private void OpenAddJobWindow()
        {
            var window = new AddJobWindow(managerModel);
            window.ShowDialog();
        }

        private void ExecuteJobs()
        {
            configFileModel.LogFileDestination = "Log";
            configFileModel.StatusFileDestination = "Status";
            managerService.Execute(managerModel, configFileModel);
        }

        private void ExecuteJobSelection(ObservableCollection<string> selectedNames)
        {
            if (selectedNames.Any())
            {
                configFileModel.LogFileDestination = "Log";
                configFileModel.StatusFileDestination = "Status";

                managerService.ExecuteSelected(selectedNames, managerModel, configFileModel);
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

        private void DelFromList()
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