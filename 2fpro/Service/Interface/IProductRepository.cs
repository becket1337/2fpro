using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Http;

namespace _2fpro.Service.Interface
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        IQueryable<ProdImage> ProdImages { get; }

        Task CreateAsync(Product menu, ProductViewModel prod = null);
        Task SetPreview(int pimgid);
        Task SavePhoto(IFormFile file, int pid);
        ProdImage GetImg(int pimgid);
        Task<bool> GetPreviewImg(int pid);
        Task<bool> CheckPreview(int pimgid);
        void PhotoDel(ProdImage pimg);
        Task UpdateSort(int id, int sort);
        Product Get(int id);
        Task<List<Product>> GetAsync();
        List<Product> Get();
        void Delete(Product menu);
        Task EditAsync(Product menu);
        void Save();
        Task<string> GetCatName(int id);
        bool ProdIsAdded(int id, Cart cart);
        Task<List<Product>> GetItemsAsync();
        void PhotoDelAll(int id);
        Task ClearTable();
        Task<Product> GetAsync(int id);
        Task<ProdImage> GetImgAsync(int id);
        void UpdateProdSort(int id, int sort);
        Task<List<Product>> GetByCatid(int id);
        Task<List<Product>> GetShowed();
        Task<List<Product>> GetProdsByFilter(List<Category> arr);
    }
}