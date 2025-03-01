﻿using EZSave.Core.Models;
using EZSave.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;

namespace EZSave.Client.ViewModels
{
    public class ConfigViewModel : INotifyPropertyChanged
    {
        private readonly ConfigService _configService;
        private readonly ManagerModel _managerModel;
        private readonly ManagerService _managerService;
        private ConfigFileModel _configFileModel;
        private SocketClientService _socketService;

        public ICommand SaveConfigCommand { get; }
        public ICommand EditJobCommand { get; }
        public ICommand DeleteJobCommand { get; }
        public ICommand RefreshJobsCommand { get; }

        public ConfigViewModel(ConfigFileModel config, ManagerModel managerModel, SocketClientService socket)
        {
            _configService = new ConfigService();
            _managerModel = managerModel;
            _configFileModel = config;
            _managerService = new ManagerService(_configService, _configFileModel);
            SaveConfigCommand = new RelayCommand(SaveConfig);
            EditJobCommand = new RelayCommand(EditJob);
            DeleteJobCommand = new RelayCommand(DeleteJob);
            RefreshJobsCommand = new RelayCommand(RefreshJobs);

            _configService.LoadConfigFile(_configFileModel);
            _socketService = socket;
            RefreshJobs();
        }

        public string ConfFileDestination
        {
            get => _configFileModel.ConfFileDestination;
            set { _configFileModel.ConfFileDestination = value; OnPropertyChanged(); }
        }

        public string LogFileDestination
        {
            get => _configFileModel.LogFileDestination;
            set { _configFileModel.LogFileDestination = value; OnPropertyChanged(); }
        }

        public string LogType
        {
            get => _configFileModel.LogType;
            set { _configFileModel.LogType = value; OnPropertyChanged(); }
        }
        public string CryptoKey
        {
            get => _configFileModel.Key;
            set { _configFileModel.Key = value; OnPropertyChanged(); }
        }

        public string StatusFileDestination
        {
            get => _configFileModel.StatusFileDestination;
            set { _configFileModel.StatusFileDestination = value; OnPropertyChanged(); }
        }

        private ObservableCollection<JobModel> _jobs;

        public ObservableCollection<JobModel> Jobs
        {
            get => _jobs;
            set { _jobs = value; OnPropertyChanged(); }
        }

        private JobModel _selectedJob;

        public JobModel SelectedJob
        {
            get => _selectedJob;
            set
            {
                _selectedJob = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedJobSource));
                OnPropertyChanged(nameof(SelectedJobDestination));
                OnPropertyChanged(nameof(SelectedJobType));
            }
        }

        public string SelectedJobSource
        {
            get => SelectedJob?.Source ?? "";
            set
            {
                if (SelectedJob != null)
                {
                    SelectedJob.Source = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedJobDestination
        {
            get => SelectedJob?.Destination ?? "";
            set
            {
                if (SelectedJob != null)
                {
                    SelectedJob.Destination = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedJobType
        {
            get => SelectedJob?.Type ?? "";
            set
            {
                if (SelectedJob != null)
                {
                    SelectedJob.Type = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<string> JobTypes { get; } = new() { "full", "diff" };
        public List<string> LogTypes { get; } = new() { "xml", "json" };

        private string _statusMessage;

        public string StatusMessage
        {
            get => _statusMessage;
            set { _statusMessage = value; OnPropertyChanged(); }
        }

        public long FileSizeThreshold
        {
            get => _configFileModel.FileSizeThreshold;
            set
            {
                _configFileModel.FileSizeThreshold = value;
                JobModel.FileSizeThreshold = value;
                OnPropertyChanged();
            }
        }

        private void SetStatusMessage(string message)
        {
            StatusMessage = message;
        }

        private void RefreshJobs()
        {
            Jobs = new ObservableCollection<JobModel>(_managerModel.Jobs.ToList());
        }

        private void EditJob()
        {
            if (SelectedJob != null)
            {
                _configFileModel.Jobs[SelectedJob.Name] = SelectedJob;
                //_configService.SaveConfigFile(_configFileModel);
                var selectedJob = JsonSerializer.Serialize(SelectedJob);
                _socketService.SendCommand("editjob", selectedJob);
                RefreshJobs();
                SetStatusMessage("Job modifié avec succès !");
            }
        }

        public void DelFromSelectedList(string job, ObservableCollection<string> JobList)
        {
            if (job != null)
            {
                JobList.Remove(job);
            }

            foreach (var item in JobList)
            {
                Debug.WriteLine(item);
            }
        }

        private void DeleteJob()
        {
            if (SelectedJob != null)
            {
                _managerService.RemoveJob(SelectedJob, _managerModel);
                _configService.SaveConfigFile(_configFileModel);
                var job2del = JsonSerializer.Serialize(SelectedJob);
                var result = _socketService.SendCommand("deljob", job2del);
                RefreshJobs();
                if (result == "Success")
                {
                    SetStatusMessage("Job supprimé avec succès !");
                }
                else
                {
                    SetStatusMessage(result);
                }
                //DelFromSelectedList(SelectedJob.Name, BaseViewModel.MainWindowViewModel.List);
            }
        }

        private void SaveConfig()
        {
            var confFile = JsonSerializer.Serialize(_configFileModel);
            _socketService.SendCommand("saveconf", confFile);
            //_configService.SaveConfigFile(_configFileModel);
            RefreshJobs();
            SetStatusMessage("Configuration sauvegardée !");
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}