using Bogus;
using Microsoft.EntityFrameworkCore;
using Steam.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Steam.Services.DataBase
{
    public class SteamDbContext : DbContext
    {
        public SteamDbContext(DbContextOptions<SteamDbContext> options) : base(options)
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
                          
                v.HasData(new User() { Name="Admin", Password="Admin", UserName="Admin"});
            });

            modelBuilder.Entity<Game>(g =>
            {
               
                g.HasData(new Game() { Name = "Overwatch", Coast = 10, Category = "shooter", CreationDate = DateTime.Now, Description = "The best game ever", Developer = "I dont know ", Publisher = "Pups", });
            });
          
        }
    }
}
