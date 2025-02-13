using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EZSave.GUI.Commands;

namespace EZSave.GUI.ViewModels
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand ChangeLanguageCommand { get; }

        private string _currentLanguage;
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    ChangeLanguage(value);
                    OnPropertyChanged();
                }
            }
        }

        public LanguageViewModel()
        {
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
            _currentLanguage = CultureInfo.CurrentUICulture.Name;
        }

        public void RefreshLanguage()
        {
            OnPropertyChanged(nameof(CurrentLanguage));
        }

        private void ChangeLanguage(string languageCode)
        {
            try
            {
                var newCulture = new CultureInfo(languageCode);
                CultureInfo.CurrentCulture = newCulture;
                CultureInfo.CurrentUICulture = newCulture;

                Properties.Resources.Culture = newCulture;

                foreach (Window window in Application.Current.Windows)
                {
                    window.Language = XmlLanguage.GetLanguage(newCulture.Name);
                }

                OnPropertyChanged(nameof(CurrentLanguage));
            }
            catch (CultureNotFoundException)
            {
                MessageBox.Show("Langue non supportée.");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
