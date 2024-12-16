
using _2fpro.Models.Middleware;
using Microsoft.AspNetCore.Rewrite;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Extension
{
    public static class RewriteOptionsExtensions
{
        public static RewriteOptions AddRedirectToWww(this RewriteOptions options)
        {
            options.Rules.Add(new RedirectToWwwRule());
            return options;
        }
    }
}
