using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindowStudy.Controls
{
    public class WindowTitleBarButton : Button, IHitTestElement
    {
        static WindowTitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitleBarButton), new FrameworkPropertyMetadata(typeof(WindowTitleBarButton)));
        }

        public HitTestValues ReservedHitTest
        {
            get { return (HitTestValues)GetValue(ReservedHitTestProperty); }
            set { SetValue(ReservedHitTestProperty, value); }
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

        public static readonly DependencyProperty ReservedHitTestProperty
            = DependencyProperty.Register("ReservedHitTest", typeof(HitTestValues), typeof(WindowTitleBarButton), new UIPropertyMetadata(HitTestValues.HTNOWHERE));
        public static readonly DependencyProperty IsMouseOverByHitTestProperty = DependencyProperty.Register("IsMouseOverByHitTest", typeof(bool), typeof(WindowTitleBarButton), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsPressedByHitTestProperty = DependencyProperty.Register("IsPressedByHitTest", typeof(bool), typeof(WindowTitleBarButton), new UIPropertyMetadata(false));

    }
}
