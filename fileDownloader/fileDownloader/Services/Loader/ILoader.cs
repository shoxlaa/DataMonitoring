using fileDownloader.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileDownloader.Services.Loader
{
    public interface ILoader
    {
        IEnumerable<TradeData> Load(string filePath);
    }

}
