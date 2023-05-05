using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Net;

using _2fpro.Models;

namespace _2fpro.Exstentions
{
    public static class SendMail
    {
        public static void SendResetEmail(string resetToken)
        {
            //    MailMessage email = new MailMessage();
            //    var userId = WebSecurity.GetUserIdFromPasswordResetToken(resetToken);
            //    UserProfile user = null;
            //    using (ApplicationDbContext db = new ApplicationDbContext()) {
            //        user = db.UserProfiles.Single(x => x.UserId == userId);
            //    }
            //    email.From = new MailAddress("inbox@ontrue.ru");
            //    email.To.Add(new MailAddress(user.Email));

            //    email.Subject = "Смена пароля";
            //    email.IsBodyHtml = true;
            //    string link = "http://localhost:2389/?resetToken=" + resetToken;
            //    email.Body = "<p>Проследуйте по ссылке для смены вашего пароля <a href=" + link + ">" + link + "</a></p>" ;
            //    email.Body += "<p>If you did not request a password reset you do not need to take any action.</p>";

            //    using (var smtp = new SmtpClient())
            //    {
            //        smtp.EnableSsl = true;
            //        smtp.Host = "mail.infobox.ru";
            //        smtp.Port = 2525;
            //        smtp.Credentials = new NetworkCredential("inbox@ontrue.ru", "shamutra1");

            //        smtp.Send(email);
            //    }


            //}

            //public static string HashResetParams(string username, string guid)
            //{

            //    byte[] bytesofLink = System.Text.Encoding.UTF8.GetBytes(username + guid);
            //    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //    string HashParams = BitConverter.ToString(md5.ComputeHash(bytesofLink));

            //    return HashParams;
            //}

        }
    }
}