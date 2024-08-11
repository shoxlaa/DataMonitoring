using Steam.Services.DataBase.Interfaces;
using Steam.ViewModels;
using Steam.ViewModels.Base;
using System;

namespace Steam.Services.Mvvm
{
    public class ViewModelFactory
    {
        private readonly IUserDataBase _userData;


        public ViewModelFactory(IUserDataBase storeDataBase)
        {
            _userData = storeDataBase;
        }
        public IBaseViewModel Create(ViewModelType type)
        {
            return type switch
            {
                ViewModelType.StoreViewModel => App.p_container.GetInstance<LoginViewModel>(),
                ViewModelType.GamesViewModel => App.p_container.GetInstance<GamesViewModel>(),
                ViewModelType.AddViewModel => App.p_container.GetInstance<AddViewModel>(),
                ViewModelType.GameDescriptionViewModel => App.p_container.GetInstance<GameDescriptionViewModel>(),


                _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
        }
    }

}
