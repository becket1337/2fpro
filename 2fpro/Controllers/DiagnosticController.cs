using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Models;
using _2fpro.Service.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace _2fpro.Controllers
{

    public class DiagnosticController : Controller
    {
        IDiagnosticService _diagService;
        public DiagnosticController(IDiagnosticService diagService)
        {
            _diagService = diagService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetDiagWindow()
        {
            return ViewComponent("DiagnosticInfo");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetUserInfo(string data)
        {
            var parsedData = JObject.Parse(data);
            string username = (string)parsedData["uname"];
            var diag = await _diagService.GetAsync(username);
            return PartialView(diag);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> GetUsersList()
        {
            return PartialView(await _diagService.UsersPerDaySQL());
        }

        public class CorrectDiag
        {
            public string Name { get; set; }
        }
    }
}