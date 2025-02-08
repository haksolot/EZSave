using EZSave.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZSave.TUI.ViewModels;


namespace EZSave.Tests
{
    public class LanguageTest
    {
        [Fact]
        public void ChangeLanguage_ShouldUpdateTranslatedText_WhenLanguageChanged()
        {
            // Arrange
            var viewModel = new LanguageViewModel();

            // Act
            viewModel.ChangeLanguage("en"); // Change to English
            var translatedText = viewModel.TranslatedText;

            // Assert
            Assert.Equal("Hello World", translatedText); // Check if the translated text is updated correctly
        }

        [Fact]
        public void CurrentLanguage_ShouldChange_WhenNewLanguageIsSet()
        {
            // Arrange
            var viewModel = new LanguageViewModel();

            // Act
            viewModel.CurrentLanguage = "fr"; // Set language to French
            var currentLanguage = viewModel.CurrentLanguage;

            // Assert
            Assert.Equal("fr", currentLanguage); // Check if the current language is correctly set
        }
    }
}
