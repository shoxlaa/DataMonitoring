using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using Steam.Services.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steam.ViewModels
{
    public partial class GameDescriptionViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Game _selectedGame;

        [ObservableProperty]
        private string _ratingString;
        private readonly IGamesDataBase _gamesData;
        private readonly IGameUserDataBase _gameUsers;
        private readonly IUserDataBase _usersDB;

        private User _user { get; set; }
        public GameDescriptionViewModel(IGamesDataBase gamesData, IGameUserDataBase gameUsers, IUserDataBase users)
        {
            _gamesData = gamesData;
            _usersDB = users;
            _gameUsers = gameUsers;
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<Game>>(this, (sender, message) =>
            {
                _selectedGame = message.Value;
            });
            WeakReferenceMessenger.Default.Register<ValueChangedMessage<User>>(this, (sender, message) =>
            {
                _user = message.Value;
            });
        }
        [RelayCommand]
        private void Back()
        {
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.GamesViewModel));
        }
        [RelayCommand]
        private void NewRating()
        {
            ///_gamesData.Update(SelectedGame.Id, SelectedGame);
        }
        [RelayCommand]
        public void Buy()
        {
            //  что бы купить товар мне нужна айдишка игры и айдишка юзера 
            // после добавляю в Таблицу айдишки 
            var UsersGame = new GameUser() { GamesGameId = SelectedGame.GameId, UserUserId = _user.UserId };
            try
            {
                _selectedGame.GameUser.Add(UsersGame);
                _user.GamesUser.Add(UsersGame);
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Game>(SelectedGame), "BuyGame");

                //_usersDB.Update((int)_user.UserId, _user);
                //_gamesData.Update((int)_selectedGame.GameId, _selectedGame);
                _gameUsers.Add(UsersGame);
                
                Back();
            }
            catch
            {

                Back();
            }


        }
    }
}
