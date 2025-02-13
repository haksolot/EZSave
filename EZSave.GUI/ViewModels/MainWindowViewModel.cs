using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.Commands;
using EZSave.GUI.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EZSave.GUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ManagerModel _managerModel;
        private readonly ConfigFileModel _configFileModel = new ConfigFileModel(); // 🔥 Ajout de ConfigFileModel
        private ObservableCollection<JobModel> _jobs;
        private JobModel _selectedJob;

        public ObservableCollection<JobModel> Jobs
        {
            get => _jobs;
            set => SetProperty(ref _jobs, value);
        }

        public JobModel SelectedJob
        {
            get => _selectedJob;
            set => SetProperty(ref _selectedJob, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand OpenConfigCommand { get; }
        public ICommand ChangeLanguageCommand { get; } // 🔥 Ajout du bouton de langue
        public ICommand ExecuteSelectedJobsCommand { get; }
        public ICommand ExecuteAllJobsCommand { get; }
        public ICommand AddJobCommand { get; }

        public LanguageViewModel LanguageViewModel { get; set; }

        public MainWindowViewModel()
        {
            LanguageViewModel = new LanguageViewModel();
            _managerModel = new ManagerModel();
            Jobs = new ObservableCollection<JobModel>(_managerModel.Jobs);

            RefreshCommand = new RelayCommand(RefreshJobs);
            OpenConfigCommand = new RelayCommand(OpenConfig);
            ChangeLanguageCommand = new RelayCommand(ChangeLanguage); // 🔥 Ajout
            AddJobCommand = new RelayCommand(OpenAddJobWindow);
            ExecuteSelectedJobsCommand = new RelayCommand(ExecuteSelectedJobs);
            ExecuteAllJobsCommand = new RelayCommand(ExecuteAllJobs);
        }

        private void RefreshJobs()
        {
            Jobs = new ObservableCollection<JobModel>(_managerModel.Jobs);
        }

        private void OpenConfig()
        {
            var configWindow = new ConfigWindow(_configFileModel); // 🔥 Correction de l'ouverture
            configWindow.ShowDialog();
        }

        private void ChangeLanguage()
        {
            LanguageViewModel.RefreshLanguage();
        }

        private void OpenAddJobWindow()
        {
            var addJobWindow = new AddJobWindow(_managerModel);
            addJobWindow.ShowDialog();
            RefreshJobs();
        }

        private async void ExecuteSelectedJobs()
        {
            if (SelectedJob != null)
            {
                Console.WriteLine($"🚀 Lancement du job {SelectedJob.Name}");
                await JobService.Start(SelectedJob);
                Console.WriteLine($"✅ Job {SelectedJob.Name} terminé !");
            }
        }

        private async void ExecuteAllJobs()
        {
            foreach (var job in Jobs)
            {
                Console.WriteLine($"🚀 Lancement du job {job.Name}");
                await JobService.Start(job);
                Console.WriteLine($"✅ Job {job.Name} terminé !");
            }
        }


        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
