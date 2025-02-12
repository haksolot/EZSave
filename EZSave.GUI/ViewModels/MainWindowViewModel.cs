using EZSave.Core.Models;
using EZSave.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.WindowsAPICodePack.Dialogs;
namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public LanguageViewModel LanguageViewModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }
        public ManagerService managerService;
        private readonly ConfigService configService;

        private readonly ManagerModel managerModel;

        public IEnumerable<JobModel> jobs;
        public ICommand SelectFolderCommand { get; }
        public IEnumerable<JobModel> Jobs
        {
            get => jobs;
            set => SetProperty(ref jobs, value);
        }

        public ICommand RefreshCommand { get; set; }
        public ICommand AddJobCommand { get; set; }
        public ICommand ExecuteAllJobsCommand { get; set; }
        public ICommand OpenJobWindowCommand { get; set; }

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
            //Jobs = managerModel.Jobs;
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
            //return isExecuted;
        }



    }
}
