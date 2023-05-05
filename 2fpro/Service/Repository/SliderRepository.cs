using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class SliderRepository : ISliderRepository
    {
        private ApplicationDbContext db;
        private IHostingEnvironment _env;
        private IFileManager filemanager;

        public SliderRepository(ApplicationDbContext _db, IHostingEnvironment env, IFileManager _filemanager)
        {
            db = _db;
            _env = env;
            filemanager = _filemanager;
        }

        public IQueryable<Portfolio> Sliders { get { return db.Portfolios; } }



        public IEnumerable<Portfolio> getAll()
        {
            return Sliders;
        }
        public async Task<List<Portfolio>> GetAsync()
        {
            return await db.Portfolios.ToListAsync();
        }
        public async Task Create(Portfolio folio = null, IFormFile file = null)
        {

            if (folio != null)
            {
                var rndName = filemanager.GetRandomName();
                var gall = new Portfolio();
                gall.Title = folio.Title;
                gall.Description = folio.Description;
                gall.Price = folio.Price;
                gall.ProdLink = folio.ProdLink;
                gall.VideoLink = folio.VideoLink;
                gall.ImageMimeType = rndName + Path.GetExtension(file.FileName);

                db.Portfolios.Add(gall);
                db.SaveChanges();

                var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + gall.ID.ToString());
                var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + gall.ID.ToString() + "/small");
                filemanager.CheckDirectory(dirPath);
                filemanager.CheckDirectory(dirPaths);


                var filePath = Path.Combine(_env.WebRootPath, dirPath + "/" + rndName + Path.GetExtension(file.FileName));
                var filePaths = Path.Combine(_env.WebRootPath, dirPaths + "/" + rndName + Path.GetExtension(file.FileName));

                await filemanager.WriteImage(file, filePath, false, 0, 0);
                await filemanager.WriteImage(file, filePaths, false, 300, 0);


            }




        }

        public async Task Edit(Portfolio folio = null, IFormFile file = null)
        {

            var fol = await db.Portfolios.SingleOrDefaultAsync(x => x.ID == folio.ID);
            fol.Title = folio.Title;
            fol.Description = folio.Description;
            fol.Price = folio.Price;
            fol.ProdLink = folio.ProdLink;
            fol.VideoLink = folio.VideoLink;
            if (file != null)
            {
                //удаляем папку с фото для изделия
                var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + folio.ID.ToString());
                filemanager.RemoveDir(dirPath);

                //создаем и вносим новое фото в папку для изделия
                var rndName = filemanager.GetRandomName();
                fol.ImageMimeType = rndName + Path.GetExtension(file.FileName);
                await db.SaveChangesAsync();
                dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + folio.ID.ToString());
                var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + fol.ID.ToString() + "/small");
                filemanager.CheckDirectory(dirPath);
                filemanager.CheckDirectory(dirPaths);


                var filePath = Path.Combine(_env.WebRootPath, dirPath + "/" + rndName + Path.GetExtension(file.FileName));
                var filePaths = Path.Combine(_env.WebRootPath, dirPaths + "/" + rndName + Path.GetExtension(file.FileName));

                await filemanager.WriteImage(file, filePath, false, 0, 0);
                await filemanager.WriteImage(file, filePaths, false, 300, 0);

            }
            db.SaveChanges();

        }



        public Portfolio GetPortfolio(int id)
        {
            var folio = db.Portfolios.Find(id);
            return folio;
        }
        public async Task<Portfolio> GetAsync(int id)
        {
            var folio = await db.Portfolios.FindAsync(id);
            return folio;
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public async Task DeleteAll()
        {
            if (await Sliders.AnyAsync())
            {
                int res = await db.Database.ExecuteSqlCommandAsync("Truncate table [Portfolios]");
            }
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Slider");
            filemanager.RemoveDir(dirPath);
        }
        public void Delete(Portfolio folio = null)
        {
            if (folio != null)
            {

                var item = db.Portfolios.SingleOrDefault(x => x.ID == folio.ID);
                var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Slider/" + folio.ID.ToString());

                filemanager.RemoveDir(dirPath);
                db.Portfolios.Remove(folio);
                db.SaveChanges();
            }
        }


        public async Task UpdateSort(int id, int newSort)
        {
            Portfolio image = await GetAsync(id);

            image.Sortindex = newSort;

            db.SaveChanges();
        }

    }
}