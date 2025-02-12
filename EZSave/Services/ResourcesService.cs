using EZSave.Language.Resources;
using System.Collections;
using System.Globalization;
using System.Resources;


namespace EZSave.Core.Services
{
    public class ResourcesService
    {

        private readonly ResourceManager _resourceManager;
        private readonly Dictionary<string, string> _resourcesCache;
        private CultureInfo _currentCulture;

        public ResourcesService()
        {
            _resourcesCache = new Dictionary<string, string>();
            _resourceManager = new ResourceManager(typeof(AppResources));

            _currentCulture = new CultureInfo("fr");

            LoadResources("fr");
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key) ?? $"[{key}]";
        }

        public CultureInfo CurrentCulture
        {
            get => _currentCulture;
            set
            {
                if (_currentCulture != value)
                {
                    _currentCulture = value;
                    LoadResources(value.Name);  
                }
            }
        }


       

        public void LoadResources(string cultureCode)
        {


            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);

            _resourcesCache.Clear();

            foreach (DictionaryEntry entry in _resourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true))
            {
                _resourcesCache.Add(entry.Key.ToString(), entry.Value.ToString());
            }
        }

        public void ChangeLanguage(string cultureCode)
        {

            CurrentCulture = new CultureInfo(cultureCode);  
        }
    }
}