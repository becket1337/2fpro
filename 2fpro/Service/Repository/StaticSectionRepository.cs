using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using _2fpro.Service.Interface;
using _2fpro.Models;
using _2fpro.Areas.Admin.Controllers;
using _2fpro.Data;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class StaticSectionRepository : IStaticSectionRepository
    {
        private ApplicationDbContext db;

        public StaticSectionRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IQueryable<StaticSection> StaticSections { get { return db.StaticSections; } }

        public void Edit(StaticSection cnf)
        {
            db.Entry(cnf).State = EntityState.Modified;
            db.SaveChanges();
        }
        public StaticSection Get(int id)
        {
            return db.StaticSections.SingleOrDefault(x => x.ID == id);
        }
        public void Delete(StaticSection section)
        {
            db.StaticSections.Remove(section);
            db.SaveChanges();
        }
        public StaticSection GetSection(int type)
        {
            switch (type)
            {
                case 1:
                    return db.StaticSections.First(x => x.Type == 1);
                case 2:
                    return db.StaticSections.First(x => x.Type == 2);
                case 3:
                    return db.StaticSections.First(x => x.Type == 3);
                case 4:
                    return db.StaticSections.First(x => x.Type == 4);
                case 5:
                    return db.StaticSections.First(x => x.Type == 5);
                case 6:
                    return db.StaticSections.First(x => x.Type == 6);
                default:
                    return db.StaticSections.First();
            }

        }
    }
}