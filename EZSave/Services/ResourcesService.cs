using System;
using System.Globalization;
using System.Resources;
using System.Threading;
    using EZSave.Language.Resources;

namespace EZSave.Core.Services
{
    public class ResourcesService
    {

        private readonly ResourceManager _resourceManager;

        public ResourcesService()
        {
            _resourceManager = new ResourceManager(typeof(AppResources));
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key) ?? $"[{key}]";
        }

        public void ChangeLanguage(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }
    }
}
