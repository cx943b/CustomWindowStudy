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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int WM_NCCALCSIZE = 0x0083;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var wndHandle = new WindowInteropHelper(this).EnsureHandle();
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

                        // like TitleBar
                        if (btnMax != null)
                        {
                            Point posButton = btnMax.TranslatePoint(new Point(0, 0), this);
                            Rect rectButton = new Rect(posButton, new Size(btnMax.ActualWidth, btnMax.ActualHeight));

                            if(rectButton.Contains(mousePos))
                            {
                                btnMax.Background = Brushes.LightBlue;
                                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] MouseOver: btnTest(MaxButton)");

                                return (IntPtr)HitTestValues.HTMAXBUTTON;
                            }
                            else
                            {
                                btnMax.Background = Brushes.Red;
                            }
                        }
                        if (btnClose != null)
                        {
                            Point posButton = btnClose.TranslatePoint(new Point(0, 0), this);
                            Rect rectButton = new Rect(posButton, new Size(btnClose.ActualWidth, btnClose.ActualHeight));

                            if (rectButton.Contains(mousePos))
                            {
                                btnClose.Background = Brushes.LightBlue;
                                Debug.WriteLine($"[{DateTime.Now.ToLongTimeString()}] MouseOver: btnTest(MaxButton)");

                                return (IntPtr)HitTestValues.HTCLOSE;
                            }
                            else
                            {
                                btnClose.Background = Brushes.Red;
                            }
                        }
                        // like TitleBar

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