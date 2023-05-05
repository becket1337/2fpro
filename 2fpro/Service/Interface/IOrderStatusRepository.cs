using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;

namespace _2fpro.Service.Interface
{
    public interface IOrderStatusRepository
    {
        IQueryable<OrderStatus> OrderStatuses { get; }

        void Create(OrderStatus orderStatus);

        OrderStatus Get(int id);
        void Delete(OrderStatus orderStatus);
        void Edit(OrderStatus orderStatus);
    }
}