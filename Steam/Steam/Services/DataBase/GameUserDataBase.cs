using Microsoft.EntityFrameworkCore;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steam.Services.DataBase
{
    public class GameUserDataBase : IGameUserDataBase
    {
        private readonly SteamDbContext _steamDb = new SteamDbContext(new DbContextOptionsBuilder<SteamDbContext>().Options);

        public bool Add(GameUser user)
        {
            try
            {
                _steamDb.GameUser.Add(user);
                _steamDb.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<GameUser> GetAll()
        {
            return _steamDb.GameUser.Where(x=> x.UserUserId>=0).ToList();
        }

        public List<Game> GetAllGames(int UserId)
        {
           var values= _steamDb.GameUser.Where(c=> c.UserUserId == UserId).ToList();
            var Games= values.Select(d => d.Game).ToList();
            return Games;
        }

        public IEnumerator<GameUser> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
