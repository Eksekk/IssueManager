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
                // Look for any projects.
                if (context.Project.Any())
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

                List<Issue> issues = new()
                {
                    new Issue
                    {
                        Title = "Issue 1",
                        Description = "Description 1",
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
                        Author = "Author 3",
                        SubmitDate = DateTime.Parse("2024-06-05"),
                        LastUpdateDate = DateTime.Parse("2024-06-05"),
                        CloseDate = null
                    }
                };

                var project = new Project
                {
                    Name = "Project 1",
                    Description = "Description 1",
                    Issues = issues
                };

                

                context.Project.AddRange(project);
                context.SaveChanges();
            }
        }

        private static Project getCppProject()
        {
            var project = new Project
            {
                Name = "Website crawler",
                Description = "A simple C++ program using Boost.Beast to get useful data from any website."
            };

            List<Issue> issues = new()
            {
                new()
                {
                    Title = "Some pages are skipped when crawling",
                    Description = "Some pages are skipped when crawling, but they should not be. Reason is currently unknown. There are very little such pages, which makes it hard to pinpoint the problem",
                    Author = "Author 1",
                    SubmitDate = DateTime.Parse("2024-06-05 8:55"),
                    LastUpdateDate = DateTime.Parse("2024-06-12 12:23"),
                    Status = IssueStatus.WORK_IN_PROGRESS
                }
            };

            project.Issues = issues;

            return new();
        }
    }
}
