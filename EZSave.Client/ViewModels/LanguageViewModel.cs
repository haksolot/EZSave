using EZSave.Client.Properties;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace EZSave.Client.ViewModels
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
        public string ButtonPlay => Resources.Play;
        public string ButtonPause => Resources.Pause;
        public string ButtonStop => Resources.Stop;
        public string ButtonExecuteOne => Resources.ExecuteOneJob;
        public string JobExecuted => Resources.JobsExecutedSuccess;
        public string JobNotExecuted => Resources.JobsExecutedFail;
        public string JobBeingExecuted => Resources.TitleListBox;
    }
}
