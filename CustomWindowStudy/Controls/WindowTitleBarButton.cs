using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindowStudy.Controls
{
    public class WindowTitleBarButton : Button
    {
        static WindowTitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitleBarButton), new FrameworkPropertyMetadata(typeof(WindowTitleBarButton)));
        }

        public WindowTitleBarButtonFlag ButtonFlag
        {
            get { return (WindowTitleBarButtonFlag)GetValue(ButtonFlagProperty); }
            set { SetValue(ButtonFlagProperty, value); }
        }
        public bool IsMouseOverByHitTest
        {
            get { return (bool)GetValue(IsMouseOverByHitTestProperty); }
            set { SetValue(IsMouseOverByHitTestProperty, value); }
        }
        public bool IsPressedByHitTest
        {
            get { return (bool)GetValue(IsPressedByHitTestProperty); }
            set { SetValue(IsPressedByHitTestProperty, value); }
        }

        public static readonly DependencyProperty ButtonFlagProperty
            = DependencyProperty.Register("ButtonFlag", typeof(WindowTitleBarButtonFlag), typeof(WindowTitleBarButton), new UIPropertyMetadata(WindowTitleBarButtonFlag.None));
        public static readonly DependencyProperty IsMouseOverByHitTestProperty = DependencyProperty.Register("IsMouseOverByHitTest", typeof(bool), typeof(WindowTitleBarButton), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsPressedByHitTestProperty = DependencyProperty.Register("IsPressedByHitTest", typeof(bool), typeof(WindowTitleBarButton), new UIPropertyMetadata(false));

    }
}
