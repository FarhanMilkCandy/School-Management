using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SMS.Data;
using SMS.Models;

namespace SMS.Controllers
{
    public class FeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fees.Include(f => f.Courses);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Fees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fees = await _context.Fees
                .Include(f => f.Courses)
                .FirstOrDefaultAsync(m => m.FeeId == id);
            if (fees == null)
            {
                return NotFound();
            }

            return View(fees);
        }

        // GET: Fees/Create
        public IActionResult Create()
        {
            ViewData["CoursesId"] = new SelectList(_context.Courses, "CourseId", "CourseName");
            return View();
        }

        // POST: Fees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeeId,FeeAmount,FeeDescription,CoursesId")] Fees fees)
        {
            //ModelState.SetModelValue( this.ModelState.Keys.Where(w=> w.ToString().Equals("Courses")).FirstOrDefault(), _context.Courses.Where(w => w.CourseId == fees.CoursesId).FirstOrDefault() ?? new Course(), "");
            var errors = ModelState.Keys
                   .Where(k => ModelState[k].Errors.Count > 0)
                   .Select(k => new
                   {
                       propertyName = k,
                       errorMessage = ModelState[k].Errors[0].ErrorMessage
                   }).ToList();
            if (ModelState.IsValid)
            {
                _context.Add(fees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CoursesId"] = new SelectList(_context.Courses, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        // GET: Fees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fees = await _context.Fees.FindAsync(id);
            if (fees == null)
            {
                return NotFound();
            }
            ViewData["CoursesId"] = new SelectList(_context.Courses, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        // POST: Fees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeeId,FeeAmount,FeeDescription,CoursesId")] Fees fees)
        {
            if (id != fees.FeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeesExists(fees.FeeId))
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
            ViewData["CoursesId"] = new SelectList(_context.Courses, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        // GET: Fees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fees = await _context.Fees
                .Include(f => f.Courses)
                .FirstOrDefaultAsync(m => m.FeeId == id);
            if (fees == null)
            {
                return NotFound();
            }

            return View(fees);
        }

        // POST: Fees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fees'  is null.");
            }
            var fees = await _context.Fees.FindAsync(id);
            if (fees != null)
            {
                _context.Fees.Remove(fees);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeesExists(int id)
        {
          return _context.Fees.Any(e => e.FeeId == id);
        }
    }
}
