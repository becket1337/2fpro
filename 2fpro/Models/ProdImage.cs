using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _2fpro.Models
{
    [Serializable]
    public class ProdImage
    {
        public int ID { get; set; }

        public byte[] ImageDataType { get; set; }
        [StringLength(300)]
        public string ImageMimeType { get; set; }
        public int IsPreview { get; set; }
        public int ProductID { get; set; }
        public int Sortindex { get; set; }
    }
}