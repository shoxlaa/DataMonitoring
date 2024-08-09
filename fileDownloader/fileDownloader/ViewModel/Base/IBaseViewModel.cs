using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileDownloader.ViewModel.Base
{
    // I am crating the interface ( I in SOlID  ) interface segregation principe   to inheritance the  BaseViewModel 
    // and add the attrube  INotifyPropertyChange don't do i in all my viewmodels 
    //yes in this project i will have only 1 or 2 pages  in real projects we really need that 

    public interface IBaseViewModel
    {
    } 


    public partial class  MainViewModel : BaseViewModel
    {
        // private readonly ViewModelFactory _factory; 

        [ObservableProperty]
        private BaseViewModel? _currentViewModel;



    }
}
