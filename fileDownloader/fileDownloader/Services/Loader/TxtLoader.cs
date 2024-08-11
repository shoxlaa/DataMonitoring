using fileDownloader.Model;
using System.Diagnostics;
using System.IO;


namespace fileDownloader.Services.Loader
{
    public class TxtLoader : ILoader
    {
        public IEnumerable<TradeData> Load(string filePath)
        {
            var tradeDataList = new List<TradeData>();
            var lines = File.ReadAllLines(filePath);
            Logger.Log($"Loading file: {filePath}");
            foreach (var line in lines)
            {
                Debug.WriteLine($"Processing line: {line}");
                var values = line.Split(';');
                try
                {
                    var tradeData = new TradeData
                    {
                        Date = DateTime.Parse(values[0]),
                        Open = decimal.Parse(values[1]),
                        High = decimal.Parse(values[2]),
                        Low = decimal.Parse(values[3]),
                        Close = decimal.Parse(values[4]),
                        Volume = int.Parse(values[5])
                    };
                    tradeDataList.Add(tradeData);
                }
                catch (Exception e)
                {

                }


            }
            return tradeDataList;
        }
    }

    // I added lock beacuse using threads create the problems 
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
