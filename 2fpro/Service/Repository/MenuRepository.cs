using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private ApplicationDbContext db;
        public MenuRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IQueryable<Menu> Menues { get { return db.Menues; } }

        public void Create(Menu menu)
        {
            menu.LastModifiedDate = DateTime.Now;
            //Menu image = (from i in this.db.Images
            //               where i.Sortindex == (from x in this.db.Images select x.Sortindex).Max<int>()
            //               select i).SingleOrDefault<Image>();
            //int num = (image == null) ? 1 : (image.Sortindex + 1);


            db.Menues.Add(menu);
            db.SaveChanges();

        }
        public async Task<string> GetMenuName(int? parentId)
        {
            int newId = parentId ?? 0;


            if (newId != 0)
            {
                var item = await GetAsync(newId);
                return item.Text;
            }
            return "";

        }
        public string GetMenuUrll(int parentid)
        {
            // int newId = parentid ?? 0;


            if (parentid != 0)
            {
                var item = db.Menues.Find(parentid);
                return item.Url;
            }
            return "";
        }
        public async Task<string> GetMenuUrl(int parentid)
        {
            // int newId = parentid ?? 0;


            if (parentid != 0)
            {
                var item = await db.Menues.FindAsync(parentid);
                return item.Url;
            }
            return "";
        }
        public async Task<Menu> GetAsync(int id)
        {
            var item = await db.Menues.FindAsync(id);
            return item;
        }
        public async Task<IEnumerable<Menu>> GetAsync()
        {
            var item = await db.Menues.ToListAsync();
            return item;
        }
        public void Edit(Menu menu)
        {
            menu.LastModifiedDate = DateTime.Now;

            db.Entry(menu).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Menu menu)
        {
            if (menu.ParentId == 0 && db.Menues.Where(x => x.ParentId == menu.Id).Any())
                db.Menues.RemoveRange(db.Menues.Where(x => x.ParentId == menu.Id));
            db.Menues.Remove(menu);
            db.SaveChanges();
        }

        public void UpdateSort(int id, int newSort)
        {
            Menu image = db.Menues.Find(id);

            image.SortOrder = newSort;

            this.db.SaveChanges();
        }
    }
}