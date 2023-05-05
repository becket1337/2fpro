using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2fpro.ViewModel
{
    public class TreeViewModel
    {
        public string Text { get; set; }
        public List<TreeViewModel> Items { get; set; }
    }
}