using Microsoft.EntityFrameworkCore;
using Steam.Models;
using Steam.Services.DataBase.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Steam.Services.DataBase
{
    public class UserDataBase : IUserDataBase
    {
        private readonly SteamDbContext _steamDb = new SteamDbContext(new DbContextOptionsBuilder<SteamDbContext>().Options);
        public bool Add(User user)
        {
            try
            {
                user.UserId = null;
                  
               _steamDb.Users.Add(user);
                _steamDb.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public User? Find(string UserName, string Password)
        {
           return _steamDb.Users.FirstOrDefault<User>(x => x.UserName == UserName && x.Password == Password);
        }

        public List<User> GetAll()
        {
            return _steamDb.Users.Where(p => p.UserId >= 0).ToList();
        }

        public IEnumerator<User> GetEnumerator()
        {
            foreach (var product in _steamDb.Users)
            {
                yield return product;

            }
        }

        public User? GetUserById(int id)
        {
            return _steamDb.Users.SingleOrDefault(p => p.UserId == id);
        }

        public bool Remove(User user)
        {
            try
            {
                _steamDb.Users.Remove(user);
                _steamDb.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Update(int id , User newUser)
        {
            try
            {

                var result = GetUserById(id);

                //result.GamesUser.Add();
                result.Name = newUser.Name;
                result.UserName = newUser.UserName;
                result.Password = newUser.Password;
                _steamDb.SaveChanges();

            }
            catch
            {

            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
