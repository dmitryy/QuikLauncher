using System;
using System.Configuration;

namespace QuikLauncher
{
    public class ConfigurationService : IConfigurationService
    {
        public string QuikAuth { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikAuthTitleVar"], EnvironmentVariableTarget.Machine);
        public string QuikPass { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikPasswordVar"], EnvironmentVariableTarget.Machine);
        public string QuikPath { get; } = Environment.GetEnvironmentVariable(ConfigurationManager.AppSettings["QuikPathVar"], EnvironmentVariableTarget.Machine);
        public string QuikAuthTitle { get; } = ConfigurationManager.AppSettings["QuikAuthorizedTitle"];
        public string QuikNotAuthTitle { get; } = ConfigurationManager.AppSettings["QuikNotAuthorizedTitle"];
        public string QuikNotAuthTitle2 { get; } = ConfigurationManager.AppSettings["QuikNotAuthorizedTitle2"];
        public string QuikLoginTitle { get; } = ConfigurationManager.AppSettings["QuikLoginDialogTitle"];
        public string QuikConnectButtonTitle { get; } = ConfigurationManager.AppSettings["QuikConnectButtonTitle"];
        public int Delay { get; } = Convert.ToInt32(ConfigurationManager.AppSettings["DelayForLogin"]);
    }
}
