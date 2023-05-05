using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetFoldersViewComponent : ViewComponent
    {
        private IFileManager _manager;

        public GetFoldersViewComponent(IFileManager manager)
        {
            _manager = manager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string folder = null)
        {

            FilesViewModel vm = new FilesViewModel
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = 0,
                    DirName = folder
                },
                Dirs = _manager.GetDirs()

            };

            return View(vm);
        }
    }
}
