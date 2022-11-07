using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SMS.Data;
using SMS.Enums;
using SMS.Models;

namespace SMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        // GET: Fees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Fees.Include(f => f.Courses);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        // GET: Fees/Create
        public IActionResult Create()
        {
            var fees = _context.Fees.Select(s=> s.CoursesId).ToList();
            var coursesWithoutFee = _context.Courses.Where(w => !fees.Contains(w.CourseId)).ToList();
            ViewBag.CourseId = new SelectList(coursesWithoutFee, "CourseId", "CourseName");
            return View();
        }

        // POST: Fees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeeId,FeeAmount,FeeDescription,CoursesId")] Fees fees)
        {
            var feesList = _context.Fees.Select(s => s.CoursesId).ToList();
            var coursesWithoutFee = _context.Courses.Where(w => !feesList.Contains(w.CourseId)).ToList();
            if (ModelState.IsValid)
            {
                _context.Add(fees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CourseId = new SelectList(coursesWithoutFee, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        [Authorize(Roles = "Admin")]
        // GET: Fees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var fee = _context.Fees.Select(s => s.CoursesId).ToList();
            var coursesWithoutFee = _context.Courses.Where(w => !fee.Contains(w.CourseId)).ToList();
            if (id == null || _context.Fees == null)
            {
                return NotFound();
            }

            var fees = await _context.Fees.FindAsync(id);
            if (fees == null)
            {
                return NotFound();
            }
            ViewBag.CourseId = new SelectList(coursesWithoutFee, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        // POST: Fees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FeeId,FeeAmount,FeeDescription,CoursesId")] Fees fees)
        {
            var fee = _context.Fees.Select(s => s.CoursesId).ToList();
            var coursesWithoutFee = _context.Courses.Where(w => !fee.Contains(w.CourseId)).ToList();
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
            ViewBag.CourseId = new SelectList(coursesWithoutFee, "CourseId", "CourseName", fees.CoursesId);
            return View(fees);
        }

        [Authorize(Roles = "Admin")]
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
