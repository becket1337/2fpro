using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _2fpro.Pages
{
    public class robotsTxtModel : PageModel
    {
        IConfigRepository _confRep;

        public robotsTxtModel(IConfigRepository conRep)
        {
            _confRep = conRep;
        }
        public string robotsContent;
        public void OnGet()
        {
            robotsContent = _confRep.Configs.SingleOrDefault().Robots;

        }
    }
}