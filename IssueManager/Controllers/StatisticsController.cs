using IssueManager.Data;
using Microsoft.AspNetCore.Mvc;
using IssueManager.Models;
using Microsoft.EntityFrameworkCore;

namespace IssueManager.Controllers
{
	using CountIdPair = KeyValuePair<int, int>;
	public class StatisticsController : Controller
	{
        private readonly IssueManagerContext _context;
        public StatisticsController(IssueManagerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Statistics()
        {
            List<Project> projects = await _context.Project.Include(p => p.Issues).ThenInclude(i => i.Comments).ToListAsync();
            // calculate various statistics about whole state of IssueManager
            // for example: number of projects, number of issues, number of comments, etc.

            int projectCount = projects.Count, issueCount = projects.Sum(p => p.Issues.Count), commentCount = projects.Sum(p => p.Issues.Sum(i => i.Comments.Count));
            double averageIssuesPerProject = (double)issueCount / projectCount;
            double averageCommentsPerIssue = (double)commentCount / issueCount;
            double averageCommentsPerProject = (double)commentCount / projectCount;
            Dictionary<IssueStatus, int> issuesByStatus = projects.SelectMany(p => p.Issues).GroupBy(i => i.Status).ToDictionary(g => g.Key, g => g.Count());
            Dictionary<string, int> issuesByAuthor = projects.SelectMany(p => p.Issues).GroupBy(i => i.Author).ToDictionary(g => g.Key, g => g.Count());
            Dictionary<string, CountIdPair> issuesByProject = projects.ToDictionary(p => p.Name, p => new CountIdPair(p.Issues.Count, p.Id));
            Dictionary<string, CountIdPair> commentsByIssue = projects.SelectMany(p => p.Issues).ToDictionary(i => i.Title, i => new CountIdPair(i.Comments.Count, i.Id));
            Dictionary<string, CountIdPair> commentsByProject = projects.ToDictionary(p => p.Name, p => new CountIdPair(p.Issues.Sum(i => i.Comments.Count), p.Id));

            Dictionary<string, int> issuesByProjectIds = projects.ToDictionary(p => p.Name, p => p.Id);

            ViewData["projectCount"] = projectCount;
            ViewData["issueCount"] = issueCount;
            ViewData["commentCount"] = commentCount;
            ViewData["averageIssuesPerProject"] = averageIssuesPerProject;
            ViewData["averageCommentsPerIssue"] = averageCommentsPerIssue;
            ViewData["averageCommentsPerProject"] = averageCommentsPerProject;
            ViewData["issuesByStatus"] = issuesByStatus;
            ViewData["issuesByAuthor"] = issuesByAuthor;
            ViewData["issuesByProject"] = issuesByProject;
            ViewData["commentsByIssue"] = commentsByIssue;
            ViewData["commentsByProject"] = commentsByProject;

            return View();
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction(nameof(Statistics));
        }
    }
}
