using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Models;
using _2fpro.ViewModel;

namespace _2fpro.ViewModel
{
    public class SectionsViewModel
    {
        public IEnumerable<StaticSection> StaticSections { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}