using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IssueManager.Models;

namespace IssueManager.Data
{
    public class IssueManagerContext : DbContext
    {
        public IssueManagerContext (DbContextOptions<IssueManagerContext> options)
            : base(options)
        {
        }

        public DbSet<IssueManager.Models.Issue> Issue { get; set; } = default!;
    }
}
