using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Standups.Data;
using Standups.Models;

namespace Standups.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class CommentsController : Controller
    {
        private readonly StandupsDbContext _context;

        public CommentsController(StandupsDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Comments
        public async Task<IActionResult> Index()
        {
            var standupsDbContext = _context.Supcomments.Include(s => s.Supquestion);
            return View(await standupsDbContext.ToListAsync());
        }

        // GET: Admin/Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Supcomments == null)
            {
                return NotFound();
            }

            var supcomment = await _context.Supcomments
                .Include(s => s.Supquestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supcomment == null)
            {
                return NotFound();
            }

            return View(supcomment);
        }

        // GET: Admin/Comments/Create
        public IActionResult Create()
        {
            ViewData["SupquestionId"] = new SelectList(_context.Supquestions, "Id", "Question");
            return View();
        }

        // POST: Admin/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SupquestionId,SubmissionDate,Comment,AdvisorRating")] Supcomment supcomment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supcomment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupquestionId"] = new SelectList(_context.Supquestions, "Id", "Question", supcomment.SupquestionId);
            return View(supcomment);
        }

        // GET: Admin/Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Supcomments == null)
            {
                return NotFound();
            }

            var supcomment = await _context.Supcomments.FindAsync(id);
            if (supcomment == null)
            {
                return NotFound();
            }
            ViewData["SupquestionId"] = new SelectList(_context.Supquestions, "Id", "Question", supcomment.SupquestionId);
            return View(supcomment);
        }

        // POST: Admin/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SupquestionId,SubmissionDate,Comment,AdvisorRating")] Supcomment supcomment)
        {
            if (id != supcomment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supcomment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupcommentExists(supcomment.Id))
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
            ViewData["SupquestionId"] = new SelectList(_context.Supquestions, "Id", "Question", supcomment.SupquestionId);
            return View(supcomment);
        }

        // GET: Admin/Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Supcomments == null)
            {
                return NotFound();
            }

            var supcomment = await _context.Supcomments
                .Include(s => s.Supquestion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supcomment == null)
            {
                return NotFound();
            }

            return View(supcomment);
        }

        // POST: Admin/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Supcomments == null)
            {
                return Problem("Entity set 'StandupsDbContext.Supcomments'  is null.");
            }
            var supcomment = await _context.Supcomments.FindAsync(id);
            if (supcomment != null)
            {
                _context.Supcomments.Remove(supcomment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupcommentExists(int id)
        {
          return _context.Supcomments.Any(e => e.Id == id);
        }
    }
}
