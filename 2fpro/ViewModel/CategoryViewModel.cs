using Microsoft.AspNetCore.Mvc;


namespace _2fpro.ViewModel
{
    public class CategoryViewModel
    {
        [HiddenInput]
        public int ID { get; set; }

        public string CategoryName { get; set; }

        public string CatType { get; set; }

        public string CatDescription { get; set; }


        public int? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        public int Sortindex { get; set; }

        public bool DoShow { get; set; }
        public int Prods { get; set; }

        public string EncryptedId { get; set; }
        public string imgLink { get; set; }
        public string ImageMimeType { get; set; }
    }
}