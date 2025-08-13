using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomWindowStudy.Controls
{
    public enum WindowGlowBarPosition { Left, Top, Right, Bottom }

    public partial class WindowGlowBar : Window
    {
        public WindowGlowBar()
        {
            InitializeComponent();
        }

        public WindowGlowBarPosition Position
        {
            get { return (WindowGlowBarPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(WindowGlowBarPosition), typeof(WindowGlowBar), new UIPropertyMetadata(WindowGlowBarPosition.Left));
    }
}
