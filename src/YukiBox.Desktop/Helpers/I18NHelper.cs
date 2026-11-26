using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;

using Microsoft.UI.Xaml.Markup;

namespace YukiBox.Desktop.Helpers;

[MarkupExtensionReturnType(ReturnType = typeof(string))]
public class I18N : MarkupExtension
{
    public string Code { get; set; }

    public I18N()
    {
    }

    public I18N(string code)
    {
        Code = code;
    }

    protected override object ProvideValue()
    {
        string value = !string.IsNullOrEmpty(Code) ? I18NSource.Instance[Code] : string.Empty;
        return value;
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

    public override string ToString()
    {
        return DisplayName;
    }

    private void Initialize()
    {
        if (CultureInfo is not null)
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
}

public class I18NSource
{
    private static readonly Lazy<I18NSource> s_lazy = new(() => new I18NSource());

    public static I18NSource Instance => s_lazy.Value;

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

    public string this[string key]
    {
        get
        {
            if (CurrentLanguage?.CultureInfo is null)
            {
                CultureInfo cultureInfo = CultureInfo.CurrentUICulture;
                string value = _resManager.GetString(key, cultureInfo);
                return value;
            }
            else
            {
                string value = _resManager.GetString(key, CurrentLanguage?.CultureInfo);
                return value;
            }
        }
    }

    private readonly ResourceManager _resManager = Properties.Strings.ResourceManager;

    private I18NSource()
    {
    }

    public void Initialize()
    {
        CultureInfo systemUICulture = CultureInfo.CurrentUICulture;
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
        ObservableCollection<LanguageInfo> supportedLanguages =
        [
            new(string.Empty)
        ];

        ResourceManager resourceManager = Properties.Strings.ResourceManager;
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (CultureInfo culture in cultures)
        {
            try
            {
                if (culture.Equals(CultureInfo.InvariantCulture))
                {
                    continue;
                }

                ResourceSet resourceSet = resourceManager.GetResourceSet(culture, true, false);
                if (resourceSet is not null)
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
