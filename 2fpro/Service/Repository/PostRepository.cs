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
    public class PostRepository : IPostRepository
    {
        private ApplicationDbContext db;
        private FileManager filemanager = new FileManager();
        private IHostingEnvironment _env;
        public PostRepository(ApplicationDbContext _db, IHostingEnvironment env)
        {
            db = _db;
            _env = env;
        }

        public IQueryable<Post> Posts { get { return db.Posts; } }

        public void Create(Post post)
        {

            //WebImage formImg = null;
            //byte[] imgBytes = null;
            //if (file.ContentLength > 1000000)
            //{
            //    formImg = new WebImage(file.InputStream);
            //    imgBytes = formImg.Resize(formImg.Width / 2, formImg.Height / 2).GetBytes();
            //    post.PreviewPhoto = imgBytes;
            //}
            //else post.PreviewPhoto = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

            db.Posts.Add(post);
            db.SaveChanges();
        }

        public Post Get(int id)
        {
            var item = db.Posts.Find(id);
            return item;
        }
        public void Edit(Post post)
        {


            var item = Get(post.ID);
            item.CreatedAt = post.CreatedAt;
            item.Title = post.Title;
            item.Body = post.Body;
            item.Preview = post.Preview;

            db.SaveChanges();
        }
        public void Delete(Post post)
        {

            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Post/" + post.ID.ToString());

            filemanager.RemoveDir(dirPath);
            db.Posts.Remove(post);
            db.SaveChanges();
        }


        public async Task SavePhoto(IFormFile file, int pid, bool isWM = false)
        {

            if (file.Length > 4000000) throw new Exception();

            var rndName = filemanager.GetRandomName();
            var post = Get(pid);
            post.ImageMimeType = rndName + Path.GetExtension(file.FileName);
            
            await db.SaveChangesAsync();

            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Post/" + pid.ToString());
            var dirPaths = Path.Combine(_env.WebRootPath, "Content/Files/Post/" + pid.ToString() + "/s");

            if (!Directory.Exists(dirPath))
            {
                filemanager.CheckDirectory(dirPath);
                filemanager.CheckDirectory(dirPaths);
            }
            //var fileName = rndName + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_env.WebRootPath, dirPath + "/" + rndName + Path.GetExtension(file.FileName));
            var filePaths = Path.Combine(_env.WebRootPath, dirPaths + "/" + rndName + Path.GetExtension(file.FileName));
            await filemanager.WriteImage(file, filePath, false, 1920, 0);
            await filemanager.WriteImage(file, filePaths, false, 500, 0);
        }
        public void PhotoDel(Post cat)
        {
            var dirPath = Path.Combine(_env.WebRootPath, "Content/Files/Post/" + cat.ID.ToString());

            filemanager.RemoveDir(dirPath);
            cat.ImageMimeType = "";
            db.Entry(cat).State = EntityState.Modified;
            db.SaveChanges();

        }
        public void RemoveDir(string src)
        {
            filemanager.RemoveDir(src);

        }
    }
}