using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Models;

namespace SMS.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Payments.Include(p => p.Enrollments).Include(p => p.Fees).Include(p => p.Payee);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Enrollments)
                .Include(p => p.Fees)
                .Include(p => p.Payee)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["EnrollmentsId"] = new SelectList(_context.Enrollments, "EnrollmentId", "EnrollmentId");
            ViewData["FeesId"] = new SelectList(_context.Fees, "FeeId", "FeeId");
            ViewData["PayeeId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,PaymentDate,EnrollmentsId,FeesId,PayeeId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EnrollmentsId"] = new SelectList(_context.Enrollments, "EnrollmentId", "EnrollmentId", payment.EnrollmentsId);
            ViewData["FeesId"] = new SelectList(_context.Fees, "FeeId", "FeeId", payment.FeesId);
            ViewData["PayeeId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", payment.PayeeId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["EnrollmentsId"] = new SelectList(_context.Enrollments, "EnrollmentId", "EnrollmentId", payment.EnrollmentsId);
            ViewData["FeesId"] = new SelectList(_context.Fees, "FeeId", "FeeId", payment.FeesId);
            ViewData["PayeeId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", payment.PayeeId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,PaymentDate,EnrollmentsId,FeesId,PayeeId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["EnrollmentsId"] = new SelectList(_context.Enrollments, "EnrollmentId", "EnrollmentId", payment.EnrollmentsId);
            ViewData["FeesId"] = new SelectList(_context.Fees, "FeeId", "FeeId", payment.FeesId);
            ViewData["PayeeId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", payment.PayeeId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Payments == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Enrollments)
                .Include(p => p.Fees)
                .Include(p => p.Payee)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Payments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Payments'  is null.");
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
          return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
