using EZSave.Core.Services;
using System.ComponentModel.Design;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Here");
        // Test du ResourceService
        //TestResourceService();
    }

    //static void TestResourceService()
    //{
    //    // Instancier le service de ressources
    //    ResourcesService resourceService = new ResourcesService();

    //    // Tester en récupérant une ressource avec une clé donnée
    //    string welcomeMessage = resourceService.GetString("WelcomeMessage");

    //    // Afficher le résultat pour vérifier
    //    Console.WriteLine($"Message bien chargé: {welcomeMessage}");

    //    // Essayer de changer la langue
    //    resourceService.SetLanguage("en"); // Changer la langue en anglais
    //    string welcomeMessageEn = resourceService.GetString("WelcomeMessage");
    //    Console.WriteLine($"Message en anglais: {welcomeMessageEn}");
    //}
}
