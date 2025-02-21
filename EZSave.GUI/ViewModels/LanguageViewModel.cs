using EZSave.GUI.Properties;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace EZSave.GUI.ViewModels
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        public ICommand ChangeLanguageCommand { get; }

        public LanguageViewModel()
        {
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void ChangeLanguage(string languageCode)
        {
            CultureInfo newCulture = new CultureInfo(languageCode);
            CultureInfo.CurrentCulture = newCulture;
            CultureInfo.CurrentUICulture = newCulture;
            Resources.Culture = newCulture;

            OnPropertyChanged(string.Empty);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ButtonAdd => Resources.ConfigOption1;
        public string ButtonRefresh => Resources.RefreshJobs;

        public string ButtonConfig => Resources.ConfModeTitle;
        public string ButtonExecuteAll => Resources.ExecuteAllJobs;
        public string ButtonPLay => Resources.PlayJob;
        public string ButtonStop => Resources.StopJob;
        public string ButtonPause => Resources.PauseJob;
        public string ButtonResume => Resources.ResumeJob;
        public string ButtonExecuteOne => Resources.ExecuteOneJob;
        public string JobExecuted => Resources.JobsExecutedSuccess;
        public string JobNotExecuted => Resources.JobsExecutedFail;
        public string JobBeingExecuted => Resources.TitleListBox;
    }
}