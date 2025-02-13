using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Markup;
using System.Windows;
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

        public void ChangeLanguage(string cultureCode)
        {
            // Modifier la culture en cours
            var culture = new CultureInfo(cultureCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            // Charger les ressources correspondantes
            var currentApp = Application.Current;
            currentApp.Resources.MergedDictionaries.Clear();

            // Charger les dictionnaires de ressources en fonction de la culture
            var resourceUri = new Uri($"Resources/{cultureCode}.xaml", UriKind.Relative);
            currentApp.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = resourceUri });

            // Optionnel : mettre à jour la langue des fenêtres si nécessaire
            foreach (Window window in Application.Current.Windows)
            {
                window.Language = XmlLanguage.GetLanguage(culture.Name);
            }
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
