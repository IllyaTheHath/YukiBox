using System;

using Microsoft.UI.Xaml.Controls;

namespace YukiBox.Desktop.Contracts.Services
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }

        void Initialize(Frame shellFrame);

        void NavigateTo(string pageKey);

        void NavigateTo(string pageKey, object parameter);
    }
}