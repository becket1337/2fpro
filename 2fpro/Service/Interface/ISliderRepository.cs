using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;
using Microsoft.AspNetCore.Http;

namespace _2fpro.Service.Interface
{
    public interface ISliderRepository
    {
        IQueryable<Portfolio> Sliders { get; }
        IEnumerable<Portfolio> getAll();

        Task Create(Portfolio folio = null, IFormFile file = null);
        Task Edit(Portfolio folio = null, IFormFile file = null);
        Portfolio GetPortfolio(int id);
        void Save();
        void Delete(Portfolio folio = null);
        Task DeleteAll();
        Task UpdateSort(int id, int sort);
        Task<Portfolio> GetAsync(int id);
        Task<List<Portfolio>> GetAsync();

    }
}