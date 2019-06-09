using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Student.Controllers
{
    [Area("Student")]
    public class CoursesController : Controller
    {
        private readonly WebAppContext _context;

        public CoursesController(WebAppContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var courses = _context.Courses;
            return View(await courses.ToListAsync());
        }

        // GET: Teacher/Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .SingleOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Lectures = _context.Lectures.Where(p => p.CourseId == id);
            return View(course);
        }
    }
}
