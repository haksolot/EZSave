using EZSave.Core.Services;
using System.Globalization;  
using System.Threading;     
using System.Resources;

namespace EZSave.Tests
{
    public class LanguageTestGUI
    {
        private readonly ResourceManager _resourcesManager;
        [Fact]
        public void TestLoadResources()
        {
            var resourcesService = new ResourcesService();

            resourcesService.LoadResources("fr");

            var result = resourcesService.GetString("MainOption1");
            Assert.NotNull(result);
            Assert.Equal("Exécuter les jobs", result);
        }

        [Fact]
        public void TestChangeLanguage()
        {
            var resourcesService = new ResourcesService();

            resourcesService.ChangeLanguage("en");

            var result = resourcesService.GetString("MainOption1");
            Assert.NotNull(result);
            Assert.Equal("Execute jobs", result);  
        }
    }
}
