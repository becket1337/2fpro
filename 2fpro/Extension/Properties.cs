using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace _2fpro.Extension
{
    public static class Properties
    {


        public static bool IsNullOrEmpty(this object obj)
        {
            return obj == null || String.IsNullOrWhiteSpace(obj.ToString());
        }
    }
}