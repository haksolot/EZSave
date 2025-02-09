using Xunit;
using EZSave.Core.Services;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace EZSave.Tests
{
    public class LanguageTest
    {
        private readonly ResourceManager _resourcesManager;

        public LanguageTest()
        {
            _resourcesManager = new ResourceManager("EZSave.Language.Resources.AppResources", typeof(EZSave.Language.Resources.AppResources).Assembly);
        }

        [Fact]
        public void GetString_ShouldReturnEnglishValue_WhenCultureIsEnglish()
        {
            // Arrange
            CultureInfo.CurrentUICulture = new CultureInfo("en");

            // Act
            string result = _resourcesManager.GetString("WelcomeMessage");

            // Assert
            Assert.Equal("Hello !", result);
        }

        [Fact]
        public void GetString_ShouldReturnFrenchValue_WhenCultureIsFrench()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("fr");

            string result = _resourcesManager.GetString("WelcomeMessage");

            Assert.Equal("Bonjour !", result);
        }

        [Fact]
        public void GetString_ShouldReturnNull_WhenResourceDoesNotExist()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en");

            string result = _resourcesManager.GetString("NonExistingKey");

            Assert.Null(result);
        }
        [Fact]
        public void GetString_ShouldFallbackToDefaultCulture_WhenSpecificCultureIsMissing()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("es"); 

            string result = _resourcesManager.GetString("WelcomeMessage");

            Assert.Equal("Welcome to EasySave version: ", result); 
        }
    }
}




   

