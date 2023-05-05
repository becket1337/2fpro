using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace _2fpro.Service.Repository
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext db;
        private IFileManager filemanager;
        private ICategoryRepository _catRep;
        private IHostingEnvironment _env;

        public ProductRepository(ApplicationDbContext _db, ICategoryRepository catRep, IHostingEnvironment env, IFileManager _filemanager)
        {
            db = _db;
            _catRep = catRep;
            _env = env;
            filemanager = _filemanager;

        }

        public IQueryable<Product> Products { get { return db.Products; } }
        public IQueryable<ProdImage> ProdImages { get { return db.ProdImages; } }

        public async Task CreateAsync(Product product, ProductViewModel prod = null)
        {


            if (product != null)
            {
                var cat = await _catRep.GetAsync(product.CategoryID);
                product.CatName = cat.CategoryName;
                await db.Products.AddAsync(product);
            }
            else
            {
                var cat = await _catRep.GetAsync(prod.CategoryID);
                var newP = new Product();
                newP.ProductName = prod.ProductName;
                newP.Description = prod.Description;
                newP.Cloth = prod.Cloth;
                newP.Color = prod.Color;
                newP.Decor = prod.Decor;
                newP.Fill = prod.Fill;
                newP.Discount = prod.Discount;
                newP.Lacquering = prod.Lacquering;
                newP.Material = prod.Material;
                newP.Packaging = prod.Packaging;
                newP.PackagingSize = prod.PackagingSize;
                newP.Weight = prod.Weight;
                newP.Size = prod.Size;
                newP.ProductType = prod.ProductType;
                newP.Price = prod.Price;
                newP.CategoryID = prod.CategoryID;
                newP.CatName = cat.CategoryName;
                newP.ModifiedDate = DateTime.Now;
                await db.Products.AddAsync(newP);
            }
            await db.SaveChangesAsync();

        }

        public async Task<List<Product>> GetAsync()
        {
            return await db.Products.Include("ProdImages").ToListAsync();
        }
        public List<Product> Get()
        {
            return db.Products.ToList();
        }
        public async Task<List<Product>> GetShowed()
        {
            return await db.Products.Where(x => !x.DoShow).Include("ProdImages").ToListAsync();
        }
        public void UpdateProdSort(int id, int newSort)
        {
            Product image = Get(id);

            image.Sortindex = newSort;

            db.SaveChanges();
        }
        public async Task SavePhoto(IFormFile file, int pid)
        {



            var rndName = filemanager.GetRandomName();
            var prodImg = new ProdImage();

            prodImg.ProductID = pid;
            prodImg.ImageMimeType = rndName + Path.GetExtension(file.FileName);
            db.ProdImages.Add(prodImg);
            db.SaveChanges();

            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pid.ToString());
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pid.ToString() + "/200x150");
            var dirPath500 = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pid.ToString() + "/height500");

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
            await filemanager.WriteImage(file, filepath500, false, 800, 0);
            await filemanager.WriteImage(file, filePath, false, 0, 0);
            await filemanager.WriteImage(file, filePaths, false, 400, 0);

        }
        public async Task SetPreview(int pimgid)
        {

            var prodImg = await db.ProdImages.FindAsync(pimgid);
            var prod = await db.Products.Include("ProdImages").FirstOrDefaultAsync(x => x.ID == prodImg.ProductID);
            foreach (var item in prod.ProdImages)
            {
                item.IsPreview = 0;
            }
            prodImg.IsPreview = 1;
            await db.SaveChangesAsync();
        }
        public async Task<string> GetCatName(int id)
        {

            var item = await db.Categories.FindAsync(id);
            return item.CategoryName;

        }
        public async Task<List<Product>> GetByCatid(int id)
        {
            return await Products.Include("ProdImages").Where(x => x.CategoryID == id && !x.DoShow).ToListAsync();
        }
        public async Task<List<Product>> GetProdsByFilter(List<Category> cats)
        {
            int[] str = null;
            cats.ForEach(x => str.Append(x.ID));
            return await Products.Include("ProdImages").Where(x => str.Contains(x.CategoryID)).ToListAsync();
        }
        public bool ProdIsAdded(int id, Cart cart)
        {

            bool isAdded = false;
            if (cart != null)
            {
                isAdded = cart.Lines.Select(x => x.Product.ID).Contains(id);
            }
            return isAdded;
        }
        public ProdImage GetImg(int pimgid)
        {
            var prodImg = db.ProdImages.Find(pimgid);
            return prodImg;
        }
        public Task<List<Product>> GetItemsAsync()
        {
            return db.Products.ToListAsync();
        }
        public async Task<bool> GetPreviewImg(int pid)
        {
            var prod = await db.Products.FirstOrDefaultAsync(x => x.ID == pid);
            return prod.ProdImages.Any(x => x.IsPreview == 1);
        }

        public async Task UpdateSort(int id, int newSort)
        {
            ProdImage image = await db.ProdImages.FirstOrDefaultAsync(x => x.ID == id);

            image.Sortindex = newSort;

            await db.SaveChangesAsync();
        }
        public async Task<bool> CheckPreview(int pimgid)
        {
            var prodimg = await db.ProdImages.FirstOrDefaultAsync(x => x.ID == pimgid);
            return prodimg.IsPreview == 1 ? true : false;
        }
        public void PhotoDel(ProdImage pimg)
        {
            var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pimg.ProductID.ToString() + "/" + Prodimg.ImageMimeType);
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pimg.ProductID.ToString() + "/200x150/" + Prodimg.ImageMimeType);
            var dirPath500 = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + pimg.ProductID.ToString() + "/height500/" + Prodimg.ImageMimeType);
            filemanager.RemoveFile(dirPath);
            filemanager.RemoveFile(dirPaths);
            filemanager.RemoveFile(dirPath500);

            db.ProdImages.Remove(pimg);
            db.SaveChanges();
        }
        public void PhotoDelAll(int id)
        {
            if (id != 0)
            {
                if (Products.Include("ProdImages").SingleOrDefault(x => x.ID == id).ProdImages.Any())
                {

                    db.ProdImages.RemoveRange(db.ProdImages.Where(x => x.ProductID == id));
                    db.SaveChanges();
                }

                filemanager.RemoveDir(Path.Combine(_env.WebRootPath, "Content/Files/Product/" + id.ToString()));
            }


        }
        public async Task ClearTable()
        {
            filemanager.RemoveDir(Path.Combine(_env.WebRootPath, "Content/Files/Product"));
            if (await db.Products.AnyAsync())
            {
                db.Products.RemoveRange(db.Products);
                await db.SaveChangesAsync();
            }

        }
        public void Save()
        {
            db.SaveChanges();
        }

        public Product Get(int id)
        {
            var item = db.Products.Find(id);
            return item;
        }
        public async Task<Product> GetAsync(int id)
        {
            var item = await db.Products.FindAsync(id);
            return item;
        }
        public async Task<ProdImage> GetImgAsync(int id)
        {
            var item = await db.ProdImages.FindAsync(id);
            return item;
        }
        public async Task EditAsync(Product product)
        {


            db.Entry(product).State = EntityState.Modified;
            product.ModifiedDate = DateTime.Now;
            await db.SaveChangesAsync();
        }
        public void Delete(Product product)
        {
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Product/" + product.ID.ToString());
            filemanager.RemoveDir(dirPath);
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}