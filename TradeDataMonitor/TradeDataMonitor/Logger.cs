using System.IO;

namespace TradeDataMonitor
{
    public static class Logger
    {
        private static readonly string logFilePath = "log.txt";
        private static readonly object lockObject = new object();

        public static void Log(string message)
        {
            lock (lockObject)
            {
                using (var writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
        }
    }




}
