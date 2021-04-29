using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace YukiBox.Desktop.Controls
{
    public sealed class FluentMessageBox
    {
        private FluentMessageBox()
        {
        }

        public static MessageBoxResult Show(
            String messageBoxText)
        {
            return ShowCore(
                null,
                messageBoxText,
                String.Empty,
                MessageBoxButton.OK,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            String messageBoxText,
            String caption)
        {
            return ShowCore(
                null,
                messageBoxText,
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            String messageBoxText,
            String caption,
            MessageBoxButton button)
        {
            return ShowCore(
                null,
                messageBoxText,
                caption,
                button,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            return ShowCore(
                null,
                messageBoxText,
                caption,
                button,
                icon,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult)
        {
            return ShowCore(
                null,
                messageBoxText,
                caption,
                button,
                icon,
                defaultResult);
        }

        public static MessageBoxResult Show(
            Window owner,
            String messageBoxText)
        {
            return ShowCore(
                owner,
                messageBoxText,
                String.Empty,
                MessageBoxButton.OK,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            Window owner,
            String messageBoxText,
            String caption)
        {
            return ShowCore(
                owner,
                messageBoxText,
                caption,
                MessageBoxButton.OK,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button)
        {
            return ShowCore(
                owner,
                messageBoxText,
                caption,
                button,
                MessageBoxImage.None,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            return ShowCore(
                owner,
                messageBoxText,
                caption,
                button,
                icon,
                MessageBoxResult.None);
        }

        public static MessageBoxResult Show(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult)
        {
            return ShowCore(
                owner,
                messageBoxText,
                caption,
                button,
                icon,
                defaultResult);
        }

        private static MessageBoxResult ShowCore(
            Window owner,
            String messageBoxText,
            String caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            MessageBoxResult defaultResult)
        {
            FluentMessageBoxWindow window = new(
                owner,
                messageBoxText,
                caption,
                button,
                icon,
                defaultResult);
            window.ShowDialog();
            return window.Result;
        }
    }
}