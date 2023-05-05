using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private ApplicationDbContext db;

        public OrderRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IQueryable<Order> Orders { get { return db.Orders.Include("OrderItems"); } }

        public async Task Create(Order order)
        {
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();
        }

        public Order Get(int id)
        {
            var item = Orders.FirstOrDefault(x => x.ID == id);
            return item;
        }
        public void Edit(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(Order order)
        {
            db.Orders.Remove(order);
            db.SaveChanges();
        }
        public async Task DeleteAll()
        {
            db.Orders.RemoveRange(await Orders.ToListAsync());
            db.SaveChanges();
        }
        public void Update()
        {
            db.SaveChanges();
        }
    }
}