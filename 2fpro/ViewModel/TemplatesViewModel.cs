using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2fpro.ViewModel
{
    public class TemplatesViewModel
    {
        public int type { get; set; }

        public string username { get; set; }
        public bool isConfirmed { get; set; }
        public int style { get; set; }
        public string credentialToken { get; set; }
        public string userid { get; set; }
    }
}