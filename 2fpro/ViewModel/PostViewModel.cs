using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;

namespace _2fpro.ViewModel
{
    public class PostViewModel
    {

        public IEnumerable<Post> Posts { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}