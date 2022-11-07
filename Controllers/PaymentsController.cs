using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Enums;
using SMS.Models;

namespace SMS.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Student"))
            {
                ViewBag.Payments = await _context.Payments.Include(p => p.Enrollments).Include(p => p.Fees).Include(p => p.Payee).Where(w => w.Enrollments.StudentId.Equals(_userManager.GetUserId(User)) && w.Payee.Id.Equals(_userManager.GetUserId(User))).Join(_context.Courses,
                    Payment => Payment.Enrollments.CoursesId,
                    Courses => Courses.CourseId,
                    (Payment, Courses) => new { Payment, Courses })
                        .Select(s => new PaymentViewModel
                        {
                            PaymentDate = s.Payment.PaymentDate.ToString("MMMM dd, yyyy"),
                            CourseName = s.Courses.CourseName,
                            FeeAmount = s.Payment.Fees.FeeAmount.ToString()
                        }).ToListAsync();
                return View();
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.Payments = await _context.Payments.Include(p => p.Enrollments).Include(p => p.Fees).Include(p => p.Payee).Join(_context.Courses,
                    Payment => Payment.Enrollments.CoursesId,
                    Courses => Courses.CourseId,
                    (Payment, Courses) => new { Payment, Courses })
                        .Select(s => new PaymentViewModel
                        {
                            PaymentDate = s.Payment.PaymentDate.ToString("MMMM dd, yyyy"),
                            CourseName = s.Courses.CourseName,
                            FeeAmount = s.Payment.Fees.FeeAmount.ToString()
                        }).ToListAsync();
                return View();
            }
            else
            {
                return NotFound();
            }

        }

        [Authorize(Roles = "None")] 
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

        [Authorize(Roles = "Student")]
        // GET: Payments/Create
        public async Task<IActionResult> Create()
        {
            var stdPayRecord = _context.Payments.Where(w => w.PayeeId.Equals(_userManager.GetUserId(User))).Select(s => s.EnrollmentsId).ToList();
            var stdEnrollments = await _context.Enrollments.Where(w => !stdPayRecord.Contains(w.EnrollmentId) && w.StudentId.Equals(_userManager.GetUserId(User)) && w.Grade == null).Join(_context.Courses,
                Enrollments => Enrollments.CoursesId,
                Courses => Courses.CourseId,
                (Enrollments, Courses) => new { Enrollments, Courses }).ToListAsync();
            ViewData["EnrollmentsId"] = new SelectList(stdEnrollments, "Enrollments.EnrollmentId", "Courses.CourseName");
            ViewData["PayeeId"] = _userManager.GetUserId(User);
            ViewData["PayeeName"] = _userManager.GetUserName(User);
            ViewData["TodayDate"] = DateOnly.Parse(DateTime.Today.ToShortDateString());
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,PaymentDate,EnrollmentsId,FeesId,PayeeId")] Payment payment)
        {
            payment.PaymentDate = DateOnly.Parse(DateTime.Today.ToString("MMMM dd, yyyy"));
            var course = await _context.Enrollments.Where(w => w.EnrollmentId.Equals(payment.EnrollmentsId)).Select(s => s.CoursesId).FirstOrDefaultAsync();
            payment.FeesId = await _context.Fees.Where(w => w.CoursesId.Equals(course)).Select(s => s.FeeId).FirstOrDefaultAsync();
            //payment.PayeeId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var stdPayRecord = _context.Payments.Where(w => w.PayeeId.Equals(_userManager.GetUserId(User))).Select(s => s.EnrollmentsId).ToList();
            var stdEnrollments = await _context.Enrollments.Where(w => !stdPayRecord.Contains(w.EnrollmentId) && w.StudentId.Equals(_userManager.GetUserId(User)) && w.Grade == null).Join(_context.Courses,
                Enrollments => Enrollments.CoursesId,
                Courses => Courses.CourseId,
                (Enrollments, Courses) => new { Enrollments, Courses }).ToListAsync();
            ViewData["EnrollmentsId"] = new SelectList(stdEnrollments, "Enrollments.EnrollmentId", "Courses.CourseName");
            return View(payment);
        }

        [Authorize(Roles = "None")]
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
        [Authorize(Roles = "None")]
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

        [Authorize(Roles = "None")]
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

        [Authorize(Roles = "None")]
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

        public class PaymentViewModel
        {
            public string PaymentDate { get; set; }
            public string CourseName { get; set; }
            public string FeeAmount { get; set; }
        }
    }
}
