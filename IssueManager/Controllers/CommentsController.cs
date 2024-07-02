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
        public async Task<IActionResult> Index(int? issueId, string search/*, bool? byTitle*/, string sort)
        {
            ViewData["issueId"] = issueId;
            // providing this value here instead of determining it in view, because view cannot do it if there isn't any issue in result sequence
            ViewData["issueName"] = issueId is null ? "All issues" : _context.Issue.Find(issueId).Title;


            ViewData["search"] = search;
            // empty string means no filtering
                Expression<Func<Comment, bool>> searchPredicate = c => string.IsNullOrEmpty(search) || c.Content.Contains(search);
			return View(
                await _context.Comment
                .Include(c => c.Issue)
                .Where(c => issueId == null || c.Issue.Id == issueId)
                .Where(searchPredicate)
                .OrderBy(c => c.SubmitDate)
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
        public async Task<IActionResult> Create(int? issueId)
        {
            if (issueId is null)
            {
                //this.SetTemporaryMessage("Cannot create a comment not referencing any issue. Go to issues page, select comments from there, and try again.", Constants.BootstrapMsgType.Danger);
                //return RedirectToAction(nameof(Index));
            }
            //ViewData["issueId"] = issueId;
            // assign SelectList of issues (key is id, value is issue name) to ViewBag
            ViewBag.IssueId = await _context.Issue.Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Title }).ToListAsync();
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Author,Content")] Comment comment)
        {
            if (new[] {nameof(Comment.Author), nameof(Comment.Content)}.All(s => ModelState.IsFieldValid(s)))
            {
                if (!int.TryParse(Request.Form["Issue.Id"], out int issueId))
                {
                    this.SetTemporaryMessage("Provided issue ID is not a number or is invalid number.", Constants.BootstrapMsgType.Danger);
                    return RedirectToAction(nameof(Index));
                }
                comment.SubmitDate = DateTime.Now;
                comment.Issue = await _context.Issue.FindAsync(issueId);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                this.SetTemporaryMessage("Comment created successfully.", Constants.BootstrapMsgType.Success);
                return RedirectToAction(nameof(Index), new { issueId = issueId });
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
        public async Task<IActionResult> Edit([Bind("Author,Content,SubmitDate")] Comment comment)
        {
            int id = Convert.ToInt32(Request.Form["Id"]);
            Comment commentInDb = await _context.Comment.FindAsync(id);
            if (commentInDb == null)
			{
				return NotFound();
			}

            if (new[] { nameof(Comment.Author), nameof(Comment.Content), nameof(Comment.SubmitDate) }.All(s => ModelState.IsFieldValid(s)))
            {
				try
				{
					commentInDb.Author = comment.Author;
					commentInDb.Content = comment.Content;
					commentInDb.SubmitDate = comment.SubmitDate;
					_context.Update(commentInDb);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CommentExists(commentInDb.Id)) // probably useless (id is checked above), but I'll leave it
					{
                        this.SetTemporaryMessage("Comment was not updated due to invalid data.", Constants.BootstrapMsgType.Danger);
						return NotFound();
					}
					else
					{
						throw;
					}
				}
                this.SetTemporaryMessage("Comment updated successfully.", Constants.BootstrapMsgType.Success);
				return RedirectToAction(nameof(Index));
			}
            else
			{
				this.SetTemporaryMessage("Comment was not updated due to invalid data.", Constants.BootstrapMsgType.Danger);
				return View(comment);
            }
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            if (comment != null)
            {
                _context.Comment.Remove(comment);
                await _context.SaveChangesAsync();
                this.SetTemporaryMessage("Comment deleted successfully.", Constants.BootstrapMsgType.Success);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
