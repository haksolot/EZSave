using EZSave.Core.Models;
using EZSave.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using EZSave.GUI.Views;

namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }
        public ManagerService managerService;
        private readonly ConfigService configService;
        private readonly ManagerModel managerModel;

        public ObservableCollection<JobModel> Jobs { get; set; }

        public ICommand RefreshCommand { get; }
        public ICommand AddJobCommand { get; }
        public ICommand ExecuteAllJobsCommand { get; }
        public ICommand OpenJobWindowCommand { get; }
        public ICommand ExecuteJobSelectionCommand { get; }
        public ICommand OpenConfigCommand { get; }

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
            OpenConfigCommand = new RelayCommand(OpenConfigWindow);

            RefreshJobs();
        }

        private void OpenConfigWindow()
        {
            var configWindow = new ConfigWindow(new ConfigViewModel(configFileModel, managerModel));
            configWindow.ShowDialog();
            RefreshJobs(); // Rafraîchir après fermeture de la config
        }

        private void RefreshJobs()
        {
            Jobs = new ObservableCollection<JobModel>(managerModel.Jobs);
            OnPropertyChanged(nameof(Jobs));
        }

        private void OpenAddJobWindow()
        {
            var window = new AddJobWindow(managerModel);
            window.ShowDialog();
            RefreshJobs();
        }

        private void ExecuteJobs()
        {
            configFileModel.LogFileDestination = "Log";
            configFileModel.StatusFileDestination = "Status";
            managerService.Execute(managerModel, configFileModel);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
