using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace _2fpro.ViewModel
{
    public class PagingInfo
    {
        public PagingInfo()
        {
            cattype = "models";
            page = 1;
        }

        private PagingInfo paingInfo { get; set; }

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Service { get; set; }
        public int CatId { get; set; }

        public string cattype { get; set; }
        public int[] models { get; set; }
        public int[] parts { get; set; }

        public int Page { get; set; }
        public int page { get; set; }
        public int catid { get; set; }// for url param
        public string catname { get; set; }
        public string Sort { get; set; }
        public DirectoryInfo Dir { get; set; }
        public string DirName { get; set; }

        public bool Condition { get; set; }
        public Func<int, string> PageUrl { get; set; }
        public Func<int, string, string> PageUrlWithParam { get; set; }
        public Func<int, int, string, string> PageUrlWithParam2 { get; set; }
        public int TotalPage
        {
            get { return (int)Math.Ceiling((double)TotalItems / (double)ItemsPerPage); }
        }
    }
}