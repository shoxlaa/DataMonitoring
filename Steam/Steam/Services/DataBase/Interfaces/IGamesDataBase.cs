using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Steam.Models;
using System;
using System.Collections.Generic;

namespace Steam.Services.DataBase.Interfaces
{
    public interface IGamesDataBase : IEnumerable<Game>
    {
        public bool Add(Game game);

        public bool Remove(Game game);
        public List<Game> GetAll();
        public Game GetById(int id);
        public void Update(int OldGameId, Game NewGame);
        //public List<Game> Find(Func<string, List<Game>> options);
    }
}
