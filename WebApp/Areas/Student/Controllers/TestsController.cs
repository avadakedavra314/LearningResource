using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Student.Controllers
{
    [Area("Student")]
    public class TestsController : Controller
    {
        private readonly WebAppContext _context;

        public TestsController(WebAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
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
            ViewBag.Tests = _context.Tests.Where(p => p.CourseId == id);
            return View(course);
        }
        [HttpGet("{id:int}/{q:int}/{a:int}")]
        public async Task<IActionResult> Pass(int? id, int q, int a)
        {
            if (id == null)
            {
                return NotFound();
            }
            var test = await _context.Tests
                .Include(t => t.Course)
                .SingleOrDefaultAsync(m => m.Id == id);
            // Getting test result of following user
            IEnumerable<Result> results = _context.Results.Where(u => u.TestId == test.Id);
            var questions = _context.Questions.Where(p => p.TestId == id);

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
            var usersResult = results.Where(u => u.UserId == user.Id).FirstOrDefault();

            if(usersResult == null)
            {
                usersResult = new Result { Score = 0, TestId = test.Id, UserId = user.Id, Finished = false };
                _context.Results.Add(usersResult);
            }

            _context.SaveChanges();

            if (usersResult.Finished == true)
            {
                return RedirectToAction("Index", "Tests", new { id = test.CourseId });
            }

            var qArray = questions.ToArray();
            var selectedQuestion = qArray[q];
            ViewBag.Question = selectedQuestion;
            ViewBag.NextQuestionId = q;

            var answers = _context.Answers.Where(u => u.QuestionId == selectedQuestion.Id);
            ViewBag.Answers = answers;
            ViewBag.Course = _context.Courses.Where(u => u.Id == test.CourseId).SingleOrDefault();

            // Writing answer for user
            if (a != 100)
            {
                int nextQuestionIndex = q + 1;
                var answerFromUser = answers.FirstOrDefault(u => u.Id == a);
                if (answerFromUser.Rightness == true)
                {
                    usersResult.Score++;
                }
                _context.SaveChanges();

                if (nextQuestionIndex == qArray.Length)
                {
                    usersResult.Finished = true;
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Tests", new { id = test.CourseId });
                }

                // Preparing next question
                ViewBag.NextQuestionId = nextQuestionIndex;

                ViewBag.Question = qArray[nextQuestionIndex];
                
                // Getting ANSWERS collection for next question
                ViewBag.Answers = _context.Answers.Where(u => u.QuestionId == qArray[nextQuestionIndex].Id);
            }

            return View(test);
        }
    }
}
