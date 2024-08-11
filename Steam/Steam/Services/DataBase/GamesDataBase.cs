using Microsoft.EntityFrameworkCore;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steam.Services.DataBase
{
    //добавить удаление игры 
    //добавить удаление юзера 
    //восстонавить пароль  юзера 
    public class GamesDataBase : IGamesDataBase
    {
        private readonly SteamDbContext _steamDb = new SteamDbContext(new DbContextOptionsBuilder<SteamDbContext>().Options);

        public bool Add(Game game)
        {
            try
            {
                game.GameId = null;
                _steamDb.Games.Add(game);
                _steamDb.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        

        public List<Game> GetAll()
        {
            return _steamDb.Games.Where<Game>(x => x.GameId >= 0).ToList();
        }

        public Game? GetById(int id)
        {
            return _steamDb.Games.FirstOrDefault<Game>(x => x.GameId == id);
        }

        public IEnumerator<Game> GetEnumerator()
        {
            foreach (var product in _steamDb.Games)
            {
                yield return product;

            }
        }

        public bool Remove(Game game)
        {
            throw new System.NotImplementedException();
        }

        public void Update(int OldGameId, Game NewGame)
        {
            var v = GetById(OldGameId);

            if (v != null)
            {
                v.Publisher = NewGame.Publisher;
                v.Name = NewGame.Name;
                v.Coast = NewGame.Coast;
                v.Category = NewGame.Category;
                v.CreationDate = NewGame.CreationDate;
                v.Description = NewGame.Description;
                v.Developer = NewGame.Developer;
                v.GameUser = NewGame.GameUser;  
            }

            _steamDb.SaveChanges();

        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
