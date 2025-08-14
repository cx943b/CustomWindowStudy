using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindowStudy.Controls
{
    public class WindowTitleBar : ContentControl
    {
        FrameworkElement? _PART_SysIcon;
        Dictionary<FrameworkElement, HitTestValues> _dicHitTestValues = new Dictionary<FrameworkElement, HitTestValues>();

        static WindowTitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitleBar), new FrameworkPropertyMetadata(typeof(WindowTitleBar)));
        }

        public HitTestValues CheckHitTest(Point srcPos)
        {
            Point point = PointFromScreen(srcPos);
            Point sysIconPos = _PART_SysIcon.TranslatePoint(new Point(0, 0), this);
        }

        public override void OnApplyTemplate()
        {
            _PART_SysIcon = GetTemplateChild("PART_SysIcon") as FrameworkElement;
            if (_PART_SysIcon != null)
            {
                _dicHitTestValues.Add(_PART_SysIcon, HitTestValues.HTSYSMENU);
            }

            base.OnApplyTemplate();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}