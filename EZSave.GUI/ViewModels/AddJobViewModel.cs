using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EZSave.Core.Models;
using EZSave.Core.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;

namespace EZSave.GUI.ViewModels
{
    public class AddJobViewModel : INotifyPropertyChanged
    {


        private string _Name;
        private string _Source;
        private string _Destination;
        private string _Type;

        public ICommand AddJobCommand { get; }
        //public ICommand SelectSourceFolderCommand { get; }
        //public ICommand SelectDestinationFolderCommand { get; }

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

        public ManagerModel managerModel { get; set; }
        public ConfigFileModel configFileModel { get; set; }
        private readonly ConfigService configService;
        private readonly ManagerService managerService;
        public AddJobViewModel(ManagerModel manager)
        {

            configFileModel = new ConfigFileModel();
            managerModel = manager;
            configService = new ConfigService();
            managerService = new ManagerService();
            AddJobCommand = new RelayCommand(AddJob);
            //SelectSourceFolderCommand = new RelayCommand(SelectSourceFolder);
            //SelectDestinationFolderCommand = new RelayCommand(SelectDestinationFolder);
        }

        public void AddJob()
        {

            var job = new JobModel();
            job.Name = Name;
            job.Source = Source;
            job.Destination = Destination;
            job.Type = Type;

            
            managerService.Add(job, managerModel);
            configService.SaveJob(job, configFileModel);
           


            MessageBox.Show("Job ajouté et sauvegardé avec succès !");
            //Message = "Job ajouté et sauvegardé dans config !";
            //return isAdded;
        }

        


        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T old, T @new, [CallerMemberName] string name = "")
        {
            old = @new;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

      

    }
}
