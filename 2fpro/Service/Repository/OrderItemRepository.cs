using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Data;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Service.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private ApplicationDbContext db;

        public OrderItemRepository(ApplicationDbContext _db)
        {
            db = _db;
        }


        public IQueryable<OrderItem> OrderItems { get { return db.OrderItems; } }

        public async Task Create(Product product, int quantity, int orderId, float lastPrice)
        {
            OrderItem oitem = new OrderItem();

            oitem.OrderID = orderId;
            oitem.ProductName = product.ProductName;
            oitem.Quantity = quantity;

            oitem.Discount = product.Discount;
            oitem.Price = (float)product.DiscountedPrice();
            oitem.LastPrice = lastPrice;
            oitem.Category = db.Categories.Find(product.CategoryID).CategoryName;
            oitem.Description = product.Description;
            await db.OrderItems.AddAsync(oitem);
            await db.SaveChangesAsync();
        }

        public OrderItem Get(int id)
        {
            var item = db.OrderItems.Find(id);
            return item;
        }
        public void Edit(OrderItem orderitem)
        {
            db.Entry(orderitem).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void Delete(OrderItem orderitem)
        {
            db.OrderItems.Remove(orderitem);
            db.SaveChanges();
        }
    }
}