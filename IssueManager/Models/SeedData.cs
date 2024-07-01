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

                // Example Data for Projects
                List<Project> exampleProjects = [
                    new Project
                    {
                        Name = "E-commerce Platform",
                        Description = "An online platform for buying and selling products.",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Title = "Slow sorting of product data",
                                Description = "Sorting products by price takes too long to complete.",
                                Author = "Jane Doe",
                                SubmitDate = DateTime.Now.AddDays(-10),
                                LastUpdateDate = DateTime.Now.AddDays(-5),
                                Status = IssueStatus.WORK_IN_PROGRESS,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "John Smith",
                                        Content = "I've noticed this issue as well.",
                                        SubmitDate = DateTime.Now.AddDays(-9)
                                    },
                                    new Comment
                                    {
                                        Author = "Jane Doe",
                                        Content = "Working on a solution.",
                                        SubmitDate = DateTime.Now.AddDays(-5)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Search function not returning accurate results",
                                Description = "The search function often returns irrelevant results.",
                                Author = "John Smith",
                                SubmitDate = DateTime.Now.AddDays(-15),
                                LastUpdateDate = DateTime.Now.AddDays(-7),
                                Status = IssueStatus.SUBMITTED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Jane Doe",
                                        Content = "Can you provide more details?",
                                        SubmitDate = DateTime.Now.AddDays(-14)
                                    }
                                }
                            }
                        }
                    },
                    new Project
                    {
                        Name = "Mobile Banking App",
                        Description = "A mobile application for managing bank accounts and transactions.",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Title = "Login failure on certain devices",
                                Description = "Users are unable to log in on older Android devices.",
                                Author = "Alice Johnson",
                                SubmitDate = DateTime.Now.AddDays(-8),
                                LastUpdateDate = DateTime.Now.AddDays(-3),
                                Status = IssueStatus.REPRODUCED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Bob Brown",
                                        Content = "I can reproduce this issue on my Android device.",
                                        SubmitDate = DateTime.Now.AddDays(-7)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Transaction history not updating",
                                Description = "Recent transactions are not showing up in the history.",
                                Author = "Bob Brown",
                                SubmitDate = DateTime.Now.AddDays(-12),
                                LastUpdateDate = DateTime.Now.AddDays(-1),
                                Status = IssueStatus.WORK_IN_PROGRESS,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Alice Johnson",
                                        Content = "Looking into this issue.",
                                        SubmitDate = DateTime.Now.AddDays(-10)
                                    },
                                    new Comment
                                    {
                                        Author = "Charlie Green",
                                        Content = "This seems to be affecting many users.",
                                        SubmitDate = DateTime.Now.AddDays(-9)
                                    }
                                }
                            }
                        }
                    },
                    new Project
                    {
                        Name = "Social Media Platform",
                        Description = "A platform for connecting and sharing with friends and family.",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Title = "Image upload feature broken",
                                Description = "Users are unable to upload images to their posts.",
                                Author = "David Lee",
                                SubmitDate = DateTime.Now.AddDays(-20),
                                LastUpdateDate = DateTime.Now.AddDays(-15),
                                Status = IssueStatus.FIXED,
                                CloseDate = DateTime.Now.AddDays(-14),
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Eve Black",
                                        Content = "I'm experiencing this issue as well.",
                                        SubmitDate = DateTime.Now.AddDays(-19)
                                    },
                                    new Comment
                                    {
                                        Author = "David Lee",
                                        Content = "The issue has been fixed in the latest update.",
                                        SubmitDate = DateTime.Now.AddDays(-15)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Messages not being sent",
                                Description = "Messages are stuck in the outbox and not being delivered.",
                                Author = "Eve Black",
                                SubmitDate = DateTime.Now.AddDays(-7),
                                LastUpdateDate = DateTime.Now.AddDays(-2),
                                Status = IssueStatus.ACKNOWLEDGED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "David Lee",
                                        Content = "We're aware of this issue and working on a fix.",
                                        SubmitDate = DateTime.Now.AddDays(-6)
                                    }
                                }
                            }
                        }
                    },
                    new Project
                    {
                        Name = "Library Management System",
                        Description = "A system to manage library books, members, and transactions.",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Title = "Book search returns incorrect results",
                                Description = "Searching for books by title sometimes returns irrelevant results.",
                                Author = "Michael Scott",
                                SubmitDate = DateTime.Now.AddDays(-30),
                                LastUpdateDate = DateTime.Now.AddDays(-28),
                                Status = IssueStatus.ACKNOWLEDGED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Dwight Schrute",
                                        Content = "Can confirm this issue.",
                                        SubmitDate = DateTime.Now.AddDays(-29)
                                    },
                                    new Comment
                                    {
                                        Author = "Pam Beesly",
                                        Content = "This is affecting the catalog search.",
                                        SubmitDate = DateTime.Now.AddDays(-28)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Unable to add new members",
                                Description = "The system throws an error when trying to add new members.",
                                Author = "Jim Halpert",
                                SubmitDate = DateTime.Now.AddDays(-25),
                                LastUpdateDate = DateTime.Now.AddDays(-22),
                                Status = IssueStatus.WORK_IN_PROGRESS,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Pam Beesly",
                                        Content = "This needs to be fixed ASAP.",
                                        SubmitDate = DateTime.Now.AddDays(-24)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Overdue fines not being calculated",
                                Description = "Overdue fines are not being applied to late returns.",
                                Author = "Angela Martin",
                                SubmitDate = DateTime.Now.AddDays(-20),
                                LastUpdateDate = DateTime.Now.AddDays(-19),
                                Status = IssueStatus.REPRODUCED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Oscar Martinez",
                                        Content = "I have tested this and can reproduce the issue.",
                                        SubmitDate = DateTime.Now.AddDays(-19)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "Email notifications not sent",
                                Description = "Users are not receiving email notifications for overdue books.",
                                Author = "Kevin Malone",
                                SubmitDate = DateTime.Now.AddDays(-18),
                                LastUpdateDate = DateTime.Now.AddDays(-16),
                                Status = IssueStatus.SUBMITTED
                            },
                            new Issue
                            {
                                Title = "Incorrect book details displayed",
                                Description = "Book details page shows incorrect information.",
                                Author = "Stanley Hudson",
                                SubmitDate = DateTime.Now.AddDays(-15),
                                LastUpdateDate = DateTime.Now.AddDays(-13),
                                Status = IssueStatus.WORK_IN_PROGRESS
                            },
                            new Issue
                            {
                                Title = "Page not found error",
                                Description = "Navigating to certain pages results in a 404 error.",
                                Author = "Phyllis Vance",
                                SubmitDate = DateTime.Now.AddDays(-12),
                                LastUpdateDate = DateTime.Now.AddDays(-10),
                                Status = IssueStatus.FIXED,
                                CloseDate = DateTime.Now.AddDays(-8),
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Meredith Palmer",
                                        Content = "The issue seems to be fixed now.",
                                        SubmitDate = DateTime.Now.AddDays(-7)
                                    }
                                }
                            },
                            new Issue
                            {
                                Title = "System slow during peak hours",
                                Description = "The system becomes very slow during peak usage hours.",
                                Author = "Andy Bernard",
                                SubmitDate = DateTime.Now.AddDays(-10),
                                LastUpdateDate = DateTime.Now.AddDays(-9),
                                Status = IssueStatus.WONT_FIX
                            },
                            new Issue
                            {
                                Title = "Barcode scanner not working",
                                Description = "The barcode scanner is not recognizing book barcodes.",
                                Author = "Ryan Howard",
                                SubmitDate = DateTime.Now.AddDays(-8),
                                LastUpdateDate = DateTime.Now.AddDays(-6),
                                Status = IssueStatus.WORK_IN_PROGRESS
                            },
                            new Issue
                            {
                                Title = "Unable to renew books online",
                                Description = "Users are unable to renew books through the online portal.",
                                Author = "Kelly Kapoor",
                                SubmitDate = DateTime.Now.AddDays(-5),
                                LastUpdateDate = DateTime.Now.AddDays(-4),
                                Status = IssueStatus.ACKNOWLEDGED
                            },
                            new Issue
                            {
                                Title = "Books marked as available are checked out",
                                Description = "The system shows books as available even though they are checked out.",
                                Author = "Creed Bratton",
                                SubmitDate = DateTime.Now.AddDays(-3),
                                LastUpdateDate = DateTime.Now.AddDays(-2),
                                Status = IssueStatus.CANNOT_REPRODUCE
                            },
                            new Issue
                            {
                                Title = "Reservation system not working",
                                Description = "Users are unable to reserve books.",
                                Author = "Toby Flenderson",
                                SubmitDate = DateTime.Now.AddDays(-1),
                                LastUpdateDate = DateTime.Now,
                                Status = IssueStatus.SUBMITTED
                            }
                        }
                    },
                    new Project
                    {
                        Name = "Task Management Tool",
                        Description = "A tool to manage tasks and track progress for teams.",
                        Issues = new List<Issue>
                        {
                            new Issue
                            {
                                Title = "Notifications not showing up",
                                Description = "Users are not receiving notifications for task updates.",
                                Author = "Jason Taylor",
                                SubmitDate = DateTime.Now.AddDays(-10),
                                LastUpdateDate = DateTime.Now.AddDays(-5),
                                Status = IssueStatus.REPRODUCED,
                                Comments = new List<Comment>
                                {
                                    new Comment
                                    {
                                        Author = "Sarah Lee",
                                        Content = "Same issue here.",
                                        SubmitDate = DateTime.Now.AddDays(-9)
                                    },
                                    new Comment
                                    {
                                        Author = "Tom Brown",
                                        Content = "Hope this gets fixed soon.",
                                        SubmitDate = DateTime.Now.AddDays(-8)
                                    },
                                    new Comment
                                    {
                                        Author = "Emily White",
                                        Content = "Facing this problem as well.",
                                        SubmitDate = DateTime.Now.AddDays(-7)
                                    },
                                    new Comment
                                    {
                                        Author = "Daniel Green",
                                        Content = "Any updates on this?",
                                        SubmitDate = DateTime.Now.AddDays(-6)
                                    },
                                    new Comment
                                    {
                                        Author = "Jason Taylor",
                                        Content = "Still looking into it.",
                                        SubmitDate = DateTime.Now.AddDays(-5)
                                    },
                                    new Comment
                                    {
                                        Author = "Sarah Lee",
                                        Content = "Noticed this since the last update.",
                                        SubmitDate = DateTime.Now.AddDays(-4)
                                    },
                                    new Comment
                                    {
                                        Author = "Tom Brown",
                                        Content = "Please prioritize this.",
                                        SubmitDate = DateTime.Now.AddDays(-3)
                                    },
                                    new Comment
                                    {
                                        Author = "Emily White",
                                        Content = "Critical issue for our team.",
                                        SubmitDate = DateTime.Now.AddDays(-2)
                                    },
                                    new Comment
                                    {
                                        Author = "Daniel Green",
                                        Content = "Is there a workaround?",
                                        SubmitDate = DateTime.Now.AddDays(-1)
                                    },
                                    new Comment
                                    {
                                        Author = "Jason Taylor",
                                        Content = "Working on a fix, will update soon.",
                                        SubmitDate = DateTime.Now
                                    },
                                    new Comment
                                    {
                                        Author = "Sarah Lee",
                                        Content = "Thanks for the update.",
                                        SubmitDate = DateTime.Now
                                    }
                                }
                            }
                        }
                    }
                ];

                context.Project.AddRange(exampleProjects.Append(project));
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

            return project;
        }
    }
}
