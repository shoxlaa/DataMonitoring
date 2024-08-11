using Steam.ViewModels;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Steam.Views;
using Steam.Services.Mvvm;
using Steam.Services.DataBase;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.Extensions.Options;
using Container = SimpleInjector.Container;
using Steam.Services.DataBase.Interfaces;

namespace Steam
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container p_container { get; } = new();
        public static Container v_container { get; } = new();


        protected override void OnStartup(StartupEventArgs e)
        {

             p_container.Register<IGameUserDataBase, GameUserDataBase>(Lifestyle.Singleton);

            p_container.Options.EnableAutoVerification = false;
            //viewModels
            p_container.RegisterSingleton<MainViewModel>();
            p_container.Register<GamesViewModel>(Lifestyle.Singleton);
            p_container.RegisterSingleton<LoginViewModel>();
            p_container.RegisterSingleton<AddViewModel>();
            p_container.RegisterSingleton<GameDescriptionViewModel>();
            //services 
            p_container.Register<IUserDataBase, UserDataBase>(Lifestyle.Singleton);
            p_container.Register<IGamesDataBase, GamesDataBase>(Lifestyle.Singleton);
            p_container.Register<ViewModelFactory>(Lifestyle.Singleton);
            p_container.Register<MainWindow>(Lifestyle.Singleton);
            var view = p_container.GetInstance<MainWindow>();
            view.Show();
            base.OnStartup(e);
        }
    }
}
