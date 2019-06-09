using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Areas.Student.Controllers
{
    [Area("Student")]
    public class LecturesController : Controller
    {
        private readonly WebAppContext _context;

        public LecturesController(WebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lecture = await _context.Lectures
                .Include(t => t.Course)
                .SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.Lectures = lecture;

            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var lectures =_context.Lectures.Include(u => u.Course).Where(u => u.CourseId == id);
            ViewBag.Lectures = lectures;

            if (lectures == null)
            {
                return NotFound();
            }

            return View(await lectures.ToListAsync());
        }
    }
}