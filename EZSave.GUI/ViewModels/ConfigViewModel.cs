using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EZSave.Core.Models;
using EZSave.Core.Services;
using System.Windows;
using EZSave.GUI.Commands;


namespace EZSave.GUI.ViewModels
{
    public class ConfigViewModel : INotifyPropertyChanged
    {
        private string _logPath;
        private string _configPath;
        private string _statusPath;
        private readonly ConfigService _configService;
        private readonly ConfigFileModel _configFileModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand SaveConfigCommand { get; }

        public string LogPath
        {
            get => _logPath;
            set => SetProperty(ref _logPath, value);
        }

        public string ConfigPath
        {
            get => _configPath;
            set => SetProperty(ref _configPath, value);
        }

        public string StatusPath
        {
            get => _statusPath;
            set => SetProperty(ref _statusPath, value);
        }

        public ConfigViewModel(ConfigFileModel configFileModel)
        {
            _configFileModel = configFileModel;
            _configService = new ConfigService();
            SaveConfigCommand = new RelayCommand(SaveConfig);
        }

        private void SaveConfig()
        {
            _configService.SetLogDestination(LogPath, _configFileModel);
            _configService.SetConfigDestination(ConfigPath, _configFileModel);
            _configService.SetStatusDestination(StatusPath, _configFileModel);
            MessageBox.Show("Configuration enregistrée !");
        }

        private void SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
