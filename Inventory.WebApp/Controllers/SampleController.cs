using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebApp.Controllers
{
    public class BaaaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Async
        // public async Task<IActionResult> Index()
        // {
        //     return View(await _context.Posts.ToListAsync());
        // }
    }
}