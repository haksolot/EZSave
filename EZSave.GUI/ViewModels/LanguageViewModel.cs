using EZSave.GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace EZSave.GUI.ViewModels
{
    public class LanguageViewModel
    {
        public ICommand ChangeLanguageCommand { get; }

        public LanguageViewModel()
        {
            ChangeLanguageCommand = new RelayCommand<string>(ChangeLanguage);
        }

        private void ChangeLanguage(string languageCode)
        {
            try
            {
                CultureInfo newCulture = new CultureInfo(languageCode);
                CultureInfo.CurrentCulture = newCulture;
                CultureInfo.CurrentUICulture = newCulture;
                Resources.Culture = newCulture;

                foreach (var window in Application.Current.Windows)
                {
                    if (window is Window w)
                    {
                        w.Language = XmlLanguage.GetLanguage(newCulture.Name);

                        // 🔥 Mise à jour des bindings dynamiques
                        BindingOperations.GetBindingExpressionBase(w, Window.LanguageProperty)?.UpdateTarget();
                    }
                }
            }
            catch (CultureNotFoundException)
            {
            }
        }



        public string LocalizedText => Resources.ConfigOption1;
    }
}
