using EZSave.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZSave.TUI.ViewModels;
using System.Globalization;


namespace EZSave.Tests
{
    public class LanguageTest
    {
        private readonly ResourcesService _resourcesService;

        public LanguageTest()
        {
            _resourcesService = new ResourcesService();
        }

        [Fact]
        public void GetString_ReturnExpectedValue()
        {
            
            string key = "WelcomeMessage"; 
            string expectedValue = "Bonjour";
            
            string result = _resourcesService.GetString(key);

            
            Assert.Equal(expectedValue, result); 
        }

        [Fact]
        public void GetString_ShouldReturnKey_WhenKeyNotFound()
        {
            // Arrange
            string key = "NonExistingKey"; // Une clé qui n'existe pas
            string expectedValue = "[NonExistingKey]"; // Si la clé n'existe pas, la méthode doit retourner la clé entre crochets

            // Act
            string result = _resourcesService.GetString(key);

            // Assert
            Assert.Equal(expectedValue, result); // Vérifie que la valeur retournée est bien la clé entre crochets
        }

        [Fact]
        public void SetLanguage_ShouldChangeCulture()
        {
            // Arrange
            string languageCode = "fr-FR"; // Code de langue pour la culture française
            string expectedLanguage = "fr"; // Culture courante attendue après la définition

            // Act
            _resourcesService.SetLanguage(languageCode);

            // Assert
            Assert.Equal(expectedLanguage, CultureInfo.CurrentCulture.TwoLetterISOLanguageName); // Vérifie que la culture actuelle est bien le français
        }

        [Fact]
        public void SetLanguage_ShouldNotChangeCulture_WhenInvalidCode()
        {
            // Arrange
            string invalidLanguageCode = "invalid"; // Un code de langue invalide
            string originalLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName; // Récupérer la langue originale

            // Act
            _resourcesService.SetLanguage(invalidLanguageCode);

            // Assert
            Assert.Equal(originalLanguage, CultureInfo.CurrentCulture.TwoLetterISOLanguageName); // Vérifie que la culture n'a pas changé avec un code de langue invalide
        }
    
}
}
