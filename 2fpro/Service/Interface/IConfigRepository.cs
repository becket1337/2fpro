using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;

namespace _2fpro.Service.Interface
{

    public interface IConfigRepository
    {
        IQueryable<Config> Configs { get; }

        Task Edit(Config cnf);
        Task<Config> Options();
    }
}