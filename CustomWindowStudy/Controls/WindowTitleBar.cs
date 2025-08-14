using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CustomWindowStudy.Controls
{
    [TemplatePart(Name = "PART_SysIcon", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_TitleArea", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "PART_MinimizeButton", Type = typeof(IHitTestElement))]
    [TemplatePart(Name = "PART_MaximizeButton", Type = typeof(IHitTestElement))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(IHitTestElement))]
    public class WindowTitleBar : ContentControl
    {
        FrameworkElement? _PART_SysIcon;
        FrameworkElement? _PART_TitleArea;
        WindowTitleBarButton? _PART_MinimizeButton;
        WindowTitleBarButton? _PART_MaximizeButton;
        WindowTitleBarButton? _PART_CloseButton;

        Dictionary<HitTestValues, Rect> _dicHitTestValues = new Dictionary<HitTestValues, Rect>();

        static WindowTitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitleBar), new FrameworkPropertyMetadata(typeof(WindowTitleBar)));
        }

        public HitTestValues CheckHitTest(Point srcPos)
        {
            Point mousePos = PointFromScreen(srcPos);
            FrameworkElement? ele = null;

            foreach(var kvp in _dicHitTestValues)
            {
                ele = kvp.Key;
                Rect rect = new Rect(ele.TranslatePoint(new Point(0, 0), this), ele.RenderSize);

                if (rect.Contains(mousePos))
                    return kvp.Value;
            }

            return HitTestValues.HTNOWHERE;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _PART_SysIcon = GetTemplateChild("PART_SysIcon") as FrameworkElement;
            if (_PART_SysIcon != null)
            {
                _dicHitTestValues.Add(_PART_SysIcon, HitTestValues.HTSYSMENU);
            }

            _PART_TitleArea = GetTemplateChild("PART_TitleArea") as FrameworkElement;
            if(_PART_TitleArea != null)
            {
                _dicHitTestValues.Add(_PART_TitleArea, HitTestValues.HTCAPTION);
            }

            _PART_MinimizeButton = GetTemplateChild("PART_MinimizeButton") as WindowTitleBarButton;
            if (_PART_MinimizeButton != null)
            {
                _dicHitTestValues.Add(_PART_MinimizeButton, HitTestValues.HTMINBUTTON);
            }

            _PART_MaximizeButton = GetTemplateChild("PART_MaximizeButton") as WindowTitleBarButton;
            if (_PART_MaximizeButton != null)
            {
                _dicHitTestValues.Add(_PART_MaximizeButton, HitTestValues.HTMAXBUTTON);
            }

            _PART_CloseButton = GetTemplateChild("PART_CloseButton") as WindowTitleBarButton;
            if (_PART_CloseButton != null)
            {
                _dicHitTestValues.Add(_PART_CloseButton, HitTestValues.HTCLOSE);
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
        }
    }
}