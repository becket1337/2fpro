using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;



namespace _2fpro.ViewModel
{
    [Serializable]
    public class ProductViewModel
    {


        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Наименование - обязательное к заполнению")]
        public string ProductName { get; set; }
        public string imgLink { get; set; }
        public string Description { get; set; }
        [Required]
        public int CategoryID { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string CategoryName { get; set; }
        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }
        [StringLength(300)]
        public string ProductType { get; set; }

        public bool InStock { get; set; } // в наличии
        public bool DoShow { get; set; } // Видимость в каталоге
        public bool ToYaMarket { get; set; } // В ямаркет

        public string Size { get; set; } // размеры
        public string Packaging { get; set; } // упак
        public float PackagingSize { get; set; } //размер упак
        public float Weight { get; set; } // вес
        public string Manufacturer { get; set; } //производитель
        public string Cloth { get; set; }//ткань
        public string Color { get; set; } //окрас
        public string Lacquering { get; set; }//лакировка
        public string Decor { get; set; } //декор


        public int Discount { get; set; } //скидка
        public string Material { get; set; }//материал
        public int Sortindex { get; set; }

        public string Fill { get; set; }//наполнение



    }
}