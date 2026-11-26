using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;

using Microsoft.UI.Xaml.Markup;

namespace YukiBox.Desktop.Helpers;

[MarkupExtensionReturnType(ReturnType = typeof(string))]
public class I18N : MarkupExtension
{
    public string Code { get; set; }

    public I18N() : base()
    {
    }

    public I18N(string code) : base()
    {
        Code = code;
    }

    protected override object ProvideValue()
    {
        return !string.IsNullOrEmpty(Code) ? I18NSource.Instance[Code] : string.Empty;
    }
}

public class LanguageInfo
{
    public CultureInfo CultureInfo { get; private set; }

    public string DisplayName { get; private set; }

    public string LanguageName { get; private set; }

    public LanguageInfo(CultureInfo cultureInfo)
    {
        CultureInfo = cultureInfo;
        Initialize();
    }

    public LanguageInfo(string cultureName)
    {
        if (!string.IsNullOrEmpty(cultureName))
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
            LanguageName = string.Empty;
        }
    }

    public override string ToString()
    {
        return DisplayName;
    }
}

public class I18NSource
{
    private static readonly Lazy<I18NSource> s_lazy = new(() => new I18NSource());

    public static I18NSource Instance { get { return s_lazy.Value; } }

    public string this[string key]
    {
        get
        {
            if (CurrentLanguage?.CultureInfo is null)
            {
                var cultureInfo = CultureInfo.CurrentUICulture;
                return _resManager.GetString(key, cultureInfo);
            }
            else
            {
                return _resManager.GetString(key, CurrentLanguage?.CultureInfo);
            }
        }
    }

    private readonly ResourceManager _resManager = Properties.Strings.ResourceManager;

    private LanguageInfo _currentLanguage;

    public LanguageInfo CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage != value)
            {
                _currentLanguage = value;
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
        string language = ConfigHelper.CurrentConfig.System.Language;
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
            new(string.Empty)
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
