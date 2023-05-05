using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;

namespace _2fpro.Service.Interface
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }

        Task Create(Order order);

        Order Get(int id);
        void Delete(Order order);
        void Edit(Order order);
        void Update();
        Task DeleteAll();
    }
}