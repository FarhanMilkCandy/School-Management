using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Data;
using SMS.Enums;
using SMS.Models;

namespace SMS.Controllers
{
    public class OfferedCoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OfferedCoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<OfferedCoursesViewModel> offeredCourses = new List<OfferedCoursesViewModel>();
            if (User.IsInRole("Student"))
            {
                var courseList = _context.Courses.Where(w => w.IsOffered == true).Join(_context.Fees,
                    Courses=> Courses.CourseId,
                    Fees=> Fees.FeeId,
                    (Courses, Fees) => new { Courses, Fees }).ToList();
                var enrolledCourse = _context.Enrollments.Where(w => w.StudentId == _userManager.GetUserId(User)).Select(s=> s.CoursesId).ToList();
                foreach (var course in courseList)
                {
                    OfferedCoursesViewModel offeredCourse = new OfferedCoursesViewModel();
                    offeredCourse.CourseName = course.Courses.CourseName;
                    offeredCourse.CourseType = course.Courses.CourseType;
                    offeredCourse.CourseDescription = course.Courses.CourseDescription ?? "No Description Available";
                    offeredCourse.CourseFee = course.Fees.FeeAmount.ToString();
                    offeredCourse.IsEnrolled = (enrolledCourse.Contains(course.Courses.CourseId)) ? true : false;
                    offeredCourses.Add(offeredCourse);
                }
            }
            else
            {
                var courseList = _context.Courses.Where(w => w.IsOffered == true).Join(_context.Fees,
                    Courses => Courses.CourseId,
                    Fees => Fees.FeeId,
                    (Courses, Fees) => new { Courses, Fees }).ToList();              
                foreach (var course in courseList)
                {
                    OfferedCoursesViewModel offeredCourse = new OfferedCoursesViewModel();
                    offeredCourse.CourseName = course.Courses.CourseName;
                    offeredCourse.CourseType = course.Courses.CourseType;
                    offeredCourse.CourseDescription = course.Courses.CourseDescription ?? "No Description Available";
                    offeredCourse.CourseFee = course.Fees.FeeAmount.ToString();
                    offeredCourse.IsEnrolled = false;
                    offeredCourses.Add(offeredCourse);
                }
            }
            ViewBag.OfferdCourses = offeredCourses;
            return View();
        }

        public class OfferedCoursesViewModel
        {
            public string CourseName { get; set; }
            public CourseTypes CourseType { get; set; }
            public string CourseDescription { get; set; }
            public string CourseFee { get; set; }
            public bool IsEnrolled { get; set; }
        }
    }
}
