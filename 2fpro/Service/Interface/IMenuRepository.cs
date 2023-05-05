using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using _2fpro.Models;

namespace _2fpro.Service.Interface
{
    public interface IMenuRepository
    {
        IQueryable<Menu> Menues { get; }

        void Create(Menu menu);

        Task<IEnumerable<Menu>> GetAsync();
        Task<Menu> GetAsync(int id);
        void Delete(Menu menu);
        void Edit(Menu menu);
        void UpdateSort(int id, int newSort);
        Task<string> GetMenuName(int? parentId);
        Task<string> GetMenuUrl(int parentId);
        string GetMenuUrll(int id);
    }
}