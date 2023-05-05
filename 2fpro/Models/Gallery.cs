using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{
    public class Gallery
    {
        public int ID { get; set; }

        [StringLength(500, ErrorMessage = "максимум 500 символов")]
        public string GalleryTitle { get; set; }

        public virtual List<Image> Images { get; set; }

        public byte[] GalleryData { get; set; }
        public int Sortindex { get; set; }
        [StringLength(100, ErrorMessage = "максимум 100 символов")]
        public string GalleryMimeType { get; set; }
    }
}