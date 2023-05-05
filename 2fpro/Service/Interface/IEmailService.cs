using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace _2fpro.Service.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string name, string message, string title, string to, bool isOrder = false);
    }
}