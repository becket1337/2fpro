using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace _2fpro.Extension
{
    public static class MainExt
    {
        public static string GetLocalhostPath()
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
        }
    }
}