using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.Commands;


namespace EZSave.GUI.ViewModels
{
    public class JobViewModel : INotifyPropertyChanged
    {
        private JobModel _selectedJob;
        private readonly ConfigFileModel _configFileModel;
        private readonly ManagerService _managerService;
        private readonly ConfigService _configService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand DeleteJobCommand { get; }
        public ICommand EditJobCommand { get; }

        public JobModel SelectedJob
        {
            get => _selectedJob;
            set => SetProperty(ref _selectedJob, value);
        }

        public JobViewModel(ConfigFileModel configFileModel)
        {
            _configFileModel = configFileModel;
            _managerService = new ManagerService();
            _configService = new ConfigService();

            DeleteJobCommand = new RelayCommand(DeleteJob);
            EditJobCommand = new RelayCommand(EditJob);
        }

        private void DeleteJob()
        {
            if (SelectedJob != null)
            {
                _managerService.RemoveJob(SelectedJob, new ManagerModel());
                _configService.DeleteJob(SelectedJob, _configFileModel);
                MessageBox.Show("Job supprimé !");
            }
        }

        private void EditJob()
        {
            if (SelectedJob != null)
            {
                var editWindow = new AddJobWindow(new ManagerModel())
                {
                    DataContext = new AddJobViewModel(new ManagerModel())
                };
                editWindow.ShowDialog();
            }
        }

        private void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
