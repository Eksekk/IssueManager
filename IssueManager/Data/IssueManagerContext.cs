using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IssueManager.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace IssueManager.Data
{
    public class IssueManagerContext : IdentityDbContext
    {
        public IssueManagerContext (DbContextOptions<IssueManagerContext> options)
            : base(options)
        {
        }

        //public DbSet<User> User { get; set; } = default!;
        public DbSet<IssueManager.Models.Issue> Issue { get; set; } = default!;
        public DbSet<IssueManager.Models.Comment> Comment { get; set; } = default!;
        public DbSet<IssueManager.Models.Project> Project { get; set; } = default!;
        // dbset of users
        public DbSet<User> User { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Issue>()
                .Property(i => i.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Issue>()
                .HasOne(i => i.project)
                .WithMany(p => p.Issues)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Issue)
                .WithMany(i => i.Comments)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
