namespace YukiBox.Desktop.Models;

public class AboutThirdPartyItem
{
    public string ProjectName { get; set; }

    public string Website { get; set; }

    public string LicenseUrl { get; set; }

    public AboutThirdPartyItem()
    {
    }

    public AboutThirdPartyItem(string projectName, string website, string licenseUrl)
    {
        ProjectName = projectName;
        Website = website;
        LicenseUrl = licenseUrl;
    }
}
