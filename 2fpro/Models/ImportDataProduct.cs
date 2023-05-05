using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{
    public class ImportDataProduct
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }
        public string Price { get; set; }
        public string IsDeleted { get; set; }
        public string ImgUrl { get; set; }
    }
}