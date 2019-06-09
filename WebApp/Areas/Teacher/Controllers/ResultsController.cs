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
    public class ResultsController : Controller
    {
        private readonly WebAppContext _context;

        public ResultsController(WebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var groups = _context.Groups;
            return View(await groups.ToListAsync());
        }

        // GET: Admin/Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _context.Users.Where(u => u.GroupId == id);
            ViewBag.Group = await _context.Groups.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(await user.ToListAsync());
        }

        public async Task<IActionResult> UserDetails(int? id)
        {
            var user = _context.Users.Include(u => u.Group).SingleOrDefault(m => m.Id == id);
            var tests = _context.Tests;
            var usersResult = _context.Results.Include(u => u.Test).ThenInclude(u => u.Course).Where(u => u.UserId == id);
            ViewBag.Questions = _context.Questions;
            ViewBag.Group = await _context.Groups.FirstOrDefaultAsync(u => u.Id == user.GroupId);
            return View(await usersResult.ToListAsync());
        }
    }
}