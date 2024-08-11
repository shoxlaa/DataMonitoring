
using Steam.Models;
using Steam.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Steam.Services.Mvvm;
using Steam.Services.DataBase.Interfaces;

namespace Steam.ViewModels
{
    public partial class GamesViewModel : BaseViewModel
    {

        [ObservableProperty]
        private List<Game> _userGames;
        [ObservableProperty]
        private List<Game> _games;
        private readonly IGamesDataBase _gamesData;
        private readonly IUserDataBase _userData;
        [ObservableProperty]
        private Game _selectedGame;
        [ObservableProperty]
        private User _sendedUser;
        public GamesViewModel(IGamesDataBase gamesData, IUserDataBase usersDataBase)
        {
            _gamesData = gamesData;
            _userData = usersDataBase;
            _userGames = new();
            _selectedGame = new();
            _sendedUser = new User();
            _games = gamesData.GetAll();  

            WeakReferenceMessenger.Default.Register<ValueChangedMessage<User?>, string>(this,"MyUser", (sender, message) =>
            {
                _sendedUser = message.Value;
                var l = SendedUser.GamesUser.Select(x => x.GamesGameId);

                foreach (var gameId in l)
                {
                    _userGames.Add(_gamesData.FirstOrDefault<Game>(x => x.GameId == gameId));
                }

            });
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<Game>, string>(this, "AddGame", (sender, message) =>
            {
                if (message.Value != null)
                {
                    _games.Add(message.Value);
                }
            });
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<Game>, string>(this, "BuyGame", (sender, message) =>
            {
                _userGames= new();  
                
                var l = SendedUser.GamesUser.Select(x => x.GamesGameId);

                foreach (var gameId in l)
                {
                    _userGames.Add(_gamesData.FirstOrDefault<Game>(x => x.GameId == gameId));
                }
            });
        }

        [RelayCommand]
        private void Add()
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.AddViewModel));
        }

        [RelayCommand]
        private void SendGame()
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.GameDescriptionViewModel));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Game>(SelectedGame));
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<User>(SendedUser));

        }

        [RelayCommand]
        private void Back()
        {
            _userGames = new();
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.StoreViewModel));
        }
       

        //private readonly IStoreDataBase _storeData;
        //string jsonString;
        //public GamesViewModel(IStoreDataBase storeDataBase)
        //{
        //    WeakReferenceMessenger.Default.Register<ValueChangedMessage<Product>>(this, (sender, message) =>
        //    {
        //        //Products.Add(message.Value);
        //        //storeDataBase.AddProduct(message.Value);
        //    });
        //}

    }
}
