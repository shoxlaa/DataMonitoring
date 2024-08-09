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

            var view = p_container.GetInstance<MainWindow>();
            view.Show();
            base.OnStartup(e);
        }
    }
}