using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Steam.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        [Key]
        public int? UserId { get; set; } = 0;
        public string Name { get; set; }
        public string? UserName { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

        public List<GameUser>? GamesUser { get; set; } = new List<GameUser>();   
    }
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameUser> GameUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=SteamDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>().HasKey(x => new { x.GamesGameId, x.UserUserId });

            modelBuilder.Entity<GameUser>().HasOne(u => u.User)
                .WithMany(h => h.GamesUser)
                .HasForeignKey(f => f.UserUserId);

            modelBuilder.Entity<GameUser>().HasOne(u => u.Game)
                .WithMany(p => p.GameUser)
                .HasForeignKey(f => f.GamesGameId);

            modelBuilder.Entity<User>(v =>
            {
                v.HasIndex(b => b.UserName).IsUnique();
                v.HasKey(b => b.UserId);
            }
            );

            modelBuilder.Entity<Game>().HasKey(k => k.GameId);
            modelBuilder.Entity<GameUser>(gu =>
            {
                gu.HasKey(k => k.GamesGameId);
                gu.HasKey(k => k.UserUserId);
            });


        }
    }

}
