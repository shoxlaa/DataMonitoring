using fileDownloader.Services.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileDownloader.Services
{
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



}
