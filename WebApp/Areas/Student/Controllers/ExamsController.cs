using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Areas.Student.Controllers
{
    [Area("Teacher")]
    public class ExamsController : Controller
    {
        private readonly WebAppContext _context;

        public ExamsController(WebAppContext context)
        {
            _context = context;
        }
        //GET: Student/Exams/Pass/5
        public async Task<IActionResult> Pass(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var test = await _context.Tests
                .Include(t => t.Course)
                .SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.Questions = _context.Questions.Where(p => p.TestId == id);
            ViewBag.Answers = _context.Answers.Where(u => u.Question.TestId == id);
            return View(test);
        }

    }
}
