using _2fpro.Extension;

using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Fonts;

namespace _2fpro.Service.Repository
{
    public class FileManager : IFileManager
    {

        private IHostingEnvironment _env;
        private readonly _2fpro.Models.SiteConfig _siteSettings;

        public FileManager() { }

        public FileManager(IHostingEnvironment env, IOptionsMonitor<_2fpro.Models.SiteConfig> siteSettings)
        {

            _env = env;
            _siteSettings = siteSettings.CurrentValue;
        }

        private byte[] istream = null;
        private string filePath = null;


        private static Random random = new Random();
        public string mainDir = "Content/Files/Pages";

        public void WriteFiles(IEnumerable<IFormFile> files, string folder)
        {
            var path = Path.Combine(_env.WebRootPath, mainDir + "/" + folder + "/");
            foreach (var file in files)
            {
                if (file.Length > 5000000) throw new Exception();

                CheckDirectory(path);

                var filename = ValidateFile(file, folder);
                filePath = Path.Combine(path, filename);

                if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                {
                    using (Image<Rgba32> image = Image.Load(file.OpenReadStream()))
                    {
                        if ((image.Height > 2000 || image.Width > 2000))
                        {
                            image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                //Position = AnchorPositionMode.Center,
                                Size = new Size(image.Width / 2, image.Height / 2)
                            }));
                        }
                        image.Save(filePath); // Automatic encoder selected based on extension.


                    }

                }
                else
                {
                    istream = new BinaryReader(file.OpenReadStream()).ReadBytes((int)file.Length);

                    using (MemoryStream ms = new MemoryStream(istream))
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {

                            ms.WriteTo(fs);
                        }
                    }
                }
            }
        }
        public void WriteFile(IFormFile file, string path)
        {
            if (file.Length > 5000000) throw new Exception();

            CheckDirectory(path);

            filePath = Path.Combine(path, GetRandomName() + Path.GetExtension(file.FileName));

            if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
            {
                using (Image<Rgba32> image = Image.Load(file.OpenReadStream()))
                {
                    if ((image.Height > 1900 || image.Width > 1900))
                    {
                        image.Mutate(x => x
                            .Resize(new ResizeOptions
                            {
                                Mode = ResizeMode.Max,
                                //Position = AnchorPositionMode.Center,
                                Size = new Size(image.Width / 2, image.Height / 2)
                            }));
                        image.Save(filePath); // Automatic encoder selected based on extension.

                    }
                    else
                    {
                        image.Save(filePath);
                    }
                }
            }
            else
            {
                istream = new BinaryReader(file.OpenReadStream()).ReadBytes((int)file.Length);

                using (MemoryStream ms = new MemoryStream(istream))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {

                        ms.WriteTo(fs);
                    }
                }
            }
        }
        public async Task WriteImage(IFormFile file, string path, bool isWM = false, int width = 0, int height = 0)
        {


            //var sn = _siteSettings.SiteName;


            //watermark


            using (Image<Rgba32> image = Image.Load(file.OpenReadStream()))
            {
                if (width == 0 && height == 0 &&  image.Width > 2000)
                {


                    image.Mutate(x => x
                        .Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            //Position = AnchorPositionMode.Center,
                            Size = new Size(1920, 0)
                        }));

                    if (isWM) SetTextTest(image);

                    image.Save(path); // Automatic encoder selected based on extension.

                }
                else if (width == 0 && height == 0)
                {
                    if (isWM) { SetTextTest(image); }
                    image.Save(path);
                }
                else
                {

                    image.Mutate(x => x
                      .Resize(new ResizeOptions
                      {
                          Mode = ResizeMode.Max,
                          

                          Size = new Size(width, 0)
                      })

                      );
                    if (isWM) SetTextTest(image);
                    image.Save(path);
                }

            }


        }
        private void SetTextTest(Image<Rgba32> image)
        {
            string text = _siteSettings.SiteName;
            Font font = new Font(SystemFonts.Find("verdana"), 10, FontStyle.Bold);
            SizeF size = TextMeasurer.Measure(text, new RendererOptions(font));

            float scalingFactor = Math.Min(image.Width / size.Width, image.Height / size.Height); //image 5400 = 128
            var scaledFont = new Font(font, scalingFactor * font.Size / 6);

            System.Numerics.Vector2 vect = new System.Numerics.Vector2(50, image.Height - 100);
            System.Numerics.Vector2 vect1 = new System.Numerics.Vector2(50 + 5, image.Height - 100);

            image.Mutate(x => x.DrawText(new TextGraphicsOptions(true)
            {
                BlendPercentage = 50,
                ApplyKerning = true
            }, text, scaledFont, Rgba32.FromHex("00000060"), vect));

            image.Mutate(x => x.DrawText(new TextGraphicsOptions(true)
            {
                BlendPercentage = 50,
                ApplyKerning = true
            }, text, scaledFont, Rgba32.FromHex("ffffff60"), vect1));
        }

        public void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

            }
            //else
            //{

            //    if (Directory.GetFiles(path).Count() == 0) return 1;
            //    else
            //    {

            //        return Directory.GetFiles(path).Count() + 1;


            //    }


            //}

        }
        public string GetRandomName()
        {

            var str = Path.GetRandomFileName();

            string res = null;
            foreach (char ch in str)
            {
                if (!Char.IsLetter(ch)) res += RandomString(2);
                else res += ch;

            }


            return res;
        }
        public void RemoveDir(string path)
        {
            if (Directory.Exists(path) && path != null)
            {
                Directory.Delete(path, true);
            }
        }

        public void RemoveFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string RandomString(int length)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // DIRECTORIES

        public void CreateDir(string path)
        {

            if (Directory.Exists(path))
            {
                DirectoryInfo di = Directory.CreateDirectory(path + "1");
            }
            else { DirectoryInfo di = Directory.CreateDirectory(path); }

            DirectoryInfo dir = new DirectoryInfo(path);

            //dir.SetAccessControl(security);
        }
        public IEnumerable<DirectoryInfo> GetDirs()
        {
            var path = Path.Combine(_env.WebRootPath, mainDir);
            if (!Directory.Exists(path)) CheckDirectory(path);
            DirectoryInfo dinfo = new DirectoryInfo(path);
            var dirs = dinfo.GetDirectories();
            return dirs;
        }

        public DirectoryInfo GetDir(string folder)
        {
            var path = Path.Combine(_env.WebRootPath, mainDir + "/" + folder);
            DirectoryInfo dinfo = new DirectoryInfo(path);
            return dinfo;
        }
        public void ClearDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                File.Delete(file.FullName);
            }
        }
        public byte[] ConvertToBytes(IFormFile image)
        {
            byte[] CoverImageBytes = null;
            BinaryReader reader = new BinaryReader(image.OpenReadStream());
            CoverImageBytes = reader.ReadBytes((int)image.Length);
            return CoverImageBytes;
        }

        public string ValidateFile(IFormFile file, string folder)
        {

            var files = new DirectoryInfo(Path.Combine(_env.WebRootPath, mainDir + "/" + folder)).GetFiles();
            var filename = WebUtility.HtmlEncode(file.FileName);
            var filenameNoext = Path.GetFileNameWithoutExtension(filename);
            if (files.Any(x => x.Name == filename))
            {
                for (var i = 1; i <= 10; i++)
                {
                    var dublicateFilename = filenameNoext + "(" + i + ")" + Path.GetExtension(filename);
                    if (files.FirstOrDefault(x => x.Name == dublicateFilename) == null)
                        return dublicateFilename;


                }
            }

            return filename;
        }
    }
}
