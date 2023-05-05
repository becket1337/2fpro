using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _2fpro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2fpro.ViewModel
{
    [Serializable]
    public class OrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }

        public Order Order { get; set; }

        public string OrderStatus { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }
        public int Items { get; set; }
        public PagingInfo PagingInfo { get; set; }

        public IEnumerable<SelectListItem> StatusList
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem {Text="Не просмотрено"},
                    new SelectListItem {Text="Отменено"},
                    new SelectListItem {Text="В процессе"},
                    new SelectListItem {Text="Завершен"}
                };

            }
        }
        public IEnumerable<SelectListItem> OrderFilters
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem {Text="Не просмотрено",Value="Не просмотрено"},

                    new SelectListItem {Text="Отменено",Value="Отменено"},
                    new SelectListItem {Text="В процессе",Value="В процессе"},
                    new SelectListItem {Text="Завершен",Value="Завершен"}
                };

            }
        }

        [Required(ErrorMessage = "Заполните имя")]
        public string Name { get; set; }
        public string Service { get; set; }
        public string Work { get; set; }
        public DateTime OrderDate { get; set; }
        public string Desc { get; set; }
        public string Price { get; set; }
        [Required(ErrorMessage = "Заполните адрес")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Заполните номер телефона")]
        public string Phone { get; set; }

        public string Email { get; set; }

        public string Quan { get; set; }
        public string ProdName { get; set; }
        public string TotalSum { get; set; }
        public int prodId { get; set; }


    }
}