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
    public class AnswersController : Controller
    {
        private readonly WebAppContext _context;

        public AnswersController(WebAppContext context)
        {
            _context = context;
        }

        // GET: Teacher/Answers
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
                return NotFound();
            ViewBag.Id = id;
            IQueryable<Answer> answers = _context.Answers.Include(u => u.Question).Where(u => u.Question.Id == id);
            return View(await answers.ToListAsync());
        }

        // GET: Teacher/Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.Include(p => p.Test).FirstOrDefaultAsync(u => u.Id == answer.QuestionId);
            ViewBag.question = question;
            return View(answer);
        }

        // GET: Teacher/Answers/Create
        public async Task<IActionResult> Create(int? id)
        {
            ViewBag.RouteId = id;
            var question = await _context.Questions.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            var questions = new List<Question> { new Question { Id = question.Id, TestId = question.TestId, Description = question.Description } };
            ViewData["QuestionId"] = new SelectList(questions, "Id", "Description");
            return View();
        }

        // POST: Teacher/Answers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,Rightness,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Description", answer.QuestionId);
            return View(answer);
        }

        // GET: Teacher/Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.Include(p=> p.Test).FirstOrDefaultAsync(u => u.Id == answer.QuestionId);
            ViewBag.question = question;

            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Description", answer.QuestionId);
            return View(answer);
        }

        // POST: Teacher/Answers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Rightness,QuestionId")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details","Questions", new { id = answer.QuestionId });
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Description", answer.QuestionId);
            return View(answer);
        }

        // GET: Teacher/Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answers
                .Include(a => a.Question)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }
            var question = await _context.Questions.Include(p => p.Test).FirstOrDefaultAsync(u => u.Id == answer.QuestionId);
            ViewBag.question = question;
            return View(answer);
        }

        // POST: Teacher/Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(m => m.Id == id);
            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.Id == id);
        }
    }
}
