using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindowStudy.Controls
{
    public class TitleBarButton : Button
    {
        static TitleBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TitleBarButton), new FrameworkPropertyMetadata(typeof(TitleBarButton)));
        }
    }
}
