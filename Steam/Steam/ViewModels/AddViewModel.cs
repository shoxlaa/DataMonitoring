using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using Steam.Services.Mvvm;
using System.Collections.ObjectModel;

namespace Steam.ViewModels;

public partial class AddViewModel : BaseViewModel
{
    private readonly IGamesDataBase _dataBase;
    [ObservableProperty]
    private Game _newGame;

    private User SpecialUser = new();
    public AddViewModel(IGamesDataBase gamesDataBase)
    {
        _dataBase = gamesDataBase;
        _newGame = new();

    }
    [RelayCommand]
    private void AddGame()
    {
        
        _dataBase.Add(NewGame);

        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Game>(NewGame), "AddGame");
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.GamesViewModel));
    }

    [RelayCommand]
    private void Back()
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<ViewModelType>(ViewModelType.GamesViewModel));
    }
}