using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext _db;
        private IFileManager filemanager;
        private IHostingEnvironment _env;

        public CategoryRepository(ApplicationDbContext db, IHostingEnvironment env, IFileManager _filemanager)
        {
            _db = db;
            _env = env;
            filemanager = _filemanager;
        }

        public IQueryable<Category> Categories { get { return _db.Categories; } }


        public async Task Create(CategoryViewModel post)
        {
            Category cat = new Category();

            cat.CatDescription = post.CatDescription;
            cat.CategoryName = post.CategoryName;
            cat.ParentCategoryId = post.ParentCategoryId ?? 0;
            var parentCat = await GetAsync(cat.ParentCategoryId);
            cat.ParentCatname = parentCat == null ? "" : parentCat.CategoryName;
            _db.Categories.Add(cat);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Product>> GetForSlider()
        {
            return await _db.Products.Include("ProdImages").OrderByDescending(x => x.ModifiedDate).Take(15).ToListAsync();
        }
        public Category Get(int id)
        {
            var item = _db.Categories.Find(id);
            return item;
        }

        public async Task newCat(string catname)
        {
            await _db.Categories.Where(x => x.ParentCategoryId == 0).ForEachAsync(x => _db.Categories.Add(new Category { CategoryName = catname + " " + x.CategoryName, ParentCategoryId = x.ID, ParentCatname = x.CategoryName }));
            await _db.SaveChangesAsync();
        }
        public async Task<Category> GetAsync(int id)
        {
            return await _db.Categories.Include("Products").FirstOrDefaultAsync(x => x.ID == id);
        }
        public IQueryable<Category> Get()
        {
            return from obj in _db.Categories.Include("Products") select obj;
        }
        public async Task<List<Category>> GetAsync()
        {
            return await Categories.Include("Products").ToListAsync();
        }

        public async Task<List<Category>> getOnlyCats()
        {
            return await _db.Categories.ToListAsync();
        }
        public async Task<List<Category>> GetFilteredAsync(int[] models, int[] parts)
        {
            var list = await GetAsync();
            var result = new List<Category>();

            if (parts.Length == 0)
            {
                result = list.Where(x => models.Contains(x.ParentCategoryId)).ToList();

            }
            else result = list.Where(x => parts.Contains(x.ID)).ToList();



            return result;
        }
        public async Task<List<Category>> GetShowed()
        {
            return await Categories.Include("Products").Where(x => !x.DoShow && x.Products.Where(y => y.ToYaMarket).Count() > 0).ToListAsync();
        }
        public async Task Edit(CategoryViewModel post)
        {

            var cat = Get(post.ID);
            cat.CatType = post.CatType;
            cat.CatDescription = post.CatDescription;
            cat.CategoryName = post.CategoryName;
            cat.DoShow = post.DoShow;
            cat.ParentCategoryId = post.ParentCategoryId ?? 0;
            var parentCat = await GetAsync(cat.ParentCategoryId);
            cat.ParentCatname = parentCat == null ? "" : parentCat.CategoryName;
            _db.Entry(cat).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }
        public void Delete(Category post)
        {
            string dirPath = "";
            if (Categories.Include("Products").FirstOrDefault(x => x.ID == post.ID).Products.Any())
            {
                foreach(var item in _db.Products.Where(x => x.CategoryID == post.ID))
                {
                    dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + item.ID.ToString());

                    filemanager.RemoveDir(dirPath);
                }
                _db.Products.RemoveRange(_db.Products.Where(x => x.CategoryID == post.ID));

            }
           dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Category/" + post.ID.ToString());

            filemanager.RemoveDir(dirPath);
            _db.Categories.Remove(post);
            _db.SaveChanges();
        }
        public void PhotoDel(Category cat)
        {
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Category/" + cat.ID.ToString());

            filemanager.RemoveDir(dirPath);
            cat.ImageMimeType = "";
            _db.Entry(cat).State = EntityState.Modified;
            _db.SaveChanges();

        }
        public async Task SavePhoto(IFormFile file, int pid)
        {

            //if (file.Length > 6000000) throw new IndexOutOfRangeException();

            var rndName = filemanager.GetRandomName();


            var cat = Get(pid);
            cat.ImageMimeType = rndName + Path.GetExtension(file.FileName);

            await _db.SaveChangesAsync();

            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Category/" + pid.ToString());
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Category/" + pid.ToString() + "/200x150");
            var dirPath500 = Path.Combine(_env.WebRootPath, "Content/Files/Category/" + pid.ToString() + "/height500");

            if (!Directory.Exists(dirPath))
            {
                filemanager.CheckDirectory(dirPath);
                filemanager.CheckDirectory(dirPaths);
                filemanager.CheckDirectory(dirPath500);
            }
            //var fileName = rndName + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_env.WebRootPath, dirPath + "/" + rndName + Path.GetExtension(file.FileName));
            var filePaths = Path.Combine(_env.WebRootPath, dirPaths + "/" + rndName + Path.GetExtension(file.FileName));
            var filepath500 = Path.Combine(_env.WebRootPath, dirPath500 + "/" + rndName + Path.GetExtension(file.FileName));
            await filemanager.WriteImage(file, filepath500, false, 700, 0);
            await filemanager.WriteImage(file, filePath, false, 0, 0);
            await filemanager.WriteImage(file, filePaths, false, 300, 0);

        }
        public void UpdateSort(int id, int newSort)
        {
            Category image = Get(id);

            image.Sortindex = newSort;

            this._db.SaveChanges();
        }
    }
}