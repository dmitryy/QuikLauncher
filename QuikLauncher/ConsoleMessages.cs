namespace QuikLauncher
{
    public class ConsoleMessages
    {
        public static string Start { get; } = "Check quik status...";
        public static string Done { get; } = "Done!";
        public static string LoginPopupFoundProceedAuthorize { get; } = "Login window found! Proceed authorize...";
        public static string LoginPopupFound { get; } = "Login popup found, proceed with password...";
        public static string QuikInstanceNotFound { get; } = "Quik instance not found! Starting new instance...";
        public static string QuikInstanceFoundAndNotAuthorized { get; } = "Quik instance found, but not authorized! Proceeding with connect...";
        public static string QuikAlreadyAuthorized { get; } = "Quik launched and connected.";
        public static string WaitForLoginPopup { get; } = "Wait for login window...";
    }
}
