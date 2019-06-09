using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Student.Controllers
{
    [Area("Student")]
    public class ResultsController : Controller
    {
        private readonly WebAppContext _context;

        public ResultsController(WebAppContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tests = _context.Tests;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Login == User.Identity.Name);
            var usersResult = _context.Results.Include(u => u.Test).ThenInclude(u => u.Course).Where(u => u.UserId == user.Id);

            ViewBag.Questions = _context.Questions;
            return View(await usersResult.ToListAsync());
        }
    }
}