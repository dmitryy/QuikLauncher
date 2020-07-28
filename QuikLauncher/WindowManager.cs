using System;
using System.Runtime.InteropServices;
using System.Text;

namespace QuikLauncher
{
    public class WindowManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        protected static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        protected static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        protected static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        protected static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        protected static extern IntPtr GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        [DllImport("user32.dll")]
        protected static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);

        protected const int BM_CLICK = 0x00F5;
        protected const int WM_LBUTTONDOWN = 0x201;
        protected const int WM_LBUTTONUP = 0x202;
        protected const int WM_SETTEXT = 0x000C;
        protected const int KEY_CONTROL = 0x11;
        protected const int KEY_Q = 0x51;
        protected const int KEY_DOWN = 0x0100;
        protected const int KEY_UP = 0x0101;

        protected IntPtr GetTillClassName(IntPtr parent, IntPtr hWnd, string className)
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

        protected string GetWinClass(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                return null;
            StringBuilder classname = new StringBuilder(100);
            IntPtr result = GetClassName(hwnd, classname, classname.Capacity);
            if (result != IntPtr.Zero)
                return classname.ToString();
            return null;
        }

        protected int MergeParam(int p, int p_2)
        {
            return ((p_2 << 16) | (p & 0xFFFF));
        }

        //private string GetWindowTitle(IntPtr hWnd)
        //{
        //    var length = GetWindowTextLength(hWnd);
        //    var title = new StringBuilder(length);
        //    GetWindowText(hWnd, title, length);
        //    return title.ToString();
        //}
    }
}
