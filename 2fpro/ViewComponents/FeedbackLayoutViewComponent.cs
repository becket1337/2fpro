using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class FeedbackLayoutViewComponent : ViewComponent
    {

        public FeedbackLayoutViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            return View();
        }
    }
}
