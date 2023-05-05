using _2fpro.Models;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class DiagnosticInfoViewComponent : ViewComponent
    {
        IDiagnosticService _diag;
        IDistributedCache _cache;

        public DiagnosticInfoViewComponent(IDiagnosticService diag, IDistributedCache cache)
        {
            _diag = diag;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            // кол во пользователей за день
            var listPerDay = await _diag.UsersPerDaySQL();
            var listPerWeek = await _diag.UsersPerDaySQL(7);
            var listPerMounth = await _diag.UsersPerDaySQL(30);
            ViewData["dcount"] = listPerDay.Count();
            ViewData["wcount"] = listPerWeek.Count();
            ViewData["mcount"] = listPerMounth.Count();

            List<Diagnostic> list = new List<Diagnostic>();
            if (User.IsInRole("Admin")) list = await _diag.GetAllOnlineAsync();
            return View(list);
        }

    }
}
