using System;
using System.Threading;

namespace QuikLauncher
{
    public class QuikLaunchService : IQuikLaunchService
    {
        private readonly IConfigurationService _configuration;
        private readonly IQuikApplicationManager _quik;

        public QuikLaunchService(
            IConfigurationService configuration,
            IQuikApplicationManager quik)
        {
            _configuration = configuration;
            _quik = quik;
        }

        public void Run()
        {
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
                        // quik instance not found, need to run new instance
                        _quik.RunNewInstance();
                    }
                    else
                    {
                        // quik instance found, but need to force new connection
                        _quik.ProceedConnect(quikWindow);
                    }
                }

                while (loginPopup == IntPtr.Zero)
                {
                    // login popup should appear after new instance or connection, wait for login popup
                    loginPopup = _quik.GetLoginPopup();
                    Thread.Sleep(_configuration.Delay);
                }

                // login popup found, proceed login
                _quik.ProceedLogin(loginPopup);
            }
        }
    }
}
