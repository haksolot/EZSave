using EZSave.GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace EZSave.GUI.ViewModels
{
    public class LanguageViewModel: INotifyPropertyChanged
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
        public string ButtonExecuteOne => Resources.ExecuteOneJob;

        public string JobBeingExecuted => Resources.TitleListBox;
    }
}
