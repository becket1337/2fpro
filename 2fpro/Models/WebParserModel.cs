using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{
    public class WebParserModel
    {
        public string Id { get; set; }

        public string Price { get; set; }
        public string СartridgeLength { get; set; }
        public string Color { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ModelType { get; set; }
        public string Type { get; set; }
        public string PrintType { get; set; }
        public string resourse { get; set; }
        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = (value == 0 ? 1 : value);
            }
        }
        public string Brand { get; set; }
        public string Referense { get; set; }

        public WebParserModel()
        {
            Id = Guid.NewGuid().ToString();

        }


    }
}