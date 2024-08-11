using System.ComponentModel.DataAnnotations;

namespace Steam.Models
{
    public class GameUser
    {
      
        public int? GamesGameId { get; set; }
        public Game Game { get; set; }
        public int? UserUserId { get; set; }
        public User User { get; set; }
    }

}
