using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Threading;
    using EZSave.Language.Resources;

namespace EZSave.Core.Services
{
    public class ResourcesService
    {

        private readonly ResourceManager _resourceManager;
        private readonly Dictionary<string, string> _resourcesCache;
        public ResourcesService()
        {
            _resourcesCache = new Dictionary<string, string>();
            _resourceManager = new ResourceManager(typeof(AppResources));
            LoadResources("fr");
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key) ?? $"[{key}]";
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
            LoadResources(cultureCode);
        }
    }
}
