using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;
using Microsoft.AspNetCore.Http;

namespace _2fpro.Service.Interface
{
    public interface IPhotoGallery
    {
        Task Create(Gallery gallery = null, IFormFile file = null, int galId = 0);
        void Delete(Gallery gallery = null, Image image = null);
        void DeleteAll(int id);
        void Edit(Gallery gallery = null, Image image = null, IFormFile file = null, int galId = 0);
        IQueryable<Gallery> GalAll();
        Gallery GetGallery(int id);
        Image GetImage(int id);
        void Save();
        void UpdateImage(int id, string title);
        void UpdateSort(int id, int newSort);


        IQueryable<Gallery> Galleries { get; }

        IQueryable<Image> Images { get; }
    }
}