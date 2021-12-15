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
        public String Name { get; set; }

        public I18N() : base()
        {
        }

        public I18N(String name) : base()
        {
            Name = name;
        }

        protected override Object ProvideValue()
        {
            return !String.IsNullOrEmpty(Name) ? I18NSource.Instance[Name] : String.Empty;
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

    public class I18NSource : INotifyPropertyChanged
    {
        private readonly IMediatorService _mediatorService;

        private static readonly Lazy<I18NSource> lazy = new(() => new I18NSource());

        public static I18NSource Instance { get { return lazy.Value; } }

        public String this[String key]
        {
            get
            {
                if (CurrentLanguage?.CultureInfo is null)
                {
                    var cultureInfo = CultureInfo.CurrentUICulture;
                    var j = CultureInfo.GetCultureInfo("jp");
                    var a = this.resManager.GetString(key, cultureInfo);
                    return this.resManager.GetString(key, cultureInfo);
                }
                else
                {
                    return this.resManager.GetString(key, CurrentLanguage?.CultureInfo);
                }
            }
        }

        public static String Get(String key)
        {
            return Instance[key];
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
                    this._mediatorService.BroadcastMessage("I18N", null);
                    ConfigHelper.CurrentConfig.System.Language = value.LanguageName;
                }
            }
        }

        public ObservableCollection<LanguageInfo> SupportedLanguages { get; private set; }

        private I18NSource()
        {
            this._mediatorService = Ioc.Default.GetService<IMediatorService>();
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