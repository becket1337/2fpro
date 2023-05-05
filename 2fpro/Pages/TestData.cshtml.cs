using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using _2fpro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace _2fpro.Pages
{
    public class TestDataModel : PageModel
    {
        IDistributedCache _cache;

        public TestDataModel(IDistributedCache cache)
        {
            _cache = cache;
        }
        public IPGeoLocation GeoLoc { get; set; }
        public TimeSpan src { get; set; }
        public async Task OnGetAsync()
        {
            var date1 = new DateTime(1970, 12, 1);
            var date2 = new DateTime(2007, 6, 28);
            src = date1 - date2;
        }

        public async Task<IPGeoLocation> GetUserLocation()
        {
            var ip = "";
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }

            IPGeoLocation model = await IPGeoLocation.QueryGeographicalLocationAsync(ip);
            return model;
        }


    }
}