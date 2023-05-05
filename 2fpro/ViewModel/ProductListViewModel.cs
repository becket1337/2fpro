using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;

namespace _2fpro.ViewModel
{

    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public int TotalProducts { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public Product Product { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }

        public int[] Models { get; set; }
        public int[] Parts { get; set; }

        public string GetImgLink(int id, string name)
        {

            return "/Content/Files/Product/" + id + "/" + name;
        }
    }
}