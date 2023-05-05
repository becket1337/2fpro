using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;



namespace _2fpro.Areas.Admin.Controllers
{
    public class DataCaptchaController : Controller
    {
        //
        // GET: /Admin/DataCaptcha/

        public ActionResult Show()
        {

            var randomText = GenerateRandomText(6);
            var hash = ComputeMd5Hash(randomText + GetSalt());
            HttpContext.Session.SetString("CaptchaHash", hash);

            var rnd = new Random();
            var fonts = new[] { "Narkisim,Arial" };
            float orientationAngle = rnd.Next(0, 359);
            float y = 0;
            Brush brush = new SolidBrush(Color.FromArgb(255, 56, 62, 167));
            Brush brush1 = new SolidBrush(Color.White);
            Font font;
            font = new Font("Terminal", 16);
            const int height = 40;
            const int width = 100;
            var index0 = rnd.Next(0, fonts.Length);
            var familyName = fonts[index0];

            using (var bmpOut = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(bmpOut);

                /* if (set == 1)
                 {
                     var gradientBrush = new LinearGradientBrush(new Rectangle(0, 0, width, height),

                          Color.FromArgb(238, 240, 241, 240), Color.FromArgb(238, 240, 241, 240)

                     , orientationAngle);

                     g.FillRectangle(gradientBrush, 0, 0, width, height);
                 }
                */

                var gradientBrush = new LinearGradientBrush(new Rectangle(0, 0, width, height),

                        Color.White, Color.White

                   , orientationAngle);

                g.FillRectangle(gradientBrush, 0, 0, width, height);

                //DrawRandomLines(ref g, width, height);              
                float x = 0;
                //g.SmoothingMode = SmoothingMode.AntiAlias;
                //  g.TextRenderingHint = TextRenderingHint.AntiAlias;
                font = new Font("Constantia,Tahoma", 30, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.World);
                //g.DrawString(randomText, font, brush, x,-10);

                var rec = new Rectangle(-30, 4, width, height);
                var path = new GraphicsPath();
                path.AddString(randomText, font.FontFamily, (int)FontStyle.Italic | (int)FontStyle.Bold, 30, new Point(-5, 10), StringFormat.GenericTypographic);

                // Determine physical size of the character when rendered
                var area = Rectangle.Round(path.GetBounds());

                // Slide it to be centered in the specified bounds
                var offset = new Point(rec.Left + (rec.Width / 2 - area.Width / 2) - area.Left, rec.Top + (rec.Height / 2 - area.Height / 2) - area.Top);
                var translate = new Matrix();
                translate.Translate(offset.X, offset.Y);
                translate.Rotate(-10, MatrixOrder.Append);
                translate.Shear(1, 0, MatrixOrder.Append);

                path.Transform(translate);

                // Now render it however desired
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.FillPath(SystemBrushes.ControlText, path);


                for (var i = 0; i < width; i += 2)
                {
                    for (var j = 0; j < height; j += 2)
                    {
                        var offset1 = Math.Abs(height / 2 - j);
                        if (i + offset1 < width)
                        {
                            var pixel = bmpOut.GetPixel(i + offset1, j);
                            bmpOut.SetPixel(i, j, pixel);
                        }
                    }
                }

                var ms = new MemoryStream();
                bmpOut.Save(ms, ImageFormat.Png);
                var bmpBytes = ms.GetBuffer();
                bmpOut.Dispose();
                ms.Close();

                return new FileContentResult(bmpBytes, "image/png");
            }
        }
        public ActionResult Captcha(int set = 0)
        {
            ViewBag.Set = set;
            return PartialView();
        }
        private HttpContext str;
        public static bool IsValidCaptchaValue(string Captcha, HttpContext str)
        {
            var expectedHash = str.Session.GetString("CaptchaHash");
            var toCheck = Captcha + GetSalt();
            var hash = ComputeMd5Hash(toCheck);
            return hash.Equals(expectedHash);
        }

        private static void DrawRandomLines(ref Graphics g, int width, int height)
        {
            var rnd = new Random();
            var pen = new Pen(Color.Gray);
            for (var i = 0; i < 10; i++)
            {
                g.DrawLine(pen, rnd.Next(0, width), rnd.Next(0, height),
                                rnd.Next(0, width), rnd.Next(0, height));
            }
        }

        public static string GetSalt()
        {
            return typeof(DataCaptchaController).Assembly.FullName;
        }

        public static string ComputeMd5Hash(string input)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(input);
            HashAlgorithm md5Hasher = MD5.Create();
            return BitConverter.ToString(md5Hasher.ComputeHash(bytes));
        }

        public static string GenerateRandomText(int textLength)
        {
            const string chars = "abcdefjiclmnopqrstuvwxvz";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, textLength)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }

    }
}
