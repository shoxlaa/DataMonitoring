using fileDownloader.Model;
using System.IO;

namespace fileDownloader.Services.Loader
{
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

}
