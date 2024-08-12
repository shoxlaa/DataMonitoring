using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using MessageBox = System.Windows.MessageBox; // Это необходимо, чтобы избежать конфликта имен с System.Windows.Forms.MessageBox

namespace TradeDataMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public ObservableCollection<TradeData> TradeDataList { get; set; }
        //public ObservableCollection<string> FilesListBox { get; set; }
        private DirectoryMonitor _directoryMonitor;
        private Task _monitoringTask;
        private bool _isMonitoring;
        private FileSystemWatcher _fileWatcher;
        private bool _initialLoading;

        public MainWindow()
        {
            InitializeComponent();
            TradeDataList = new ObservableCollection<TradeData>();
            
            tradeDataGrid.ItemsSource = TradeDataList;

            LoadConfiguration();
            StartMonitoringAsync();
        }

        private void LoadConfiguration()
        {
            var directoryPath = ConfigurationManager.AppSettings["InputDirectory"];
            var monitoringFrequency = int.Parse(ConfigurationManager.AppSettings["MonitoringFrequency"]);
            var loaders = new List<ILoader> { new CsvLoader(), new XmlLoader(), new TxtLoader() };
            _directoryMonitor = new DirectoryMonitor(directoryPath, monitoringFrequency, loaders);

            
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

        private async Task ProcessFileAsync(string filePath, ILoader loader)
        {
            // Загружаем данные асинхронно
            var tradeDataList = await Task.Run(() => loader.Load(filePath));

            // Обновляем UI после загрузки данных
            Dispatcher.Invoke(() =>
            {
                foreach (var tradeData in tradeDataList)
                {
                    tradeData.Index = TradeDataList.Count + 1;
                    TradeDataList.Add(tradeData);
                }
            });
        }

        private void ChangeDirectoryButton_Click(object sender, RoutedEventArgs e)
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

                // Получаем список файлов в выбранной папке
                var files = Directory.GetFiles(selectedPath);

                // Очищаем предыдущие элементы и добавляем новые в ListBox
                FilesListBox.Items.Clear();

                foreach (var file in files)
                {
                    FilesListBox.Items.Add(Path.GetFileName(file));
                }
                var newDirectory = dialog.FileName;
                UpdateConfiguration("InputDirectory", newDirectory);


                LoadConfiguration();
                _initialLoading = true;
                ProcessExistingFiles(newDirectory); // Обрабатываем файлы без использования FileSystemWatcher
                _initialLoading = false;

                ResetFileWatcher(newDirectory);
                StartMonitoringAsync(); // Перезапуск мониторинга
            }
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