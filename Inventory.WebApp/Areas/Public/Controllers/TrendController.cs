using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Csi.WebApp.Areas.Research
{
    [Area("Research")]
    public class TrendController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Trend - Research";
            return View();
        }

        
    }
}