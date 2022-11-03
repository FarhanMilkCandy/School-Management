using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace SMS.Controllers
{
    public class ResultsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public ResultsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //[Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var studentResult = await _context.Enrollments.Where(e => e.StudentId == _userManager.GetUserId(User)).Join
                (_context.Courses,
                Enrollments=> Enrollments.CoursesId,
                Courses=> Courses.CourseId,
                (Enrollment, Course) => new {Enrollment, Course})
                .Select(s=> new ResultViewModel
                {
                    CourseName = s.Course.CourseName,
                    Grade = s.Enrollment.Grade.Value.ToString() ?? "Note Yet Graded"
                }).ToListAsync();         
            ViewBag.results = studentResult;
            return View();
        }

        public class ResultViewModel
        {
            public string CourseName { get; set; }
            public string? Grade { get; set; }
        }
    }
}
