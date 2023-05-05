using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{
    public class Image
    {
        public int ID { get; set; }

        [StringLength(500, ErrorMessage = "максимум 500 символов")]
        public string ImageTitle { get; set; }

        public byte[] ImageData { get; set; }

        public int GalleryID { get; set; }
        [StringLength(100, ErrorMessage = "максимум 100 символов")]
        public string ImageMimeType { get; set; }

        public int Sortindex { get; set; }
    }
}