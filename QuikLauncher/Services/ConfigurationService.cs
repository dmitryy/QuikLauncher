using System;
using System.Configuration;

namespace QuikLauncher
{
    public class ConfigurationService : IConfigurationService
    {
        public string QuikPath { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikPathVar"], EnvironmentVariableTarget.Machine);
        public string QuikAuthTitle { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikAuthTitleVar"], EnvironmentVariableTarget.Machine);
        public string QuikNotAuthTitle { get; } = ConfigurationManager.AppSettings["QuikNotAuthorizedTitle"];
        public string QuikNotAuthTitle2 { get; } = ConfigurationManager.AppSettings["QuikNotAuthorizedTitle2"];
        public string QuikLoginTitle { get; } = ConfigurationManager.AppSettings["QuikLoginDialogTitle"];
        public string QuikConnectButtonTitle { get; } = ConfigurationManager.AppSettings["QuikConnectButtonTitle"];
        public string QuikUserPass { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikPasswordVar"], EnvironmentVariableTarget.Machine);
        public int Delay { get; } = Convert.ToInt32(ConfigurationManager.AppSettings["DelayForLogin"]);
    }
}
