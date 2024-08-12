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

namespace fileDownloader.ViewModel
{
    public partial class LoaderViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string? _pwd;

        public LoaderViewModel() 
        {  
        }

        [RelayCommand]

        public void ChangeLocation()
        {
            // Added  the library that open diaolge  (WindowsAPICodePack) 
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C:\\Users",
                IsFolderPicker = true
            };

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var newDirectory = dialog.FileName;
                Pwd = dialog.FileName; 
               

               // UpdateConfiguration("InputDirectory", newDirectory);

                //LoadConfiguration();
                //StartMonitoringAsync(); // Перезапуск мониторинга
            }

        }  

        public void LoadConfiguration()
        {
            var directoryPath = ConfigurationManager.AppSettings["InputDirectory"];
            Pwd = directoryPath; 

            var monitoringFrequency = int.Parse(ConfigurationManager.AppSettings["MonitoringFrequency"]); 

            var loaders = new List<ILoader> { new CsvLoader(), new XmlLoader(), new TxtLoader() };
           // _directoryMonitor = new DirectoryMonitor(directoryPath, monitoringFrequency, loaders);

        }




    }
}
