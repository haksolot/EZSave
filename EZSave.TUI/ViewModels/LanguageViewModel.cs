using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EZSave.Core.Services;

namespace EZSave.TUI.ViewModels
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        private readonly ResourcesService _resourcesService;
        private string _currentLanguage;
        private string _translatedText;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Constructeur
        public LanguageViewModel()
        {
            _resourcesService = new ResourcesService();
            _currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // Langue initiale
            _translatedText = _resourcesService.GetString("WelcomeMessage"); // Message par défaut
        }

        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged();
                    ChangeLanguage(_currentLanguage); 
                }
            }
        }

        public string TranslatedText
        {
            get => _translatedText;
            set
            {
                if (_translatedText != value)
                {
                    _translatedText = value;
                    OnPropertyChanged();
                }
            }
        }

        public void ChangeLanguage(string languageCode)
        {
            _resourcesService.SetLanguage(languageCode);  
            TranslatedText = _resourcesService.GetString("WelcomeMessage"); 
        }

      
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
