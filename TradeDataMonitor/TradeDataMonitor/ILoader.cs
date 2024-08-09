namespace TradeDataMonitor
{
    public interface ILoader
    {
        IEnumerable<TradeData> Load(string filePath);
    }




}
