using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;

using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml.Markup;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Helpers
{
    [MarkupExtensionReturnType(ReturnType = typeof(String))]
    public class I18N : MarkupExtension
    {
        public String Code { get; set; }

        public I18N() : base()
        {
        }

        public I18N(String code) : base()
        {
            Code = code;
        }

        protected override Object ProvideValue()
        {
            return !String.IsNullOrEmpty(Code) ? I18NSource.Instance[Code] : String.Empty;
        }
    }

    public class LanguageInfo
    {
        public CultureInfo CultureInfo { get; private set; }

        public String DisplayName { get; private set; }

        public String LanguageName { get; private set; }

        public LanguageInfo(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
            Initialize();
        }

        public LanguageInfo(String cultureName)
        {
            if (!String.IsNullOrEmpty(cultureName))
            {
                CultureInfo = new CultureInfo(cultureName);
            }
            Initialize();
        }

        private void Initialize()
        {
            if (CultureInfo != null)
            {
                DisplayName = $"{CultureInfo.NativeName} - {CultureInfo.EnglishName} [{CultureInfo.Name}]";
                LanguageName = CultureInfo.Name;
            }
            else
            {
                DisplayName = Properties.Strings.System_Language_Default;
                LanguageName = String.Empty;
            }
        }

        public override String ToString()
        {
            return DisplayName;
        }
    }

    public class I18NSource
    {
        private static readonly Lazy<I18NSource> lazy = new(() => new I18NSource());

        public static I18NSource Instance { get { return lazy.Value; } }

        public String this[String key]
        {
            get
            {
                if (CurrentLanguage?.CultureInfo is null)
                {
                    var cultureInfo = CultureInfo.CurrentUICulture;
                    return this.resManager.GetString(key, cultureInfo);
                }
                else
                {
                    return this.resManager.GetString(key, CurrentLanguage?.CultureInfo);
                }
            }
        }

        private readonly ResourceManager resManager = Properties.Strings.ResourceManager;

        private LanguageInfo _currentLanguage;

        public LanguageInfo CurrentLanguage
        {
            get => this._currentLanguage;
            set
            {
                if (this._currentLanguage != value)
                {
                    this._currentLanguage = value;
                    ConfigHelper.CurrentConfig.System.Language = value.LanguageName;
                }
            }
        }

        public ObservableCollection<LanguageInfo> SupportedLanguages { get; private set; }

        private I18NSource()
        {
        }

        public void Initialize()
        {
            var systemUICulture = CultureInfo.CurrentUICulture;
            SupportedLanguages = GetAllSupportedLanguages();
            //var language = systemUICulture.Name;
            var language = ConfigHelper.CurrentConfig.System.Language;
            CurrentLanguage = SupportedLanguages.FirstOrDefault(x => x.LanguageName == language);
            if (CurrentLanguage is null)
            {
                CurrentLanguage = SupportedLanguages[0];
            }
        }

        private static ObservableCollection<LanguageInfo> GetAllSupportedLanguages()
        {
            var supportedLanguages = new ObservableCollection<LanguageInfo>
            {
                new LanguageInfo(String.Empty)
            };

            var resourceManager = Properties.Strings.ResourceManager;
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (var culture in cultures)
            {
                try
                {
                    if (culture.Equals(CultureInfo.InvariantCulture))
                    {
                        continue;
                    }

                    var resourceSet = resourceManager.GetResourceSet(culture, true, false);
                    if (resourceSet != null)
                    {
                        supportedLanguages.Add(new LanguageInfo(culture));
                    }
                }
                catch (CultureNotFoundException)
                {
                }
            }

            return supportedLanguages;
        }
    }
}