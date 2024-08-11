using fileDownloader.Services.MVVM;
using fileDownloader.View;
using fileDownloader.ViewModel;
using fileDownloader.ViewModel.Base;
using SimpleInjector;
using System.Configuration;
using System.Data;
using System.Windows;
using Container = SimpleInjector.Container;


namespace fileDownloader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container p_container { get; } = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            p_container.Options.EnableAutoVerification = false;
            p_container.Register<ViewModelFactory>(Lifestyle.Singleton);
            //viewModels
            p_container.Register<ViewModel.MainViewModel>(Lifestyle.Singleton); 
            p_container.RegisterSingleton<LoaderViewModel>();   
            //Views 
            p_container.Register<MainWindow>(Lifestyle.Singleton);
            p_container.RegisterSingleton<LoaderView>();
            var view = p_container.GetInstance<MainWindow>();
            view.Show();
            base.OnStartup(e);
        }
    }
}