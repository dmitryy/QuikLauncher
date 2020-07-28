using System;
using System.Diagnostics;
using System.Threading;

namespace QuikLauncher
{
    public class QuikApplicationManager : WindowManager, IQuikApplicationManager
    {
        private readonly IConfigurationService _configuration;

        public QuikApplicationManager(IConfigurationService configuration)
        {
            _configuration = configuration;
        }

        public IntPtr GetAuthorizedWindow()
        {
            return FindWindow(null, GetAuthorizedTitle());
        }

        public IntPtr GetNotAuthorizedWindow()
        {
            var quikScreen = FindWindow(null, GetNotAuthorizedTitle1());

            if (quikScreen == IntPtr.Zero)
            {
                quikScreen = FindWindow(null, GetNotAuthorizedTitle2());
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
            if (toolbar != IntPtr.Zero)
            {
                SendMessage(toolbar, WM_LBUTTONDOWN, 0, MergeParam(5, 5));
                Thread.Sleep(100);
                PostMessage(toolbar, WM_LBUTTONUP, 0, MergeParam(5, 5));
                Thread.Sleep(100);
            }
        }

        public void ProceedLogin(IntPtr loginPopup)
        {
            var hwnd = new IntPtr(0);
            hwnd = FindWindowEx(loginPopup, hwnd, null, null);
            var hLogin = FindWindowEx(loginPopup, hwnd, null, null);
            var hPass = FindWindowEx(loginPopup, hLogin, null, null);
            var hBtnOk = FindWindowEx(loginPopup, hPass, null, null);

            SendMessage(hPass, WM_SETTEXT, 0, _configuration.QuikPass);
            SendMessage(hBtnOk, BM_CLICK, 0, null);
        }

        public void RunNewInstance()
        {
            Process.Start(_configuration.QuikPath);
        }

        private IntPtr GetToobarMenu(IntPtr quikWindow)
        {
            var hwnd = IntPtr.Zero;
            var firstChild = FindWindowEx(quikWindow, hwnd, null, null);
            var hwndReBarWindow32 = GetTillClassName(quikWindow, firstChild, "ReBarWindow32");

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

        private string GetAuthorizedTitle()
        {
            return string.Format(_configuration.QuikAuthTitle, _configuration.QuikAuth, GetQuikVersion());
        }

        private string GetNotAuthorizedTitle1()
        {
            return string.Format(_configuration.QuikNotAuthTitle, GetQuikVersion());
        }

        private string GetNotAuthorizedTitle2()
        {
            return string.Format(_configuration.QuikNotAuthTitle2, GetQuikVersion());
        }

        private string GetQuikVersion()
        {
            return FileVersionInfo.GetVersionInfo(_configuration.QuikPath).FileVersion;
        }
    }
}
