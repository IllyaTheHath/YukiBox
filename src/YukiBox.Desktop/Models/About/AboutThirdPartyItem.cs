using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Models
{
    public class AboutThirdPartyItem
    {
        public String ProjectName { get; set; }

        public String Website { get; set; }

        public String LicenseUrl { get; set; }

        public AboutThirdPartyItem()
        {
        }

        public AboutThirdPartyItem(String projectName, String website, String licenseUrl)
        {
            ProjectName = projectName;
            Website = website;
            LicenseUrl = licenseUrl;
        }
    }
}