using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EZSave.GUI.ViewModels
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        private readonly MainWindowViewModel MainWindowViewModel;
        public event PropertyChangedEventHandler? PropertyChanged;
        private int progression;
        public string JobName { get; set; }
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
                    CanExecuteButton = (progression == 100);
                }
            }
        }

        private bool canExecuteButton;
        public bool CanExecuteButton
        {
            get => canExecuteButton;
            set
            {
                canExecuteButton = value;
                OnPropertyChanged();
            }
        }
        public ICommand OkCommand { get; }

        public ProgressViewModel(string jobName, MainWindowViewModel mainWindowViewModel)
        {
            JobName = jobName;
            MainWindowViewModel = mainWindowViewModel;
            OkCommand = new RelayCommand(TerminerAction);

            if (MainWindowViewModel.Progressions.TryGetValue(JobName, out int initialProgress))
            {
                Progression = initialProgress;
            }

            MainWindowViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MainWindowViewModel.Progressions))
                {
                    if (MainWindowViewModel.progressions.TryGetValue(JobName, out int newProgress))
                    {
                        Progression = newProgress;
                    }
                }
            };
        }

        private void TerminerAction()
        {
            System.Windows.MessageBox.Show("Le job est terminé !");
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
