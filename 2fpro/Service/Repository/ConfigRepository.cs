using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace _2fpro.Service.Repository
{
    public class ConfigRepository : IConfigRepository
    {
        private ApplicationDbContext db;

        public ConfigRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IQueryable<Config> Configs { get { return db.Configs; } }

        public async Task Edit(Config cnf)
        {
            // 
            var local = db.Set<Config>()
                .Local
                .FirstOrDefault(entry => entry.ConfigID.Equals(cnf.ConfigID));

            // check if local is not null 
            if (local != null)
            {
                // detach
                db.Entry(local).State = EntityState.Detached;
            }
            db.Entry(cnf).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task<Config> Options()
        {
            return await db.Configs.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}