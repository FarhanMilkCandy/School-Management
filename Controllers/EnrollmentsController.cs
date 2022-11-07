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
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public EnrollmentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin, Student")]
        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var applicationDbContext = _context.Enrollments.Include(e => e.Courses).Include(e => e.Student); //Linq
                return View(await applicationDbContext.ToListAsync());
            }
            else if (User.IsInRole("Student"))
            {
                var applicationDbContext = _context.Enrollments.Where(w => w.Grade == null).Include(e => e.Courses).Include(e => e.Student);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                return NotFound();
            }

        }

        [Authorize(Roles = "Admin")]
        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Courses)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        [Authorize(Roles = "Admin, Student")]
        // GET: Enrollments/Create
        public IActionResult Create()
        {
            if (User.IsInRole("Student"))
            {
                var studentEnrollments = _context.Enrollments.Where(w => w.StudentId.Equals(_userManager.GetUserId(User))).Select(s => s.CoursesId).ToList();
                var enrollableCourses = _context.Courses.Where(w => !studentEnrollments.Contains(w.CourseId) && w.IsOffered == true).ToList();
                ViewData["CoursesId"] = new SelectList(enrollableCourses, "CourseId", "CourseName");
            }
            else
            {
                var offerableCourses = _context.Courses.Include(c => c.Fees).Where(w => w.IsOffered == true && w.Fees.CoursesId == w.CourseId).ToList();
                ViewData["CoursesId"] = new SelectList(offerableCourses, "CourseId", "CourseName");
                var nonAdmin = _context.ApplicationUsers.Join(_context.UserRoles,
                    User => User.Id,
                    roles => roles.UserId,
                    (User, roles) => new { User, roles }).Join(_context.Roles,
                    userroles => userroles.roles.RoleId,
                    roles => roles.Id,
                    (userroles, roles) => new { userroles, roles }).Where(w => !w.roles.NormalizedName.Equals("Admin")).Select(s => new { s.userroles.User.Id, s.userroles.User.UserName }).ToList();
                ViewData["StudentId"] = new SelectList(nonAdmin, "Id", "UserName");
            }
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentId,EnrollmentDate,CoursesId,StudentId,Grade")] Enrollment enrollment)
        {
            enrollment.EnrollmentDate = DateOnly.Parse(DateTime.Now.ToShortDateString());
            if (User.IsInRole("Student"))
            {
                enrollment.StudentId = _userManager.GetUserId(User);
            }
            if (!EligibleCheck(enrollment.StudentId, enrollment.CoursesId))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(enrollment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CoursesId"] = new SelectList(_context.Courses, "CourseId", "CourseName", enrollment.CoursesId);
                if (User.IsInRole("Admin"))
                {
                    var nonAdmin = _context.ApplicationUsers.Join(_context.UserRoles,
                    User => User.Id,
                    roles => roles.UserId,
                    (User, roles) => new { User, roles }).Join(_context.Roles,
                    userroles => userroles.roles.RoleId,
                    roles => roles.Id,
                    (userroles, roles) => new { userroles, roles }).Where(w => !w.roles.NormalizedName.Equals("Admin")).Select(s => new { s.userroles.User.Id, s.userroles.User.UserName }).ToList();
                    ViewData["StudentId"] = new SelectList(nonAdmin, "Id", "UserName", enrollment.StudentId);
                }
                else
                {
                    ViewData["StudentId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", enrollment.StudentId);
                }
                TempData["ErrorMessage"] = null;
                return View(enrollment);
            }
            else
            {
                if (User.IsInRole("Admin"))
                {
                    var nonAdmin = _context.ApplicationUsers.Join(_context.UserRoles,
                    User => User.Id,
                    roles => roles.UserId,
                    (User, roles) => new { User, roles }).Join(_context.Roles,
                    userroles => userroles.roles.RoleId,
                    roles => roles.Id,
                    (userroles, roles) => new { userroles, roles }).Where(w => !w.roles.NormalizedName.Equals("Admin")).Select(s => new { s.userroles.User.Id, s.userroles.User.UserName }).ToList();
                    ViewData["StudentId"] = new SelectList(nonAdmin, "Id", "UserName", enrollment.StudentId);
                }
                else
                {
                    ViewData["StudentId"] = new SelectList(_context.ApplicationUsers, "Id", "UserName", enrollment.StudentId);
                }
                TempData["ErrorMessage"] = "Student already enrolled in the same course!";
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }
            var enrollments = await _context.Enrollments.Join(_context.Payments,
                Enrollment => Enrollment.EnrollmentId,
                Payment => Payment.EnrollmentsId,
                (Enrollment, Payment) => new { Enrollment, Payment }).Select(s=> s.Enrollment).FirstOrDefaultAsync(f=> f.EnrollmentId == id);
           
            if (enrollments != null)
            {
                var enrollment = await _context.Enrollments.FindAsync(id);
                if (enrollment == null)
                {
                    return NotFound();
                }
                ViewData["CoursesId"] = enrollment.CoursesId;
                ViewData["StudentId"] = enrollment.StudentId;
                return View(enrollment);
            }
            else
            {
                TempData["ErrorMessage"] = "Student did not pay for the course!";
            }
            return View();
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollmentId,EnrollmentDate,CoursesId,StudentId,Grade")] Enrollment enrollment)
        {
                if (id != enrollment.EnrollmentId)
                {
                    if (enrollment.EnrollmentId==0)
                    {
                        TempData["ErrorMessage"] = "Student did not pay for the course!";
                        return View(enrollment);
                    }
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(enrollment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EnrollmentExists(enrollment.EnrollmentId))
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
                ViewData["CoursesId"] = enrollment.CoursesId;
                ViewData["StudentId"] = enrollment.StudentId;
            return View(enrollment);
        }

        [Authorize(Roles = "Admin")]
        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enrollments == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollments
                .Include(e => e.Courses)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.EnrollmentId == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        [Authorize(Roles = "Admin")]
        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enrollments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Enrollments'  is null.");
            }
            var enrollment = await _context.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _context.Enrollments.Remove(enrollment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentId == id);
        }

        private bool EligibleCheck(string id, int courseid)
        {
            var studentEnrollments = _context.Enrollments.Where(w => w.StudentId.Equals(id)).Select(s => s.CoursesId).ToList();
            var enrollableCourses = _context.Courses.Where(w => !studentEnrollments.Contains(w.CourseId)).ToList();
            ViewData["CoursesId"] = new SelectList(enrollableCourses, "CourseId", "CourseName");
            return studentEnrollments.Contains(courseid);
        }
    }
}
