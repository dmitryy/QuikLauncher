using System;

namespace QuikLauncher
{
    public interface IQuikApplicationManager
    {
        /// <summary>
        /// Quik launched and authorized
        /// </summary>
        IntPtr GetAuthorizedWindow();

        /// <summary>
        /// Login popup window if exists
        /// </summary>
        IntPtr GetLoginPopup();

        /// <summary>
        /// Quik launched but not authorized
        /// </summary>
        IntPtr GetNotAuthorizedWindow();

        /// <summary>
        /// Press connect button to show login popup
        /// </summary>
        void ProceedConnect(IntPtr quikWindow);

        /// <summary>
        /// Populate password to login popup window and press login button
        /// </summary>
        void ProceedLogin(IntPtr loginPopup);

        /// <summary>
        /// Start new instance of Quik
        /// </summary>
        void RunNewInstance();
    }
}