using System;
using System.Threading;

namespace QuikLauncher
{
    public class QuikLaunchService : IQuikLaunchService
    {
        private readonly IConfigurationService _configuration;
        private readonly IQuikApplicationManager _quik;
        private readonly ILog _log;

        public QuikLaunchService(
            IConfigurationService configuration,
            IQuikApplicationManager quik,
            ILog log)
        {
            _configuration = configuration;
            _quik = quik;
            _log = log;
        }

        public void Run()
        {
            _log.Log(ConsoleMessages.Start);

            var quikWindow = _quik.GetAuthorizedWindow();

            if (quikWindow == IntPtr.Zero)
            {
                // authorized quik not found, trying to find if login popup shown
                var loginPopup = _quik.GetLoginPopup();

                if (loginPopup == IntPtr.Zero)
                {
                    // login popup not found, trying to find not authorized quik
                    quikWindow = _quik.GetNotAuthorizedWindow();

                    if (quikWindow == IntPtr.Zero)
                    {
                        _log.Log(ConsoleMessages.QuikInstanceNotFound);

                        // quik instance not found, need to run new instance
                        _quik.RunNewInstance();
                    }
                    else
                    {
                        _log.Log(ConsoleMessages.QuikInstanceFoundAndNotAuthorized);

                        // quik instance found, but need to force new connection
                        _quik.ProceedConnect(quikWindow);
                    }
                }
                else
                {
                    _log.Log(ConsoleMessages.LoginPopupFound);
                }

                while (loginPopup == IntPtr.Zero)
                {
                    _log.Log(ConsoleMessages.WaitForLoginPopup);

                    // login popup should appear after new instance or connection, wait for login popup
                    loginPopup = _quik.GetLoginPopup();
                    Thread.Sleep(_configuration.Delay);
                }

                _log.Log(ConsoleMessages.LoginPopupFoundProceedAuthorize);

                // login popup found, proceed login
                _quik.ProceedLogin(loginPopup);
            }
            else
            {
                _log.Log(ConsoleMessages.QuikAlreadyAuthorized);
            }

            _log.Log(ConsoleMessages.Done);
        }
    }
}
