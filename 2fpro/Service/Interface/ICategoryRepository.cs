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
    public interface ICategoryRepository
    {
        IQueryable<Category> Categories { get; }
        Task<List<Product>> GetForSlider();
        Task Create(CategoryViewModel post);
        IQueryable<Category> Get();
        Category Get(int id);
        Task<Category> GetAsync(int id);
        void Delete(Category post);
        Task Edit(CategoryViewModel post);
        void UpdateSort(int id, int newSort);
        void PhotoDel(Category cat);
        Task SavePhoto(IFormFile file, int pid);
        Task<List<Category>> GetAsync();
        Task<List<Category>> GetFilteredAsync(int[] model, int[] parts);
        Task<List<Category>> GetShowed();
        Task<List<Category>> getOnlyCats();
        Task newCat(string catname);

    }
}