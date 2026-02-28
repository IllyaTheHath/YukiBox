using System;

using YukiBox.Desktop.Helpers;

namespace YukiBox.Desktop.Models
{
    public abstract class MusicPlayer
    {
        public virtual string Name => this.GetType().AssemblyQualifiedName;

        protected abstract string DisplayNameResourceName { get; }

        public string DisplayName => I18NSource.Instance[DisplayNameResourceName];

        public abstract string GetMusicName();

        public override string ToString()
        {
            return DisplayName;
        }
    }
}