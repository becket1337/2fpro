using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _2fpro.Providers;

namespace _2fpro.Extension
{
    public static class FileFormat
    {
        public static string ToFileSize(this long l)
        {
            return String.Format(new FileSizeFormatProvider(), "{0:fs}", l);
        }
    }
}