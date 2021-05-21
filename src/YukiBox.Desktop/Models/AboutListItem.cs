using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YukiBox.Desktop.Models
{
    public class AboutListItem
    {
        public String Title { get; set; }

        public String Description { get; set; }

        public String Glyph { get; set; }

        public Boolean OpenUrl { get; set; }

        public String Url { get; set; }

        public AboutListItem()
        {
        }

        public AboutListItem(String title, String des, String glyph, Boolean openUrl = false, String url = "")
        {
            Title = title;
            Description = des;
            Glyph = glyph;
            OpenUrl = openUrl;
            Url = url;
        }
    }
}