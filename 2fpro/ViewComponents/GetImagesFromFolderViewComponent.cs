using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetImagesFromFolderViewComponent : ViewComponent
    {
        private FileManager filemanager = new FileManager();
        private IHostingEnvironment _prodCtx;

        public GetImagesFromFolderViewComponent(IHostingEnvironment prodCtx)
        {
            _prodCtx = prodCtx;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var dirPath = Path.Combine(_prodCtx.WebRootPath, "Content/Files/Pages/примеры");
            if (Directory.Exists(dirPath))
            {
                var list = Directory.GetFiles(dirPath);
                ViewBag.fileList = list;
            }
            else { ViewBag.fileList =new string[0]; }
            
            return View();
        }
    }
}
