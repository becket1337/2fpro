using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace _2fpro.Models
{
    public class Post
    {
        public int ID { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage = "Заполните поле")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Заполните поле")]

        public string Body { get; set; }

        public byte[] PreviewPhoto { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageMimeType { get; set; }
        public string CustomStr { get; set; }

        public int Visitors { get; set; }
        public DateTime LastModified { get; set; }
        public string SecondCustomStr{ get; set; }

        public string Preview { get; set; }
        public int Sortindex { get; set; }
    }
}