using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace _2fpro.Models
{

    [Serializable()]
    public class Order
    {
        public int ID { get; set; }

        [StringLength(300)]
        [Required]
        public string Name { get; set; }
        [StringLength(500)]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }

        [StringLength(300)]
        public string OrderStatus { get; set; }

        [StringLength(300)]
        public string Country { get; set; }

        [StringLength(300)]
        public string Delivery { get; set; }

        [StringLength(300)]
        public string Payment { get; set; }

        public string Comment { get; set; }

        public string EmailAddress { get; set; }

        public DateTime CreatedAt { get; set; }

        public float OrderSum { get; set; }

        public int Sequance { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}