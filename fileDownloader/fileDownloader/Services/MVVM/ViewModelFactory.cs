using fileDownloader.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileDownloader.Services.MVVM
{
    public  class ViewModelFactory
    {
        //when u have db u will add it here 

        public ViewModelFactory() { }

        public IBaseViewModel Create( ViewModelType type)
        {
            return type switch
            {
                //ViewModelType.StoreViewModel => App.p_container.GetInstance<LoginViewModel>(),
                ///ViewModelType.GamesViewModel => App.p_container.GetInstance<GamesViewModel>(),
                //ViewModelType.AddViewModel => App.p_container.GetInstance<AddViewModel>(),
                //ViewModelType.GameDescriptionViewModel => App.p_container.GetInstance<GameDescriptionViewModel>(),


                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }

    public enum ViewModelType
    {
        LoaderViewModel; 
    }
}
