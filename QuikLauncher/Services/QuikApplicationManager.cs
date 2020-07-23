using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace QuikLauncher
{
    public class QuikApplicationManager : IQuikApplicationManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private const int BM_CLICK = 0x00F5;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_SETTEXT = 0x000C;

        private readonly IConfigurationService _configuration;

        public QuikApplicationManager(IConfigurationService configuration)
        {
            _configuration = configuration;
        }

        public IntPtr GetAuthorizedWindow()
        {
            return FindWindow(null, _configuration.QuikAuthTitle);
        }

        public IntPtr GetNotAuthorizedWindow()
        {
            var quikScreen = FindWindow(null, _configuration.QuikNotAuthTitle);

            if (quikScreen == IntPtr.Zero)
            {
                quikScreen = FindWindow(null, _configuration.QuikNotAuthTitle2);
            }

            return quikScreen;
        }

        public IntPtr GetLoginPopup()
        {
            return FindWindow(null, _configuration.QuikLoginTitle);
        }

        public void ProceedConnect(IntPtr quikWindow)
        {
            var toolbar = GetToobarMenu(quikWindow);

            SendMessage(toolbar, WM_LBUTTONDOWN, 0, MAKELPARAM(5, 5));
            Thread.Sleep(100);
            PostMessage(toolbar, WM_LBUTTONUP, 0, MAKELPARAM(5, 5));
            Thread.Sleep(100);
        }

        public void ProceedLogin(IntPtr loginPopup)
        {
            var hwnd = new IntPtr(0);
            hwnd = FindWindowEx(loginPopup, hwnd, null, null);
            var hLogin = FindWindowEx(loginPopup, hwnd, null, null);
            var hPass = FindWindowEx(loginPopup, hLogin, null, null);
            var hBtnOk = FindWindowEx(loginPopup, hPass, null, null);

            SendMessage(hPass, WM_SETTEXT, 0, _configuration.QuikUserPass);
            SendMessage(hBtnOk, BM_CLICK, 0, null);
        }

        public void RunNewInstance()
        {
            Process.Start(_configuration.QuikPath);
        }

        private IntPtr GetToobarMenu(IntPtr quik)
        {
            var hwnd = IntPtr.Zero;
            var firstChild = FindWindowEx(quik, hwnd, null, null);
            var hwndReBarWindow32 = GetTillClassName(quik, firstChild, "ReBarWindow32");

            if (hwndReBarWindow32 != IntPtr.Zero)
            {
                // TODO: this number of steps for particular quik install only! revisit this and refactor to find needed menu via loop
                hwnd = IntPtr.Zero;
                firstChild = FindWindowEx(hwndReBarWindow32, hwnd, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                firstChild = FindWindowEx(hwndReBarWindow32, firstChild, null, null);
                return firstChild;
            }

            return IntPtr.Zero;
        }

        private IntPtr GetTillClassName(IntPtr parent, IntPtr hWnd, string className)
        {
            if (hWnd == IntPtr.Zero)
            {
                return hWnd;
            }

            var name = GetWinClass(hWnd);
            if (name.Equals(className))
            {
                return hWnd;
            }
            else
            {
                var nextHwnd = FindWindowEx(parent, hWnd, null, null);
                return GetTillClassName(parent, nextHwnd, className);
            }
        }

        private string GetWinClass(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return null;
            StringBuilder classname = new StringBuilder(100);
            IntPtr result = GetClassName(hwnd, classname, classname.Capacity);
            if (result != IntPtr.Zero)
                return classname.ToString();
            return null;
        }

        public static int MAKELPARAM(int p, int p_2)
        {
            return ((p_2 << 16) | (p & 0xFFFF));
        }
    }
}
