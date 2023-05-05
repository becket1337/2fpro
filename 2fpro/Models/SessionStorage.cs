using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Models
{
    public class SessionStorage
    {
        public Cart Cart { get; set; }

        public IPGeoLocation Geoloc { get; set; }

        public DateTime LastVisited { get; set; }
    }
}
