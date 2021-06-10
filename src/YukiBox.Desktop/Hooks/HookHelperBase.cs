using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace YukiBox.Desktop.Hooks
{
    public abstract class HookHelperBase : ObservableObject
    {
        private Boolean _isEnabled;

        public Boolean IsEnabled
        {
            get => this._isEnabled;
            set
            {
                if (SetProperty(ref this._isEnabled, value))
                {
                    OnIsEnabledChanged();
                }
            }
        }

        private void OnIsEnabledChanged()
        {
            if (this._isEnabled)
            {
                OnEnabled();
            }
            else
            {
                OnDisabled();
            }
        }

        public abstract void Initialize();

        protected abstract void OnEnabled();

        protected abstract void OnDisabled();
    }
}