using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View(new ErrorViewModel
            { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}