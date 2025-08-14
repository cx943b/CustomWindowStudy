using CustomWindowStudy.Controls;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomWindowStudy
{
    public delegate IntPtr WindowMessageHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled);

    public partial class MainWindow : Window
    {
        readonly IDictionary<WindowMessage, WindowMessageHandler> _dicMessageHandler = new Dictionary<WindowMessage, WindowMessageHandler>();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var wndHandle = new WindowInteropHelper(this).EnsureHandle();
            uint preference = (uint)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND;
            uint dwmBorderColor = 0xFF0000;
            //uint bAllowNcPaint = 1;
            //DWMNCRENDERINGPOLICY ncRenderingPolicy = DWMNCRENDERINGPOLICY.DWMNCRP_DISABLED;

            NativeMethods.DwmSetWindowAttribute(wndHandle, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, preference, sizeof(DWM_WINDOW_CORNER_PREFERENCE));
            NativeMethods.DwmSetWindowAttribute(wndHandle, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, dwmBorderColor, sizeof(uint));

            // DWMWINDOWATTRIBUTE.DWMWA_ALLOW_NCPAINT를 True로 설정하면 최대 최소 종료시 창 화면 효과를 볼 수 없음.
            //NativeMethods.DwmSetWindowAttribute(wndHandle, DWMWINDOWATTRIBUTE.DWMWA_ALLOW_NCPAINT, bAllowNcPaint, sizeof(uint));

            //int style = NativeMethods.GetWindowLong(wndHandle, GWL_STYLE);
            //style &= ~(WS_CAPTION | WS_SYSMENU | WS_VISIBLE); // 타이틀바와 시스템 메뉴 제거
            //NativeMethods.SetWindowLong(wndHandle, GWL_STYLE, style);

            var hwndSrc = HwndSource.FromHwnd(wndHandle);
            if (hwndSrc != null)
            {
                addWindowMessages();
                hwndSrc.AddHook(new HwndSourceHook(wndSrcHook));
            }
        }

        private void addWindowMessages()
        {
            _dicMessageHandler.Add(WindowMessage.WM_NCHITTEST, processNcHitTest);
            _dicMessageHandler.Add(WindowMessage.WM_NCLBUTTONDOWN, processNcLButtonDown);
        }
        private void removeWindowMessages()
        {
            _dicMessageHandler.Clear();
        }

        private IntPtr processNcHitTest(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = true;

            var scrPos = new Point(lParam.ToInt32() & 0xFFFF, lParam.ToInt32() >> 16);
            var mousePos = PointFromScreen(scrPos);

            HitTestValues titleHitTest = titleBar.CheckHitTest(scrPos);
            if (titleHitTest != HitTestValues.HTNOWHERE)
            {
                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: {titleHitTest}");
                return (IntPtr)titleHitTest;
            }

            // ToDo: 타이틀바가 아닌 영역에 대한 HitTest 처리
            // Resize 영역을 먼저 처리하고, 그 다음에 클라이언트 영역을 처리합니다.

            Size clientSize = new Size(this.ActualWidth, this.ActualHeight - titleBar.ActualHeight);
            Rect clientRect = new Rect(new Point(0, titleBar.ActualHeight), clientSize);

            if (mousePos.X >= 0 && mousePos.X <= 8)
            {
                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTLEFT");
                return (IntPtr)HitTestValues.HTLEFT;
            }

            if (mousePos.Y < titleBar.ActualHeight)
            {
                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTCAPTION");
                return (IntPtr)HitTestValues.HTCAPTION;
            }
            else if (clientRect.Contains(mousePos))
            {
                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTCLIENT");
                return (IntPtr)HitTestValues.HTCLIENT;
            }

            Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTNOWHERE");
            return (IntPtr)HitTestValues.HTNOWHERE;
        }
        private IntPtr processNcLButtonDown(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            
            return IntPtr.Zero;
        }

        private IntPtr wndSrcHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;

            if (_dicMessageHandler.TryGetValue((WindowMessage)msg, out var handler))
            {
                return handler(hwnd, msg, wParam, lParam, ref handled);
            }

            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            var wndHandle = new WindowInteropHelper(this).Handle;
            var hwndSrc = HwndSource.FromHwnd(wndHandle);
            hwndSrc?.RemoveHook(wndSrcHook);

            removeWindowMessages();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button_Click");
        }
    }
}