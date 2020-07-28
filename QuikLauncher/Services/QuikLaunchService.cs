using System;
using System.Threading;

namespace QuikLauncher
{
    public class QuikLaunchService : IQuikLaunchService
    {
        private readonly IConfigurationService _configuration;
        private readonly IQuikApplicationManager _quik;
        private readonly ILogger _logger;

        public QuikLaunchService(
            IConfigurationService configuration,
            IQuikApplicationManager quik,
            ILogger log)
        {
            _configuration = configuration;
            _quik = quik;
            _logger = log;
        }

        public void Run()
        {
            _logger.Log(ConsoleMessages.Start);

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
                        _logger.Log(ConsoleMessages.QuikInstanceNotFound);

                        // quik instance not found, need to run new instance
                        _quik.RunNewInstance();
                    }
                    else
                    {
                        _logger.Log(ConsoleMessages.QuikInstanceFoundAndNotAuthorized);

                        // quik instance found, but need to force new connection
                        _quik.ProceedConnect(quikWindow);
                    }
                }
                else
                {
                    _logger.Log(ConsoleMessages.LoginPopupFound);
                }

                while (loginPopup == IntPtr.Zero)
                {
                    _logger.Log(ConsoleMessages.WaitForLoginPopup);

                    // login popup should appear after new instance or connection, wait for login popup
                    loginPopup = _quik.GetLoginPopup();
                    Thread.Sleep(_configuration.Delay);
                }

                _logger.Log(ConsoleMessages.LoginPopupFoundProceedAuthorize);

                // login popup found, proceed login
                _quik.ProceedLogin(loginPopup);
            }
            else
            {
                _logger.Log(ConsoleMessages.QuikAlreadyAuthorized);
            }

            _logger.Log(ConsoleMessages.Done);
        }
    }
}
