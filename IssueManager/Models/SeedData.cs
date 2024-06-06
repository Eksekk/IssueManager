using IssueManager.Data;
using Microsoft.EntityFrameworkCore;

namespace IssueManager.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider provider)
        {
            using (var context = new IssueManagerContext(
                               provider.GetRequiredService<DbContextOptions<IssueManagerContext>>()))
            {
                // Look for any issues.
                if (context.Issue.Any())
                {
                    return;   // DB has been seeded
                }

                List<Comment> issue1comments = new()
                {
                    new Comment
                    {
                        Author = "Author 1",
                        Content = "Content 1",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    },
                    new Comment
                    {
                        Author = "Author 2",
                        Content = "Content 2",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    },
                    new Comment
                    {
                        Author = "Author 3",
                        Content = "Content 3",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    }
                };

                List<Comment> issue2comments = new()
                {
                    new Comment
                    {
                        Author = "Author 1",
                        Content = "Content 1",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    },
                    new Comment
                    {
                        Author = "Author 2",
                        Content = "Content 2",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    },
                    new Comment
                    {
                        Author = "Author 3",
                        Content = "Content 3",
                        SubmitDate = DateTime.Parse("2024-06-05")
                    }
                };

                context.Issue.AddRange(
                    new Issue
                    {
                        Title = "Issue 1",
                        Description = "Description 1",
                        CodeSnippet = "CodeSnippet 1",
                        Author = "Author 1",
                        SubmitDate = DateTime.Parse("2024-06-05"),
                        LastUpdateDate = DateTime.Parse("2024-06-05"),
                        CloseDate = null,
                        Comments = issue1comments
                    },
                    new Issue
                    {
                        Title = "Issue 2",
                        Description = "Description 2",
                        CodeSnippet = "CodeSnippet 2",
                        Author = "Author 2",
                        SubmitDate = DateTime.Parse("2024-06-05"),
                        LastUpdateDate = DateTime.Parse("2024-06-05"),
                        CloseDate = null,
                        Comments = issue2comments
                    },
                    new Issue
                    {
                        Title = "Issue 3",
                        Description = "Description 3",
                        CodeSnippet = "CodeSnippet 3",
                        Author = "Author 3",
                        SubmitDate = DateTime.Parse("2024-06-05"),
                        LastUpdateDate = DateTime.Parse("2024-06-05"),
                        CloseDate = null
                    }
                );

                var project = new Project
                {
                    Name = "Project 1",
                    Description = "Description 1",
                    Issues = [.. context.Issue] // this weird syntax makes a list from DbSet
                };

                context.Project.AddRange(project);
                context.SaveChanges();
            }
        }
    }
}
