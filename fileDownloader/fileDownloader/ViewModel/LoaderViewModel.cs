using CommunityToolkit.Mvvm.Input;
using fileDownloader.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using fileDownloader.Services.Loader;
using System.Configuration;
using fileDownloader.Model;
using fileDownloader.Services;
using System.IO;
using System.Windows.Threading;

namespace fileDownloader.ViewModel
{
    public partial class LoaderViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _pwd;
        [ObservableProperty]
        private List<TradeData> _tradeDataList;
        [ObservableProperty]
        private List<string> _filesListBox;
        private DirectoryMonitor _directoryMonitor;
        private Task _monitoringTask;
        private bool _isMonitoring;
        private FileSystemWatcher _fileWatcher;
        private bool _initialLoading;


        public LoaderViewModel()
        {
            _tradeDataList = new List<TradeData>();  
            _filesListBox = new List<string>();

            //LoadConfiguration();
            //StartMonitoringAsync();
        }

        public void LoadConfiguration()
        {
            var directoryPath = ConfigurationManager.AppSettings["InputDirectory"];
            Pwd = directoryPath;

            var monitoringFrequency = int.Parse(ConfigurationManager.AppSettings["MonitoringFrequency"]);

            var loaders = new List<ILoader> { new CsvLoader(), new XmlLoader(), new TxtLoader() };
             _directoryMonitor = new DirectoryMonitor(directoryPath, monitoringFrequency, loaders);

        }

        [RelayCommand]

        public void ChangeLocation()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = configuration.AppSettings.Settings["InputDirectory"].Value,
                IsFolderPicker = true
            };


            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string selectedPath = dialog.FileName;


                var files = Directory.GetFiles(selectedPath);


                FilesListBox.Clear();

                foreach (var file in files)
                {
                    FilesListBox.Add(Path.GetFileName(file));
                }
                var newDirectory = dialog.FileName;
                UpdateConfiguration("InputDirectory", newDirectory);


                LoadConfiguration();
                _initialLoading = true;
                ProcessExistingFiles(newDirectory);
                _initialLoading = false;

                ResetFileWatcher(newDirectory);
                StartMonitoringAsync(); // Перезапуск мониторинга
            }
        }

        private async Task ProcessFileAsync(string filePath, ILoader loader)
        {
            // Загружаем данные асинхронно
            var tradeDataList = await Task.Run(() => loader.Load(filePath));

            // Обновляем UI после загрузки данных
            
                foreach (var tradeData in tradeDataList)
                {
                    tradeData.Index = TradeDataList.Count + 1;
                    TradeDataList.Add(tradeData);
                }
           
        }

        private void ProcessExistingFiles(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);


            foreach (var file in files)
            {
                var extension = Path.GetExtension(file).ToLower();
                var loader = _directoryMonitor.GetLoaderByExtension(extension);

                if (loader != null)
                {
                    ProcessFileAsync(file, loader);
                }
            }
        }
        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            if (_initialLoading)
            {
                // Пропускаем обработку файлов, созданных при начальной загрузке
                return;
            }

            var extension = Path.GetExtension(e.FullPath).ToLower();
            var loader = _directoryMonitor.GetLoaderByExtension(extension);

            if (loader != null)
            {
                ProcessFileAsync(e.FullPath, loader);
            }
        }

        private void ResetFileWatcher(string directoryPath)
        {
            // Остановите старый FileSystemWatcher, если он существует
            if (_fileWatcher != null)
            {
                _fileWatcher.EnableRaisingEvents = false;
                _fileWatcher.Dispose();
            }

            // Настройка нового FileSystemWatcher
            _fileWatcher = new FileSystemWatcher
            {
                Path = directoryPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.*"
            };

            _fileWatcher.Created += OnFileCreated;
            _fileWatcher.EnableRaisingEvents = true;
        }

        private async void StartMonitoringAsync()
        {
            if (_isMonitoring)
            {
                _isMonitoring = false;
                if (_monitoringTask != null)
                {
                    await _monitoringTask;
                }
            }

            _isMonitoring = true;
            _monitoringTask = Task.Run(() => _directoryMonitor.StartMonitoring(ProcessFileAsync));
        }
       
        private void UpdateConfiguration(string key, string value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


    }
}
