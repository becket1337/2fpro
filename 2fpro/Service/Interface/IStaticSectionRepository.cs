using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;

namespace _2fpro.Service.Interface
{
    public interface IStaticSectionRepository
    {
        IQueryable<StaticSection> StaticSections { get; }

        void Edit(StaticSection section);
        StaticSection GetSection(int type);
        StaticSection Get(int id);
        void Delete(StaticSection section);
    }
}