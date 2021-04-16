using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;

using Microsoft.Toolkit.Mvvm.DependencyInjection;

using YukiBox.Desktop.Contracts.Services;

namespace YukiBox.Desktop.Helpers
{
    public class I18NExtension : Binding
    {
        public I18NExtension(String name) : base("[" + name + "]")
        {
            Mode = BindingMode.OneWay;
            Source = I18NSource.Instance;
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
                var a = this.resManager.GetString(key, CurrentLanguage?.CultureInfo);
                return a;
            }
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
            var language = systemUICulture.Name;
            CurrentLanguage = SupportedLanguages.FirstOrDefault(x => x.LanguageName == language);
            if(CurrentLanguage is null)
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
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
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
