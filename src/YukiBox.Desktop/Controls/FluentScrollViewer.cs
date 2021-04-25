using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace YukiBox.Desktop.Controls
{
    /// <summary>
    /// <see cref="https://github.com/ModernFlyouts-Community/ModernFlyouts/blob/main/ModernFlyouts/Helpers/ScrollViewerHelperEx.cs" />
    /// </summary>
    public static class FluentScrollViewer
    {
        #region Enabled

        public static Boolean GetEnabled(ScrollViewer element)
        {
            return (Boolean)element.GetValue(EnabledProperty);
        }

        public static void SetEnabled(ScrollViewer element, Boolean value)
        {
            element.SetValue(EnabledProperty, value);
        }

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(Boolean), typeof(FluentScrollViewer), new PropertyMetadata(false, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sv = d as ScrollViewer;
            sv.AddHandler(UIElement.PreviewMouseWheelEvent, new MouseWheelEventHandler(Sv_ScrollChanged));
        }

        #endregion Enabled

        #region IsAnimating

        internal static readonly DependencyProperty IsAnimatingProperty =
            DependencyProperty.RegisterAttached(
                "IsAnimating",
                typeof(Boolean),
                typeof(FluentScrollViewer),
                new PropertyMetadata(false));

        private static Boolean GetIsAnimating(ScrollViewer scrollViewer)
        {
            return (Boolean)scrollViewer.GetValue(IsAnimatingProperty);
        }

        private static void SetIsAnimating(ScrollViewer scrollViewer, Boolean value)
        {
            scrollViewer.SetValue(IsAnimatingProperty, value);
        }

        #endregion IsAnimating

        #region CurrentVerticalOffset

        internal static readonly DependencyProperty CurrentVerticalOffsetProperty =
            DependencyProperty.RegisterAttached("CurrentVerticalOffset",
                typeof(Double),
                typeof(FluentScrollViewer),
                new PropertyMetadata(0.0, OnCurrentVerticalOffsetChanged));

        private static Double GetCurrentVerticalOffset(ScrollViewer scrollViewer)
        {
            return (Double)scrollViewer.GetValue(CurrentVerticalOffsetProperty);
        }

        private static void SetCurrentVerticalOffset(ScrollViewer scrollViewer, Double value)
        {
            scrollViewer.SetValue(CurrentVerticalOffsetProperty, value);
        }

        private static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer ctl && e.NewValue is Double v)
            {
                ctl.ScrollToVerticalOffset(v);
            }
        }

        #endregion CurrentVerticalOffset

        #region CurrentHorizontalOffset

        internal static readonly DependencyProperty CurrentHorizontalOffsetProperty =
            DependencyProperty.RegisterAttached("CurrentHorizontalOffset",
                typeof(Double),
                typeof(FluentScrollViewer),
                new PropertyMetadata(0.0, OnCurrentHorizontalOffsetChanged));

        private static Double GetCurrentHorizontalOffset(ScrollViewer scrollViewer)
        {
            return (Double)scrollViewer.GetValue(CurrentHorizontalOffsetProperty);
        }

        private static void SetCurrentHorizontalOffset(ScrollViewer scrollViewer, Double value)
        {
            scrollViewer.SetValue(CurrentHorizontalOffsetProperty, value);
        }

        private static void OnCurrentHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer ctl && e.NewValue is Double v)
            {
                ctl.ScrollToHorizontalOffset(v);
            }
        }

        #endregion CurrentHorizontalOffset

        private static void Sv_ScrollChanged(Object sender, MouseWheelEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;

            Boolean isHorizontal = Keyboard.Modifiers == ModifierKeys.Shift;

            if (!isHorizontal)
            {
                if (!GetIsAnimating(scrollViewer))
                {
                    SetCurrentVerticalOffset(scrollViewer, scrollViewer.VerticalOffset);
                }

                Double _totalVerticalOffset = Math.Min(Math.Max(0, scrollViewer.VerticalOffset - e.Delta), scrollViewer.ScrollableHeight);
                ScrollToVerticalOffset(scrollViewer, _totalVerticalOffset);
            }
            else
            {
                if (!GetIsAnimating(scrollViewer))
                {
                    SetCurrentHorizontalOffset(scrollViewer, scrollViewer.HorizontalOffset);
                }

                Double _totalHorizontalOffset = Math.Min(Math.Max(0, scrollViewer.HorizontalOffset - e.Delta), scrollViewer.ScrollableWidth);
                ScrollToHorizontalOffset(scrollViewer, _totalHorizontalOffset);
            }

            e.Handled = true;
        }

        public static void ScrollToOffset(ScrollViewer scrollViewer, Orientation orientation, Double offset, Double duration = 500, IEasingFunction easingFunction = null)
        {
            var animation = new DoubleAnimation(offset, TimeSpan.FromMilliseconds(duration));
            easingFunction ??= new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.EasingFunction = easingFunction;
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e1) =>
            {
                if (orientation == Orientation.Vertical)
                {
                    SetCurrentVerticalOffset(scrollViewer, offset);
                }
                else
                {
                    SetCurrentHorizontalOffset(scrollViewer, offset);
                }
                SetIsAnimating(scrollViewer, false);
            };
            SetIsAnimating(scrollViewer, true);

            scrollViewer.BeginAnimation(orientation == Orientation.Vertical ? CurrentVerticalOffsetProperty : CurrentHorizontalOffsetProperty, animation, HandoffBehavior.Compose);
        }

        public static void ScrollToVerticalOffset(ScrollViewer scrollViewer, Double offset, Double duration = 500, IEasingFunction easingFunction = null)
        {
            ScrollToOffset(scrollViewer, Orientation.Vertical, offset, duration, easingFunction);
        }

        public static void ScrollToHorizontalOffset(ScrollViewer scrollViewer, Double offset, Double duration = 500, IEasingFunction easingFunction = null)
        {
            ScrollToOffset(scrollViewer, Orientation.Horizontal, offset, duration, easingFunction);
        }
    }
}