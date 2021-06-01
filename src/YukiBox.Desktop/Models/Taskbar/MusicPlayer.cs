using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public abstract class MusicPlayer : ObservableObject
    {
        public virtual String Name => this.GetType().AssemblyQualifiedName;

        protected abstract String DisplayNameResourceName { get; }

        private String _displayName;

        public String DisplayName
        {
            get => this._displayName;
            set => SetProperty(ref this._displayName, value);
        }

        public MusicPlayer()
        {
            UpdateDisplayName();
        }

        public abstract String GetMusicName();

        public override String ToString()
        {
            return DisplayName;
        }

        public void UpdateDisplayName()
        {
            DisplayName = I18NSource.Instance[DisplayNameResourceName];
        }
    }
}