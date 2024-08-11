using Steam.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steam.Services.DataBase.Interfaces
{
    public interface IUserDataBase : IEnumerable<User>
    {
        public bool Add(User user);
        public void Update(int id, User user);
        public bool Remove(User user);
        public List<User> GetAll();
        public User GetUserById(int id);
        public User Find(string UserName, string Password);
    }
}
