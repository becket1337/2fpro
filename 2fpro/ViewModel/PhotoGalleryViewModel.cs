using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;

namespace _2fpro.ViewModel
{
    public class PhotoGalleryViewModel
    {


        public IEnumerable<Gallery> Galleries { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }
}