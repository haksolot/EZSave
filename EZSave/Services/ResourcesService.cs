using System.Resources;
using System.Globalization;

namespace EZSave.Core.Services
{
    public class ResourcesService
    {
        private readonly ResourceManager _resourceManager;

        public ResourcesService()
        {
            _resourceManager = new ResourceManager("EZSave.Language.Resources.Resources",
                typeof(ResourcesService).Assembly);
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key) ?? $"[{key}]"; // Retourne la clé si non trouvée
        }

        public void SetLanguage(string languageCode)
        {
            CultureInfo culture = new CultureInfo(languageCode);
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
    }
}
