using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EZSave.Core;
using EZSave.Core.Services;

namespace EZSave.GUI.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel MainWindowViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        private SocketClientService _socket;
        public string JobName { get; set; }

        private int progression;

        public int Progression
        {
            get => progression;
            set
            {
                if (progression != value)
                {
                    Debug.WriteLine($"[DEBUG] Mise à jour de la progression dans {JobName} : {value}%");
                    progression = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProgressViewModel(string jobName, MainWindowViewModel mainWindowViewModel)
        {
            JobName = jobName;
            MainWindowViewModel = mainWindowViewModel;

            if (MainWindowViewModel.Progressions.ContainsKey(JobName))
            {
                Progression = MainWindowViewModel.Progressions[JobName];
            }
            //MainWindowViewModel.PropertyChanged += (sender, args) =>
            //{
            //    if (args.PropertyName == "Progression")
            //    {
            //        Progression = MainWindowViewModel.Progression;
            //    }
            //};
        }
        public void UpdateProgress(int value)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Progression = value;
            });
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}