using System.Xml.Linq;

namespace TradeDataMonitor
{
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




}
