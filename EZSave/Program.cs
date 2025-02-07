using System.Globalization;
using EZSave.Language;

namespace EZSave
{
    internal class Program
    {
        
            static void Main()
            {
                // Définir la langue par défaut
                SetLanguage("fr");
                Console.WriteLine(Resources.WelcomeMessage); // Affiche "Bienvenue dans notre application !"

                // Changer de langue dynamiquement
                SetLanguage("en");
                Console.WriteLine(Resources.WelcomeMessage); // Affiche "Welcome to our application!"
            }

            static void SetLanguage(string culture)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
        
    }
}
