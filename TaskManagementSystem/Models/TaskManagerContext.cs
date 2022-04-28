using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models.Configuration;

namespace TaskManagementSystem.Models
{
    public class TaskManagerContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
        public DbSet<Task> Tasks { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("path");
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new BoardMemberConfiguration());
            
            modelBuilder.Entity<Project>()
                .HasIndex(b => b.key)
                .IsUnique();

            modelBuilder.Entity<Project>()
                 .HasOne(c => c.owner);

            modelBuilder.Entity<Account>()
                .HasIndex(a => a.email)
                .IsUnique();

            modelBuilder.Entity<Board>()
                .HasIndex(a => a.name)
                .IsUnique();

            modelBuilder.Entity<Board>()
                .HasOne(c => c.owner);
         }
    }
}
