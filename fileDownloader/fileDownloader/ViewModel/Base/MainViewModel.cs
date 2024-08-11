using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging.Messages;
using CommunityToolkit.Mvvm.Messaging;
using fileDownloader.Services.MVVM;
using fileDownloader.ViewModel.Base;

namespace fileDownloader.ViewModel
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly ViewModelFactory _factory;

        [ObservableProperty]
        private BaseViewModel? _currentViewModel;

        public MainViewModel(ViewModelFactory factory)
        {

            _factory = factory;

            CurrentViewModel = (BaseViewModel?)factory.Create(ViewModelType.LoaderViewModel);

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<ViewModelType>>(this, (sender, message) =>
            {
                CurrentViewModel = (BaseViewModel?)_factory.Create(message.Value);
            });
        }
    }
}
