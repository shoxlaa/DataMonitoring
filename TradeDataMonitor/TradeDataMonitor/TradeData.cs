using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace TradeDataMonitor
{
    public class TradeData
    {
        public int Index { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public int Volume { get; set; }
    }


    public interface ILoader
    {
        IEnumerable<TradeData> Load(string filePath);
    }



    public class CsvLoader : ILoader
    {
        public IEnumerable<TradeData> Load(string filePath)
        {
            var tradeDataList = new List<TradeData>();
            var lines = File.ReadAllLines(filePath);
            Logger.Log($"Loading file: {filePath}");
            foreach (var line in lines)
            {
                Logger.Log($"Processing line: {line}");
                var values = line.Split(',');
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
            return tradeDataList;
        }
    }

    public class XmlLoader : ILoader
    {
        public IEnumerable<TradeData> Load(string filePath)
        {
            var tradeDataList = new List<TradeData>();
            var doc = XDocument.Load(filePath);

            var values = doc.Descendants("value");

            foreach (var value in values)
            {
                var tradeData = new TradeData
                {
                    Date = DateTime.Parse(value.Attribute("date").Value),
                    Open = decimal.Parse(value.Attribute("open").Value),
                    High = decimal.Parse(value.Attribute("high").Value),
                    Low = decimal.Parse(value.Attribute("low").Value),
                    Close = decimal.Parse(value.Attribute("close").Value),
                    Volume = int.Parse(value.Attribute("volume").Value)
                };
                tradeDataList.Add(tradeData);
            }

            return tradeDataList;
        }
    }

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
    public class DirectoryMonitor
    {
        private readonly string _directoryPath;
        private readonly int _monitoringFrequency;
        private readonly List<ILoader> _loaders;

        public DirectoryMonitor(string directoryPath, int monitoringFrequency, List<ILoader> loaders)
        {
            _directoryPath = directoryPath;
            _monitoringFrequency = monitoringFrequency;
            _loaders = loaders;
        }

        public ILoader GetLoaderByExtension(string extension)
        {
            return _loaders.FirstOrDefault(l => (extension == ".csv" && l is CsvLoader) ||
                                                (extension == ".xml" && l is XmlLoader) ||
                                                (extension == ".txt" && l is TxtLoader));
        }

        public async Task StartMonitoring(Func<string, ILoader, Task> processFileAsync)
        {
            var processedFiles = new HashSet<string>();
            while (true)
            {
                var files = Directory.GetFiles(_directoryPath);
                var newFiles = files.Where(file => !processedFiles.Contains(file)).ToList();

                var tasks = new List<Task>();

                foreach (var file in newFiles)
                {
                    var extension = System.IO.Path.GetExtension(file).ToLower();
                    var loader = GetLoaderByExtension(extension);

                    if (loader != null)
                    {
                        tasks.Add(processFileAsync(file, loader));
                        processedFiles.Add(file);
                    }
                }

                // Ожидаем завершения всех задач
                await Task.WhenAll(tasks);

                // Задержка между проверками
                await Task.Delay(_monitoringFrequency);
            }
        }
    }
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
