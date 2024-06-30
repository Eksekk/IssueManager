using IssueManager.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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

                createExampleUsers(provider, context);

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

        public static void createExampleUsers(IServiceProvider provider, IssueManagerContext context)
        {
            var userStore = new UserStore<User, IdentityRole<int>, IssueManagerContext, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>(context);
            var passwordHasher = new PasswordHasher<User>();
            var userValidators = new List<IUserValidator<User>>();
            var passwordValidators = new List<IPasswordValidator<User>>();
            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();
            var optionsAccessor = provider.GetRequiredService<IOptions<IdentityOptions>>();

            var userManager = new UserManager<User>(userStore, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, provider, new Logger<UserManager<User>>(new LoggerFactory()));

            userManager.CreateAsync(new User
            {
                UserName = "User1",
                Email = "user1@hotmail.com",
                PhoneNumber = "123456789",

            }, "pass1").Wait();

            var users2 = new[]
            {
                new { User = new User { UserName = "Alice123", Email = "alice123@gmail.com", PhoneNumber = "555-1234" }, Password = "defaultPass!" },
                new { User = new User { UserName = "Bob456", Email = "bob456@yahoo.com", PhoneNumber = "555-5678" }, Password = "defaultPass!" },
                new { User = new User { UserName = "Charlie789", Email = "charlie789@outlook.com", PhoneNumber = "555-9012" }, Password = "charliePass1" },
                new { User = new User { UserName = "Diana321", Email = "diana321@gmail.com", PhoneNumber = "555-3456" }, Password = "dianaPass2" },
                new { User = new User { UserName = "Evan654", Email = "evan654@yahoo.com", PhoneNumber = "555-7890" }, Password = "defaultPass!" },
                new { User = new User { UserName = "Fiona987", Email = "fiona987@outlook.com", PhoneNumber = "555-1235" }, Password = "defaultPass!" },
                new { User = new User { UserName = "George432", Email = "george432@gmail.com", PhoneNumber = "555-5671" }, Password = "defaultPass!" },
                new { User = new User { UserName = "Hannah876", Email = "hannah876@yahoo.com", PhoneNumber = "555-9013" }, Password = "hannahPass3" },
                new { User = new User { UserName = "Ian543", Email = "ian543@outlook.com", PhoneNumber = "555-3457" }, Password = "defaultPass!" },
                new { User = new User { UserName = "Julia210", Email = "julia210@gmail.com", PhoneNumber = "555-7891" }, Password = "defaultPass!" },
                new { User = new User { UserName = "admin", Email = "admin@gmail.com", PhoneNumber = "333-2432" }, Password = "admin" }
            };
            foreach (var item in users2)
            {
                item.User.EmailConfirmed = true;
                item.User.PhoneNumberConfirmed = true;
                userManager.CreateAsync(item.User, item.Password).Wait();
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
