using Microsoft.UI.Xaml.Controls;

namespace YukiBox.Desktop.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void Show();

        void CloseWindow();
    }
}