using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;
using Microsoft.AspNetCore.Http;

namespace _2fpro.Service.Interface
{
    public interface IPostRepository
    {
        IQueryable<Post> Posts { get; }

        void Create(Post post);
        void RemoveDir(string path);
        Post Get(int id);
        void Delete(Post post);
        void Edit(Post post);
        Task SavePhoto(IFormFile file, int pid, bool isWM = false);
        //void PhotoDel(string src);
        void PhotoDel(Post cat);
    }
}