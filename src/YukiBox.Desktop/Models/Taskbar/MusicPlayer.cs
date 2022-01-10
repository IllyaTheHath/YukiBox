using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public abstract class MusicPlayer
    {
        public virtual String Name => this.GetType().AssemblyQualifiedName;

        protected abstract String DisplayNameResourceName { get; }

        public String DisplayName => I18NSource.Instance[DisplayNameResourceName];

        public abstract String GetMusicName();

        public override String ToString()
        {
            return DisplayName;
        }
    }
}