//using System;
//using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using _2fpro.Service.Interface;
using System.Configuration;

using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using _2fpro.Models;
using System;

namespace _2fpro.Service.Repository
{

    public class EmailService : IEmailService
    {
        private readonly SiteConfig _settings;

        public EmailService(IOptionsMonitor<SiteConfig> settings)
        {
            _settings = settings.CurrentValue;
        }
        public async Task SendEmailAsync(string name, string message, string title, string to, bool isOrder = false)
        {
            var username = _settings.MailUsername;
            var pass = _settings.MailPassword;
            var server = _settings.MailHost;
            var email = _settings.MailAddress;
            to = isOrder ? _settings.MailAddress : _settings.MailAddress;
            string builder = "";
            if (isOrder)
            {
                // заказ с каталога
                builder =
                   $@"<h2>{_settings.SiteName}</h2><br />
                Пришло сообщение с сайта от {name}<br />
                -----------------------------------------------------
                <p>{message}</p><br />";
            }
            else
            {
                //обратная связь
                builder =
                  $@"
                Пришло сообщение с сайта {_settings.SiteName}<br />
                -----------------------------------------------------
                <p>от : {name}</p>
                <p>{message}</p><br />";
            }
            using (MailMessage message2 = new MailMessage(
                email,
                email,
                title,
                builder))
            {

                message2.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient(server))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(username, pass);
                    client.EnableSsl = false;
                    client.Port = 25;
                    await client.SendMailAsync(message2);
                };

            };


            //SmtpClient client = new SmtpClient("smtp.yandex.ru");

            //client.Credentials = new NetworkCredential("crew1251", "qweewq1251");
            //client.Port = 465;
            //client.EnableSsl = true;
            //client.Send(message2);
        }
    }
}