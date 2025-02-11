using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EZSave.Core.Services;

namespace EZSave.GUI
{
    public class LanguageViewModel : INotifyPropertyChanged
    {
        private readonly ResourcesService _resourcesService;
        private LanguageItem _currentLanguage;

        public event PropertyChangedEventHandler? PropertyChanged;

        // Liste des langues disponibles
        public List<LanguageItem> LanguageOptions { get; } = new()
        {
            new LanguageItem("Français", "fr"),
            new LanguageItem("English", "en")
        };

        public LanguageItem CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    ChangeLanguage(value.CultureCode);
                    OnPropertyChanged(nameof(CurrentLanguage));
                }
            }
        }

        public LanguageViewModel()
        {
            _resourcesService = new ResourcesService();

            _currentLanguage = LanguageOptions.FirstOrDefault(l => l.CultureCode == "fr") ?? LanguageOptions[0];

            ChangeLanguage(_currentLanguage.CultureCode);
        }

        private void ChangeLanguage(string cultureCode)
        {
            _resourcesService.ChangeLanguage(cultureCode);  

            OnPropertyChanged(nameof(CurrentLanguage));
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class LanguageItem
    {
        public string DisplayName { get; }
        public string CultureCode { get; }

        public LanguageItem(string displayName, string cultureCode)
        {
            DisplayName = displayName;
            CultureCode = cultureCode;
        }

        public override string ToString() => DisplayName; 
    }
}
