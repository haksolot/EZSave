using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EZSave.Core.Models;
using EZSave.Core.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using MS.WindowsAPICodePack.Internal;

namespace EZSave.GUI.ViewModels
{
    public class AddJobViewModel : INotifyPropertyChanged
    {

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


        public ManagerModel managerModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }
        private readonly ConfigService configService;
        private readonly ManagerService managerService;
        public AddJobViewModel(ManagerModel manager, ConfigFileModel config)
        {

            configFileModel = config;
            managerModel = manager;
            configService = new ConfigService();
            managerService = new ManagerService();
            AddJobCommand = new RelayCommand(AddJob);
        }

        public void AddJob()
        {
            var job = new JobModel();
            job.Name = Name;    
            job.Source = Source;
            job.Destination = Destination;
            job.Type = Type;


            managerService.Add(job, managerModel);
            bool result = configService.SaveJob(job, configFileModel);
            
            if (result)
            {
                Message = Properties.Resources.JobAdded;
            }
            else
            {
                Message = Properties.Resources.JobNotAdded;
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
