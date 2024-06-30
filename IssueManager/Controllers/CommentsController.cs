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
using System.Linq.Expressions;

namespace IssueManager.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IssueManagerContext _context;

        public CommentsController(IssueManagerContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index(int? issueId, string search/*, bool? byTitle*/)
        {
            ViewData["issueId"] = issueId;
            ViewData["search"] = search;
            // search by issue title if byTitle is true, otherwise search by comment content
            // empty string means no filtering
            //Expression<Func<Comment, bool>> searchPredicate = c => string.IsNullOrEmpty(search) ? true : 
                //((byTitle ?? false) ? c.Issue.Title.Contains(search) : c.Content.Contains(search));
                Expression<Func<Comment, bool>> searchPredicate = c => string.IsNullOrEmpty(search) || c.Content.Contains(search);
			return View(
                await _context.Comment
                .Include(c => c.Issue)
                .Where(c => issueId == null || c.Issue.Id == issueId)
                .Where(searchPredicate)
                .ToListAsync()
             );
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Issue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create(int? issueId)
        {
            ViewData["issueId"] = issueId;
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Author,Content,SubmitDate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (!int.TryParse(Request.Form["issueId"], out int issueId))
                {
                    this.SetTemporaryMessage("Provided issue ID is not a number or is invalid number.", Constants.BootstrapMsgType.Danger);
                    return RedirectToAction(nameof(Index));
                }
                comment.Issue = await _context.Issue.FindAsync(issueId);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Content,SubmitDate")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
