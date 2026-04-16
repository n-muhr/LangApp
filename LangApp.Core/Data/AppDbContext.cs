using LangApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LangApp.Core.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<ReviewHistory> ReviewHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Word>()
                .HasIndex(w => w.SourceText);

            modelBuilder.Entity<ReviewHistory>()
                .HasOne(rh => rh.Word)
                .WithMany(w => w.ReviewHistories)
                .HasForeignKey(rh => rh.WordId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewHistory>()
                .HasOne(rh => rh.User)
                .WithMany()
                .HasForeignKey(rh => rh.UserId);
        }
    }
}
