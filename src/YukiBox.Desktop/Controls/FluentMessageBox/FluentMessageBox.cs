using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

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
            window.Activate();
            return window.Result;
        }
    }

    public enum MessageBoxResult
    {
        None = 0,
        OK = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }

    public enum MessageBoxButton
    {
        OK = 0,
        OKCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }

    public enum MessageBoxImage
    {
        None = 0,
        Error = 0x10,
        Hand = 0x10,
        Stop = 0x10,
        Question = 0x20,
        Exclamation = 48,
        Warning = 48,
        Asterisk = 0x40,
        Information = 0x40
    }
}