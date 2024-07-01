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
using static IssueManager.Static_classes.ExtensionMethods;

namespace IssueManager.Controllers
{
	public class ProjectsController : Controller
	{
		private readonly IssueManagerContext _context;

		private void ProjectDoesntExistMsg() => this.SetTemporaryMessage("Provided project doesn't exist in the database.", Constants.BootstrapMsgType.Danger);

        public ProjectsController(IssueManagerContext context)
		{
			_context = context;
		}

		// GET: Projects
		public async Task<IActionResult> Index()
		{
			return View(await _context.Project.Include(p => p.Issues).ToListAsync());
        }

		// GET: Projects/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Project
				.Include(p => p.Issues)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (project == null)
			{
				return NotFound();
			}

			return View(project);
		}

		// GET: Projects/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Projects/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Name,Description")] Project project)
		{
			// validate only some fields
			if (new List<string>{nameof(Project.Name), nameof(Project.Description)}.All(s => ModelState.IsFieldValid(s)))
			{
				_context.Add(project);
				await _context.SaveChangesAsync();
                this.SetTemporaryMessage("Project created successfully.", Constants.BootstrapMsgType.Success);
                return RedirectToAction(nameof(Index));
			}
            this.SetTemporaryMessage("Project was not created due to invalid data.", Constants.BootstrapMsgType.Danger);
            return View(project);
		}

		// GET: Projects/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var project = await _context.Project.FindAsync(id);
			if (project == null)
			{
				return NotFound();
			}
			return View(project);
		}

		// POST: Projects/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Name,Description")] Project project)
		{
            // this actually needs to get comments from DB as well, even though they're not used here, otherwise model validation will fail
            var projectInDb = await _context.Project.Include(p => p.Issues).ThenInclude(i => i.Comments).SingleAsync(p => p.Id == id);
			if (projectInDb is null)
			{
				ProjectDoesntExistMsg();
                return NotFound();
            }
			projectInDb.Name = project.Name;
            projectInDb.Description = project.Description;

            ModelState.Clear();
			TryValidateModel(projectInDb);
            if (ModelState.IsValid)
			{
				try
				{
					_context.Update(projectInDb);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProjectExists(projectInDb.Id))
					{
                        // not using ProjectDoesntExistMsg() here, because message is different
                        this.SetTemporaryMessage("Provided project was asynchronously deleted from the database.", Constants.BootstrapMsgType.Danger);
                        return NotFound();
					}
					else
					{
						throw;
					}
				}
                this.SetTemporaryMessage("Project updated successfully.", Constants.BootstrapMsgType.Success);
                return RedirectToAction(nameof(Index));
			}
			this.SetTemporaryMessage("Project was not updated due to invalid data.", Constants.BootstrapMsgType.Danger);
			return View(project);
		}

		// GET: Projects/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
                return NotFound();
			}

			var project = await _context.Project
				.FirstOrDefaultAsync(m => m.Id == id);
			if (project == null)
			{
                return NotFound();
			}

			return View(project);
		}

		// POST: Projects/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var project = await _context.Project.FindAsync(id);
			if (project != null)
			{
				/*
                 copilot:
                Note that for the cascading delete to work, you need to have set up your database schema correctly. In Entity Framework, you can configure cascading deletes in the OnModelCreating method in your DbContext class:
                protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Project>()
        .HasMany(p => p.Tasks)
        .WithOne(t => t.Project)
        .OnDelete(DeleteBehavior.Cascade);
}
*/
                var projectToRemove = await _context.Project.Include(p => p.Issues).ThenInclude(i => i.Comments).SingleAsync(p => p.Id == id);
                _context.Project.Remove(projectToRemove);
				this.SetTemporaryMessage($"Project '{project.Name}' deleted successfully.", Constants.BootstrapMsgType.Success);
            }
			else
			{
				this.SetTemporaryMessage("Project requested to be deleted doesn't exist.", Constants.BootstrapMsgType.Danger);
            }

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProjectExists(int id)
		{
			return _context.Project.Any(e => e.Id == id);
		}
	}
}
