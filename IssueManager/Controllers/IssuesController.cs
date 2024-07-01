using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueManager.Data;
using IssueManager.Models;
using IssueManager.Static_classes;
using X.PagedList;

namespace IssueManager.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IssueManagerContext _context;

        public IssuesController(IssueManagerContext context)
        {
            _context = context;
        }

        public enum EIssuesSort
        {
            AUTHOR,
            AUTHOR_DESC,
            TITLE,
            TITLE_DESC,
            STATUS,
            STATUS_DESC,
        }

        public static readonly Dictionary<string, EIssuesSort> SortOptionsStringToEnum = new() {
            { "author", EIssuesSort.AUTHOR },
            { "author_desc", EIssuesSort.AUTHOR_DESC },
            { "title", EIssuesSort.TITLE },
            { "title_desc", EIssuesSort.TITLE_DESC },
            { "status", EIssuesSort.STATUS },
            { "status_desc", EIssuesSort.STATUS_DESC },
        };

        public static readonly Dictionary<EIssuesSort, string> SortOptionsEnumToString = SortOptionsStringToEnum.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static readonly EIssuesSort SortOptionsDefault = EIssuesSort.TITLE;
        public static readonly string SortOptionsDefaultString = SortOptionsEnumToString.GetValueOrDefault(SortOptionsDefault);

        private static EIssuesSort? GetSortFromString(string sort) // may be empty
        {
            if (string.IsNullOrEmpty(sort))
            {
                return null;
            }
            return SortOptionsStringToEnum.GetValueOrDefault(sort, SortOptionsDefault);
        }

        private enum EColumnSortStatus
        {
            Absent,
            Asc,
            Desc
        }
        // GET: Issues
        public async Task<IActionResult> Index(int? projectId, string search, string sort, string currentFilter, int? page)
        {
            ViewData["search"] = search;
            ViewData["projectId"] = projectId;

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }
            ViewBag.currentFilter = search;
            ViewBag.currentSort = sort;
            
            IQueryable<Issue> issues = _context.Issue.Where(i =>
                (projectId == null
                    || i.project.Id == projectId)
                && (string.IsNullOrEmpty(search) || i.Description.Contains(search)))
                .Include(i => i.Comments).Include(i => i.project);

            var sortType = GetSortFromString(sort);
            EColumnSortStatus authorSort = EColumnSortStatus.Absent, titleSort = EColumnSortStatus.Absent, statusSort = EColumnSortStatus.Absent;
            switch (sortType)
            {
                case EIssuesSort.AUTHOR:
                    issues = issues.OrderBy(i => i.Author);
                    authorSort = EColumnSortStatus.Asc;
                    break;
                case EIssuesSort.AUTHOR_DESC:
                    issues = issues.OrderByDescending(i => i.Author);
                    authorSort = EColumnSortStatus.Desc;
                    break;
                case EIssuesSort.TITLE:
                    issues = issues.OrderBy(i => i.Title);
                    titleSort = EColumnSortStatus.Asc;
                    break;
                case EIssuesSort.TITLE_DESC:
                    issues = issues.OrderByDescending(i => i.Title);
                    titleSort = EColumnSortStatus.Desc;
                    break;
                case EIssuesSort.STATUS:
                    issues = issues.OrderBy(i => i.Status);
                    statusSort = EColumnSortStatus.Asc;
                    break;
                case EIssuesSort.STATUS_DESC:
                    issues = issues.OrderByDescending(i => i.Status);
                    statusSort = EColumnSortStatus.Desc;
                    break;
                case null:
                default:
                    issues = issues.OrderBy(i => i.Title);
                    break;
            }

            // fill view bag with what to sort with
            ViewBag.authorSort = authorSort == EColumnSortStatus.Asc ? "author_desc" : "author";
            ViewBag.titleSort = titleSort == EColumnSortStatus.Asc ? "title_desc" : "title";
            ViewBag.statusSort = statusSort == EColumnSortStatus.Asc ? "status_desc" : "status";

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var list = issues.ToPagedList(pageNumber, pageSize);
            return View(list);
        }
//

        // GET: Issues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .Include(i => i.project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create(int projectId)
        {
            ViewData["projectId"] = projectId;
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // values that are assigned below are omitted from bind list
        public async Task<IActionResult> Create([Bind("Title,Description,Author")] Issue issue)
        {
            if (!int.TryParse(Request.Form["projectId"], out int projectId))
            {
                this.SetTemporaryMessage("Invalid project id", Constants.BootstrapMsgType.Danger);
                return View(issue);
            }
            if (new[] {nameof(Issue.Title), nameof(Issue.Description), nameof(Issue.Author)}.All(s => ModelState.IsFieldValid(s)))
            {
                issue.SubmitDate = DateTime.Now;
                issue.LastUpdateDate = DateTime.Now;
                issue.CloseDate = null;
                issue.Status = IssueStatus.SUBMITTED;
                issue.project = _context.Project.FindAsync(projectId).Result;
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index), new{ projectId });
        }

        // GET: Issues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue.FindAsync(id);
            if (issue == null)
            {
                return NotFound();
            }
            return View(issue);
        }

        // POST: Issues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SubmitDate CloseDate and LastUpdateDate intentionally omitted from the bind list
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,Author,Status")] Issue issue)
        {
            ModelState.Clear(); // we don't care about validation of part of data, we validate the whole entity later
            //             if (issue.Status == null)
            //             {
            //                 ModelState.AddModelError("Status", "Status is required");
            //             }
            /*else */
            if (!Enum.IsDefined(typeof(IssueStatus), issue.Status))
            {
                ModelState.AddModelError("Status", "Invalid status");
            }

            // find an entity with original id passed in url
            var originalIssue = await _context.Issue.Include(i => i.project).Include(i => i.Comments).SingleAsync(i => i.Id == id);
            if (originalIssue == null)
            {
                return NotFound();
            }
            originalIssue.Author = issue.Author;
            originalIssue.Title = issue.Title;
            originalIssue.Description = issue.Description;
            originalIssue.Status = issue.Status;
            if (originalIssue.Status == IssueStatus.CLOSED)
            {
                originalIssue.CloseDate = DateTime.Now;
            }
            else
            {
                originalIssue.CloseDate = null;
            }
            // every edit action updates the LastUpdateDate
            originalIssue.LastUpdateDate = DateTime.Now;
            try
            {
                if (!TryValidateModel(originalIssue))
                {
                    TempData["msg"] = "Validation failed";
                    TempData["msgType"] = Constants.GetBootstrapAlertClass(Constants.BootstrapMsgType.Danger);
                    return View(issue);
                }
                // _context.ChangeTracker.Clear(); // prevent EF error "The instance of entity type cannot be tracked because another instance with the same key value for {'Id'} is already being tracked"
                _context.Update(originalIssue);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(originalIssue.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Issues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issue = await _context.Issue
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // POST: Issues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issue = await _context.Issue.FindAsync(id);
            if (issue != null)
            {
                var dbIssue = _context.Issue.Include(i => i.Comments).SingleAsync(i => i.Id == id);
                _context.Remove(dbIssue);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueExists(int id)
        {
            return _context.Issue.Any(e => e.Id == id);
        }
    }
}
