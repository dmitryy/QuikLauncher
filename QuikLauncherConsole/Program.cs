using Microsoft.Extensions.DependencyInjection;
using QuikLauncher;

namespace QuikLauncherConsole
{
    class Program
    {
        private static ServiceProvider BuildServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<IQuikApplicationManager, QuikApplicationManager>()
                .AddSingleton<IQuikLaunchService, QuikLaunchService>()
                .AddSingleton<ILog, ConsoleLog>()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
        {
            var provider = BuildServiceProvider();

            var quikLauncher = provider.GetService<IQuikLaunchService>();

            quikLauncher.Run();

            System.Console.ReadKey();
        }
    }
}
