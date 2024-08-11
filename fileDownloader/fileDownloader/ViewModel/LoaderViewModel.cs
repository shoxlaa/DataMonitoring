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
            // Add the library that open diaolge 
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
    }
}
