using CommunityToolkit.Mvvm.ComponentModel;

namespace YukiBox.Desktop.Hooks;

public abstract class HookHelperBase : ObservableObject
{
    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (SetProperty(ref _isEnabled, value))
            {
                OnIsEnabledChanged();
            }
        }
    }

    private void OnIsEnabledChanged()
    {
        if (_isEnabled)
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
