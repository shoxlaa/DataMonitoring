using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using Steam.Services.Mvvm;
using Steam.ViewModels.Base;

namespace Steam.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {

        [ObservableProperty]
        private User? _user = new();

        [ObservableProperty]
        private string? _userName;
        [ObservableProperty]
        private string? _password;

        private readonly IUserDataBase _userData;
        private readonly IGameUserDataBase _gameUsers;

        public LoginViewModel(IUserDataBase userData, IGameUserDataBase gameUsers)
        {
            _userData = userData;
            _gameUsers = gameUsers;
            
        }
        [RelayCommand]
        private void CreateAccount()
        {
            _userData.Add(User!);
            MessageBox.Show("Account Added ");
            _user = new();
        }
        [RelayCommand]
        private void SignIn()
        {
            var myUser = _userData.Find(UserName!, Password!);
            if (myUser != null)
            {
                myUser.GamesUser = _gameUsers.GetAll().Where(x => x.UserUserId == myUser.UserId).ToList();
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.GamesViewModel));
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<User?>(myUser), "MyUser");
            }
            else
            {
                MessageBox.Show("Try again");
            }
        }


    }
}
