using Steam.Models;
using System.Collections.Generic;

namespace Steam.Services.DataBase.Interfaces
{
    public interface IGameUserDataBase : IEnumerable<GameUser>
    {
        public bool Add(GameUser user);
        public List<Game> GetAllGames(int UserId);
        public List<GameUser> GetAll();
    }
}
