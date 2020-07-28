using System;

namespace QuikLauncher
{
    public class SimpleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
