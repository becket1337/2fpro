using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class OrderStatusRepository : IOrderStatusRepository
    {
        private ApplicationDbContext db;

        public OrderStatusRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IQueryable<OrderStatus> OrderStatuses { get { return db.OrderStatuses; } }

        public void Create(OrderStatus orderStatus)
        {
            db.OrderStatuses.Add(orderStatus);
            db.SaveChanges();
        }

        public OrderStatus Get(int id)
        {
            var item = db.OrderStatuses.Find(id);
            return item;
        }
        public void Edit(OrderStatus orderStatus)
        {
            db.Entry(orderStatus).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(OrderStatus orderStatus)
        {
            db.OrderStatuses.Remove(orderStatus);
            db.SaveChanges();
        }
    }
}