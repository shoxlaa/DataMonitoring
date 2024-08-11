using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Steam.Models
{
    public class Game
    {
        [Key]
        public int? GameId { get; set; } = 1;
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public string? Developer { get; set; }
        public string? Publisher { get; set; }
        public int? Rating { get; set; }
        public float? Coast { get; set; }
        public string? Category { get; set; }
        public string? Description { get; set; }

        public List<GameUser>? GameUser { get; set; }= new List<GameUser>(){ };

    }

}
