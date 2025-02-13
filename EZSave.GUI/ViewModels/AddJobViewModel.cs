using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EZSave.Core.Models;
using EZSave.Core.Services;
using EZSave.GUI.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;


namespace EZSave.GUI.ViewModels
{
    public class AddJobViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _source;
        private string _destination;
        private string _type;

        public ICommand AddJobCommand { get; }
        public ICommand SelectSourceFolderCommand { get; }
        public ICommand SelectDestinationFolderCommand { get; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Source
        {
            get => _source;
            set => SetProperty(ref _source, value);
        }

        public string Destination
        {
            get => _destination;
            set => SetProperty(ref _destination, value);
        }

        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public ManagerModel ManagerModel { get; set; }
        public ConfigFileModel ConfigFileModel { get; set; }
        public LanguageViewModel LanguageViewModel { get; set; }
        private readonly ConfigService _configService;
        private readonly ManagerService _managerService;

        public AddJobViewModel(ManagerModel manager)
        {
            ConfigFileModel = new ConfigFileModel();
            ManagerModel = manager;
            _configService = new ConfigService();
            _managerService = new ManagerService();
            LanguageViewModel = new LanguageViewModel();

            AddJobCommand = new RelayCommand(() => AddJob());
            SelectSourceFolderCommand = new RelayCommand(() => SelectSourceFolder());
            SelectDestinationFolderCommand = new RelayCommand(() => SelectDestinationFolder());

        }

        public void AddJob()
        {
            var job = new JobModel
            {
                Name = Name,
                Source = Source,
                Destination = Destination,
                Type = Type
            };

            _managerService.Add(job, ManagerModel);
            _configService.SaveJob(job, ConfigFileModel);

            MessageBox.Show("Job ajouté et sauvegardé avec succès !");
        }

        private void SelectSourceFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Source = dialog.FileName;
            }
        }

        private void SelectDestinationFolder()
        {
            var dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Destination = dialog.FileName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
