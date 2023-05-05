using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Models
{
    public class SiteConfig
    {
        public SiteConfig()
        {

        }
        public string SiteName { get; set; }
        public string SitePath { get; set; }
        public bool SiteLiveStatus { get; set; }

        public bool SiteIsOffline { get; set; }
        public bool AllowCaptcha { get; set; }

        public string MailUsername { get; set; }
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }
        public string MailHost { get; set; }
    }
}
