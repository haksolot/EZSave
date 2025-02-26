using EZSave.Core.Models;
using EZSave.Core.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace EZSave.Client.ViewModels
{
    public class AddJobViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel MainWindowViewModel { get; set; }
        private string _Name;
        private string _Source;
        private string _Destination;
        private string _Type;

        public ICommand AddJobCommand { get; }

        public List<String> JobTypes { get; } = new List<String> { "full", "diff" };

        public string Name
        {
            get => _Name;
            set => SetProperty(ref _Name, value);
        }

        public string Source
        {
            get => _Source;
            set => SetProperty(ref _Source, value);
        }

        public string Destination
        {
            get => _Destination;
            set => SetProperty(ref _Destination, value);
        }

        public string Type
        {
            get => _Type;
            set => SetProperty(ref _Type, value);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        private SocketClientService _clientService;
        public ManagerModel managerModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }
        private readonly ConfigService configService;
        private readonly ManagerService managerService;

        public AddJobViewModel(ManagerModel manager, ConfigFileModel config, SocketClientService _socket)
        {
            //MainWindowViewModel = new MainWindowViewModel();
            configFileModel = config;
            managerModel = manager;
            configService = new ConfigService();
            managerService = new ManagerService(configService, configFileModel);
            AddJobCommand = new RelayCommand(AddJob);
            _clientService = _socket;
        }

        public void AddJob()
        {
            var job = new JobModel();
            job.Name = Name;
            job.Source = Source;
            job.Destination = Destination;
            job.Type = Type;
            var jsonJob = JsonSerializer.Serialize(job);
            managerService.Add(job, managerModel);
            //bool result = configService.SaveJob(job, configFileModel);
            string result = _clientService.SendCommand("addjob", jsonJob);
            if (result == "Success")
            {
                Message = Properties.Resources.JobAdded;
            }
            else if (result == "Error")
            {
                Message = Properties.Resources.JobNotAdded;
            }
            else
            {
                Message = "Protocol problem";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T old, T @new, [CallerMemberName] string name = "")
        {
            old = @new;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
