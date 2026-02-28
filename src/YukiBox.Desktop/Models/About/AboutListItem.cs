using System;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

namespace YukiBox.Desktop.Models
{
    public class AboutListItem
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Glyph { get; set; }

        public bool OpenUrl { get; set; }

        public string Url { get; set; }

        private ICommand _openUrlCommand;
        public ICommand OpenUrlCommand => this._openUrlCommand ??= new RelayCommand(OpenUrl_);

        public AboutListItem()
        {
        }

        public AboutListItem(string title, string des, string glyph, bool openUrl = false, string url = "")
        {
            Title = title;
            Description = des;
            Glyph = glyph;
            OpenUrl = openUrl;
            Url = url;
        }


        private void OpenUrl_()
        {
            if(OpenUrl && !string.IsNullOrEmpty(Url))
            {
                System.Diagnostics.Process.Start(Url);
            }
        }
    }
}