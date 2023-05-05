using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class MainsliderViewComponent : ViewComponent
    {
        ISliderRepository _crep;


        public MainsliderViewComponent(ISliderRepository crep)
        {
            _crep = crep;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _crep.GetAsync();
            return View(list);
        }
    }
}
