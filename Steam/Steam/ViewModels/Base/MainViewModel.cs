
using System;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Steam.Services.Mvvm;

namespace Steam.ViewModels
{
    public partial class MainViewModel : BaseViewModel
    {
        private readonly ViewModelFactory _factory;

        [ObservableProperty]
        private BaseViewModel? _currentViewModel;

        public MainViewModel(ViewModelFactory factory)
        {
            _factory = factory;

            CurrentViewModel = (BaseViewModel?)factory.Create(ViewModelType.StoreViewModel);

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<ViewModelType>>(this, (sender, message) =>
            {
                CurrentViewModel = (BaseViewModel?)_factory.Create(message.Value);
            });
        }
    }


}
