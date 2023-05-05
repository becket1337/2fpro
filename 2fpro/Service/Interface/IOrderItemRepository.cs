using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using _2fpro.Models;

namespace _2fpro.Service.Interface
{
    public interface IOrderItemRepository
    {
        IQueryable<OrderItem> OrderItems { get; }

        Task Create(Product prod, int quantity, int orderId, float lastPrice);

        OrderItem Get(int id);
        void Delete(OrderItem orderItem);
        void Edit(OrderItem orderItem);
    }
}