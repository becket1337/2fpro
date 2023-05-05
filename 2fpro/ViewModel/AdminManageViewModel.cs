using _2fpro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewModel
{
    public class AdminManageViewModel
    {
        public List<Menu> menus { get; set; }
        public List<Post> posts { get; set; }
        public List<Order> orders { get; set; }
        public List<Category> cats { get; set; }
        public List<Product> prods { get; set; }
    }
}
