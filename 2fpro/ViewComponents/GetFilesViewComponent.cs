using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewComponents
{
    public class GetFilesViewComponent : ViewComponent
    {

        private IHostingEnvironment _env;
        private IFileManager _manager;

        public GetFilesViewComponent(IHostingEnvironment env, IFileManager manager)
        {
            _env = env;
            _manager = manager;
        }
        private int FilesPerPage = 15;
        public async Task<IViewComponentResult> InvokeAsync(int page = 1, string folder = null)
        {
            var defaultDir = Path.Combine(_env.WebRootPath, "Content/Files/Pages");
            _manager.CheckDirectory(defaultDir);

            var dirp = Path.Combine(_env.WebRootPath, "Content/Files/Pages/" + folder);
            DirectoryInfo directory = new DirectoryInfo(dirp);
            DirectoryInfo maindir = new DirectoryInfo(defaultDir);
            var firstdir = maindir.GetDirectories().FirstOrDefault();
            if (maindir.GetDirectories().Count() == 0) directory = new DirectoryInfo(defaultDir); //проверка что основная папка пустая
            else if (folder == null && maindir.GetDirectories().Count() > 0) directory = firstdir;// выбор первой папки при открытии страницы с нулевым параметром dir
            else directory = new DirectoryInfo(dirp);// открытие заданной параметром dir папки 

            var allFiles = directory.GetFiles().Count();
            var files = directory.GetFiles().Skip(FilesPerPage * (page - 1)).Take(FilesPerPage).ToList();


            FilesViewModel vm = new FilesViewModel
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = allFiles,
                    CurrentPage = page,
                    ItemsPerPage = FilesPerPage,
                    Service = "files",
                    Dir = directory,
                    Condition = true

                },
                Files = files

            };

            return View(vm);
        }

    }
}
