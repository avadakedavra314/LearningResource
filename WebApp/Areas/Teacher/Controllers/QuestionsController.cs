using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    public class QuestionsController : Controller
    {
        private readonly WebAppContext _context;

        public QuestionsController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Teacher/Questions
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();
            ViewBag.Id = id;
            IQueryable<Question> questions = _context.Questions.Include(u => u.Test).Where(u => u.Test.Id == id);
            return View(await questions.ToListAsync());
        }

        // GET: Teacher/Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Test)
                .SingleOrDefaultAsync(m => m.Id == id);
            ViewBag.Answers = _context.Answers.Where(p => p.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Teacher/Questions/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.RouteId = id;
            var test = await _context.Tests.SingleOrDefaultAsync(m => m.Id == id);
            var tests = new List<Test> { new Test { Id = test.Id, CourseId = test.CourseId, Name = test.Name } };
            ViewData["TestId"] = new SelectList(tests, "Id", "Name");
            ViewBag.test = test;
            return View();
        }

        // POST: Teacher/Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,TestId")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
               return RedirectToAction("Details","Tests", new { id = question.TestId });
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Name", question.TestId);
            return View(question);
        }

        // GET: Teacher/Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.Include(u =>u.Test).SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Name", question.TestId);
            return View(question);
        }

        // POST: Teacher/Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,TestId")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Tests", new { id = question.TestId });
            }
            ViewData["TestId"] = new SelectList(_context.Tests, "Id", "Name", question.TestId);
            return View(question);
        }

        // GET: Teacher/Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Test)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Teacher/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
           return RedirectToAction("Details","Tests", new { id = question.TestId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
