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
    public enum DWMWINDOWATTRIBUTE : uint
    {
        DWMWA_NCRENDERING_ENABLED,
        DWMWA_NCRENDERING_POLICY,
        DWMWA_TRANSITIONS_FORCEDISABLED,
        DWMWA_ALLOW_NCPAINT,
        DWMWA_CAPTION_BUTTON_BOUNDS,
        DWMWA_NONCLIENT_RTL_LAYOUT,
        DWMWA_FORCE_ICONIC_REPRESENTATION,
        DWMWA_FLIP3D_POLICY,
        DWMWA_EXTENDED_FRAME_BOUNDS,
        DWMWA_HAS_ICONIC_BITMAP,
        DWMWA_DISALLOW_PEEK,
        DWMWA_EXCLUDED_FROM_PEEK,
        DWMWA_CLOAK,
        DWMWA_CLOAKED,
        DWMWA_FREEZE_REPRESENTATION,
        DWMWA_PASSIVE_UPDATE_MODE,
        DWMWA_USE_HOSTBACKDROPBRUSH,
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_WINDOW_CORNER_PREFERENCE = 33,
        DWMWA_BORDER_COLOR,
        DWMWA_CAPTION_COLOR,
        DWMWA_TEXT_COLOR,
        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
        DWMWA_SYSTEMBACKDROP_TYPE,
        DWMWA_MICA_EFFECT = 1029,
        DWMWA_LAST

    }


    // 모서리 선호도 열거형
    public enum DWM_WINDOW_CORNER_PREFERENCE
    {
        DWMWCP_DEFAULT = 0,
        DWMWCP_DONOTROUND = 1,  // 둥근 모서리 비활성화!
        DWMWCP_ROUND = 2,
        DWMWCP_ROUNDSMALL = 3
    }

    
    public partial class MainWindow : Window
    {
        [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute, uint cbAttribute);
        private const int WM_NCCALCSIZE = 0x0083;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var wndHandle = new WindowInteropHelper(this).EnsureHandle();
            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
            var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_DONOTROUND;
            DwmSetWindowAttribute(wndHandle, attribute, ref preference, sizeof(uint));
            DwmSetWindowAttribute(wndHandle, DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR, ref preference, sizeof(uint));

            var hwndSrc = HwndSource.FromHwnd(wndHandle);
            if (hwndSrc != null)
            {
                hwndSrc.AddHook(new HwndSourceHook(wndSrcHook));
            }

        }

        private IntPtr wndSrcHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle custom window messages here if needed
            // For example, you can handle WM_NCHITTEST to customize window dragging behavior
            switch (msg)
            {
                case 0x0084: // WM_NCHITTEST
                    {
                        handled = true;

                        var scrPos = new Point(lParam.ToInt32() & 0xFFFF, lParam.ToInt32() >> 16);
                        var mousePos = PointFromScreen(scrPos);

                        Size clientSize = new Size(300, 100);
                        Rect clientRect = new Rect(new Point((Width - clientSize.Width) / 2, (Height - clientSize.Height) / 2), clientSize);

                        if (clientRect.Contains(mousePos))
                        {
                            Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTCLIENT");
                            return (IntPtr)HitTestValues.HTCLIENT; // 버튼 영역
                        }

                        Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCHITTEST: HTNOWHERE");
                        return (IntPtr)HitTestValues.HTNOWHERE;
                    }
                    // WM_NCLBUTTONDOWN
                    case 0x00A1: // WM_NCLBUTTONDOWN
                    {
                        Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] WM_NCLBUTTONDOWN");
                        handled = true;
                        return IntPtr.Zero; // 기본 동작을 수행하지 않음
                    }
            }
            return IntPtr.Zero;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Button_Click");
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rgrc0;
        public RECT rgrc1;
        public RECT rgrc2;
        public IntPtr lppos;
    }

    public enum HitTestValues
    {
        HTERROR = -2,       // 오류 발생 (예: 다른 윈도우로 메시지 전송 시)
        HTTRANSPARENT = -1, // 투명한 영역 (마우스 이벤트가 아래 윈도우로 전달)
        HTNOWHERE = 0,      // 윈도우 영역이지만 아무것도 아님
        HTCLIENT = 1,       // 클라이언트 영역
        HTCAPTION = 2,      // 타이틀 바
        HTSYSMENU = 3,      // 시스템 메뉴
        HTGROWBOX = 4,      // 크기 조정 상자 (HTSIZE와 동일)
        HTMENU = 5,         // 메뉴
        HTHSCROLL = 6,      // 가로 스크롤 바
        HTVSCROLL = 7,      // 세로 스크롤 바
        HTMINBUTTON = 8,    // 최소화 버튼
        HTMAXBUTTON = 9,    // 최대화 버튼
        HTLEFT = 10,        // 왼쪽 테두리
        HTRIGHT = 11,       // 오른쪽 테두리
        HTTOP = 12,         // 위쪽 테두리
        HTTOPLEFT = 13,     // 왼쪽 위 모서리
        HTTOPRIGHT = 14,    // 오른쪽 위 모서리
        HTBOTTOM = 15,      // 아래쪽 테두리
        HTBOTTOMLEFT = 16,  // 왼쪽 아래 모서리
        HTBOTTOMRIGHT = 17, // 오른쪽 아래 모서리
        HTBORDER = 18,      // 테두리 (크기 조정 불가)
        HTOBJECT = 19,      // 개체 (예: OLE 개체)
        HTCLOSE = 20,       // 닫기 버튼
        HTHELP = 21         // 도움말 버튼
    }
}