using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class PhotoGalleryRepository : IPhotoGallery
    {
        ApplicationDbContext db;
        private IHostingEnvironment _env;

        public PhotoGalleryRepository(ApplicationDbContext _db, IFileManager _fileManager,  IHostingEnvironment env)
        {
            db = _db;
            filemanager = _fileManager;
            _env=env;
        }

        public IQueryable<Image> Images { get { return db.Images; } }
        public IQueryable<Gallery> Galleries { get { return db.Galleries; } }
        private IFileManager filemanager;

        public IQueryable<Gallery> GalAll()
        {
            return from obj in db.Galleries.Include("Images") select obj;
        }

        public async Task Create(Gallery gallery = null, IFormFile file = null, int galId = 0)
        {


            if (gallery != null)
            {
                var gall = new Gallery();
                gall.GalleryMimeType = file.ContentType;

                gall.GalleryTitle = gallery.GalleryTitle;

                this.db.Galleries.Add(gall);
                db.SaveChanges();
                gall.Sortindex = gall.ID;
                db.SaveChanges();
                int res = await SavePhoto(file, gall.ID, true);
            }
            else
            {

                Image image2 = new Image();

                image2.GalleryID = galId;
                this.db.Images.Add(image2);
                db.SaveChanges();
                image2.Sortindex = image2.ID;
                db.SaveChanges();
                int res = await SavePhoto(file, image2.ID, false);
            }

        }

        public async void Edit(Gallery gallery = null, Image image = null, IFormFile file = null, int galId = 0)
        {
            byte[] newdata = null;
            if (gallery != null)
            {
            
                var gal = db.Galleries.Find(gallery.ID);
                    gal.GalleryTitle = gallery.GalleryTitle;
                if (file != null)
                { int res = await SavePhoto(file, gallery.ID, false); }
            }
            else
            {
               
                db.Entry(image).State = EntityState.Modified;
                if (file != null)
                { int res = await SavePhoto(file, gallery.ID, false); }
            }
            db.SaveChanges();
        }

        public Image GetImage(int id)
        {
            var image = db.Images.Find(id);
            return image;
        }

        public Gallery GetGallery(int id)
        {
            var gallery = db.Galleries.Include("Images").FirstOrDefault(x=>x.ID==id);
            return gallery;
        }
        public void UpdateImage(int id, string title)
        {
            this.GetImage(id).ImageTitle = title;
            this.db.SaveChanges();
        }
        public void UpdateSort(int id, int newSort)
        {
            Image image = GetImage(id);

            image.Sortindex = newSort;

            this.db.SaveChanges();
        }
        public void Save()
        {
            db.SaveChanges();
        }

        public async Task<int> SavePhoto(IFormFile file, int id, bool isGal = true)
        {

            if (file.Length > 4000000) throw new Exception();


            string dirPath = null;
            string dirPaths = null;
            string rndName = null;
            string filePath = null;
            string filePaths = null;

            int resCount = 0;




            if (isGal)
            {
                var gal = GetGallery(id);
                dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/"+ id.ToString());
                dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/"+ id.ToString()+ "/small");
                filemanager.CheckDirectory(dirPath);
                rndName = filemanager.GetRandomName();
                gal.GalleryMimeType = rndName + Path.GetExtension(file.FileName);
            }
            else
            {
                var galImg = GetImage(id);
            
                dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + galImg.GalleryID.ToString());
                dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + galImg.GalleryID.ToString() + "/small");
                filemanager.CheckDirectory(dirPath);
                rndName = filemanager.GetRandomName();
                galImg.ImageMimeType = rndName + Path.GetExtension(file.FileName);
                resCount = id;
            }


            filePath = Path.Combine(dirPath, rndName + Path.GetExtension(file.FileName));

            filemanager.CheckDirectory(dirPaths);
            filePaths = Path.Combine(dirPaths, rndName + Path.GetExtension(file.FileName));

            await filemanager.WriteImage(file, filePath, false, 1920, 1080);
            await filemanager.WriteImage(file, filePaths, false, 300, 200);

            db.SaveChanges();

            return resCount;
        }


        public void DeleteAll(int id)
        {
            var Prodimg = db.Galleries.SingleOrDefault(x => x.ID == id);

            //var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Gallery/" +id + "/Images/");
            //DirectoryInfo di = new DirectoryInfo(dirPaths);
            //di.Delete(true);

            foreach (var img in Prodimg.Images)
            {
                db.Images.Remove(img);
                db.SaveChanges();
            }


        }
        //public void PhotoDel(ProdImage pimg)
        //{
        //    var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
        //    var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/" + Prodimg.ImageMimeType);
        //    var dirPaths = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + pimg.ProductID + "/200x150" + "/" + Prodimg.ImageMimeType);
        //    filemanager.RemoveFile(dirPath);
        //    filemanager.RemoveFile(dirPaths);

        //    db.ProdImages.Remove(pimg);
        //    db.SaveChanges();
        //}
        //public void Delete(Product product)
        //{
        //    var dirPath = HttpContext.Current.Server.MapPath("~/Content/Files/Product/" + product.ID);
        //    filemanager.RemoveDir(dirPath);
        //    db.Products.Remove(product);
        //    db.SaveChanges();
        //}
        public void GalPhotoDel(Gallery pimg) // удаление превью фото галлереи 
        {
            
            var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + pimg.ID.ToString() + "/" + Prodimg.ImageMimeType);
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + pimg.ID.ToString() + "/small/" + Prodimg.ImageMimeType);
          
            filemanager.RemoveFile(dirPath);
            filemanager.RemoveFile(dirPaths);
           
          
        }
        public void PhotoDel(Image pimg)// удаление внутренних фото галлереи
        {
            var Prodimg = db.ProdImages.SingleOrDefault(x => x.ID == pimg.ID);
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + pimg.GalleryID.ToString() + "/" + Prodimg.ImageMimeType);
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + pimg.GalleryID.ToString() + "/small/" + Prodimg.ImageMimeType);
            filemanager.RemoveFile(dirPath);
            filemanager.RemoveFile(dirPaths);

            db.Images.Remove(pimg);
            db.SaveChanges();
        }
        public void PhotoDelAll(int id)//удаление папки с фото галлереи
        {
            if (id != 0)
            {
                if (Galleries.Include("Images").SingleOrDefault(x => x.ID == id).Images.Any())
                {

                    db.Images.RemoveRange(db.Images.Where(x => x.GalleryID == id));
                    db.SaveChanges();
                }

                filemanager.RemoveDir(Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + id.ToString()));
            }


        }

        public void Delete(Gallery product)
        {
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Gallery/" + product.ID.ToString());
            filemanager.RemoveDir(dirPath);
            db.Galleries.Remove(product);
            db.SaveChanges();
        }
        public void Delete(Gallery gallery = null, Image image = null)
        {
            if (image != null)
            {
                var img = db.Images.SingleOrDefault(x => x.ID == image.ID);
                var filePath = Path.Combine("~/Content/Files/Gallery/", img.GalleryID.ToString(), "/", img.ImageMimeType);
                filemanager.RemoveFile(filePath);

                db.Images.Remove(image);
                db.SaveChanges();
            }
            else
            {
                var gal = db.Galleries.SingleOrDefault(x => x.ID == gallery.ID);
                var filePath = Path.Combine("~/Content/Files/Gallery/", gal.ID.ToString());
                filemanager.RemoveDir(filePath);
                db.Galleries.Remove(gallery);
                db.SaveChanges();
            }
        }
    }
}