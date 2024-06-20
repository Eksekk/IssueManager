using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IssueManager.Data;
using IssueManager.Models;

namespace IssueManager.Controllers
{
    public class IssuesController : Controller
    {
        private readonly IssueManagerContext _context;

        public IssuesController(IssueManagerContext context)
        {
            _context = context;
        }

        // GET: Issues
        public async Task<IActionResult> Index(int? projectId)
        {
            var list = await _context.Issue.Where(i => projectId == null || i.project.Id == projectId).Include(i => i.Comments).Include(i => i.project).ToListAsync();
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issue == null)
            {
                return NotFound();
            }

            return View(issue);
        }

        // GET: Issues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Issues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // values that are assigned below are omitted from bind list
        public async Task<IActionResult> Create([Bind("Id,Title,Description,CodeSnippet,Author")] Issue issue)
        {
            if (ModelState.IsValid)
            {
                issue.SubmitDate = DateTime.Now;
                issue.LastUpdateDate = DateTime.Now;
                issue.CloseDate = null;
                issue.Status = IssueStatus.SUBMITTED;
                _context.Add(issue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(issue);
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
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,CodeSnippet,Author,Status")] Issue issue)
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
            originalIssue.CodeSnippet = issue.CodeSnippet;
            // every edit action updates the LastUpdateDate
            originalIssue.LastUpdateDate = DateTime.Now;
            try
            {
                if (!TryValidateModel(originalIssue))
                {
                    ViewData["msg"] = "Validation failed";
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
                _context.Issue.Remove(issue);
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
