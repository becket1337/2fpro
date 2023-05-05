
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using System.Xml.Linq;
using System.Xml.Serialization;
using _2fpro.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2fpro.Areas.Admin.Controllers
{
    public class WebParserController : Controller
    {
        // GET: Admin/WebParser       
        public string brandUrl = "http://www.komus.ru/product/493556/#features";
        public string catUrl = "http://www.komus.ru/catalog/5361/_s/page/1/";
        public List<WebParserModel> CartridgeList;

        public WebParserController()
        {
            CartridgeList = new List<WebParserModel>();
        }
        [RequestSizeLimit(10_000_000)]
        public ActionResult Index()
        {


            //PushCartridgeList(18);

            //for (var i = 1; i < PagesCount(); i++)
            //{
            //    PushCartridgeList(i);
            //}
            //ViewBag.result = CartridgeList.Count();
            //XmlSerializeObject();


            //IWorkbook workbook = new XSSFWorkbook();
            //var xssfWorkbook = workbook as NPOI.XSSF.UserModel.XSSFWorkbook;
            //POIXMLProperties xmlProps = xssfWorkbook.GetProperties();
            //xmlProps.CoreProperties.Creator="Timur";
            //var sheet = xssfWorkbook.CreateSheet("Sheet A1");

            //var name = xssfWorkbook.CreateName();
            //name.NameName = "name";

            //var row = sheet.CreateRow(0);
            //row.CreateCell(0).SetCellValue("value1");
            //row.CreateCell(1).SetCellValue("value2");
            //row.CreateCell(2).SetCellValue("value3");
            //FileStream sw = new FileStream(Server.MapPath("~/test.xlsx"), FileMode.Create);
            //workbook.Write(sw);

            //sw.Close();

            return View();
        }

        public string GetHtmlString(int id = 1)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                wc.Proxy = null;
                var result = wc.DownloadString("http://www.komus.ru/catalog/5361/_s/page/" + id);

                return result;
            }
        }
        public void PushCartridgeList(int pageId = 1)
        {
            HtmlDocument html = new HtmlDocument();


            var result = GetHtmlString(pageId);

            html.LoadHtml(result);

            var list = html.GetElementbyId("page_js")
                .ChildNodes.FindFirst("div")
                .ChildNodes.Where(x => x.HasChildNodes).ToArray()[2]//.Where(x=>x.Attributes["class"].Value=="page--middle").Count();
                .ChildNodes.Where(x => x.HasChildNodes).ToArray()[0]//.Attributes["class"].Value;
                .ChildNodes.FindFirst("div")
                .ChildNodes.Where(x => x.HasChildNodes).ToArray()[3]//.Attributes["class"].Value;
                .ChildNodes.FindFirst("div")
                .ChildNodes.Where(x => x.HasChildNodes).ToArray();


            foreach (var item in list)
            {

                var currFolder = item.ChildNodes.FindFirst("div").ChildNodes.FindFirst("div");
                var infoFolder = currFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes.Where(x => x.HasChildNodes).ToArray()[1];
                var moreFolder = currFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes.Where(x => x.HasChildNodes).ToArray()[2].ChildNodes.Where(x => x.Name == "div").ToArray()[1];
                bool countryExists = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[2].ChildNodes.Where(x => x.Name == "span").ToArray()[0].InnerText == "Страна происхождения:" ? true : false;
                bool fieldsExists = moreFolder.Elements("div").Count() <= 5 ? true : false;
                //ViewBag.result = moreFolder.Elements("div").Count(); return;
                //var traceSource = new TraceSource("Tracing");
                //traceSource.TraceEvent(TraceEventType.Warning, 0, infoFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[1].ChildNodes[1].InnerText);
                //traceSource.TraceEvent(TraceEventType.Warning, 0, moreFolder.ChildNodes[1].ChildNodes[3].InnerText);
                //traceSource.TraceEvent(TraceEventType.Warning, 0, moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[3].InnerText);
                //traceSource.TraceEvent(TraceEventType.Warning, 0, moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[4].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText);
                //traceSource.TraceEvent(TraceEventType.Warning, 0, moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[3].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText);
                //traceSource.TraceEvent(TraceEventType.Warning, 0, moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[2].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText);
                //return;
                if (fieldsExists)
                {

                }
                else if (countryExists)
                {
                    CartridgeList.Add(new WebParserModel()
                    {
                        Referense = infoFolder.ChildNodes.FindFirst("div").InnerText.Trim().Replace("&nbsp;", " "),
                        ImageUrl = currFolder.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["href"].Value,
                        Price = infoFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[1].ChildNodes[1].InnerText,
                        Color = moreFolder.ChildNodes[1].ChildNodes[3].InnerText,
                        СartridgeLength = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[3].InnerText,
                        ModelType = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[5].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText,
                        PrintType = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[4].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText,
                        Type = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[3].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText


                    });
                }
                else
                {
                    CartridgeList.Add(new WebParserModel()
                    {
                        Referense = infoFolder.ChildNodes.FindFirst("div").InnerText.Trim().Replace("&nbsp;", " "),
                        ImageUrl = currFolder.ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[1].Attributes["href"].Value,
                        Price = infoFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[1].ChildNodes[1].InnerText,
                        Color = moreFolder.ChildNodes[1].ChildNodes[3].InnerText,
                        СartridgeLength = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[1].ChildNodes[3].InnerText,
                        ModelType = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[4].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText,
                        PrintType = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[3].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText,
                        Type = moreFolder.ChildNodes.Where(x => x.HasChildNodes).ToArray()[2].ChildNodes.Where(x => x.Name == "span").ToArray()[1].InnerText


                    });
                }

            }

            CartridgeList.ForEach(x => GetBrand(x));

            //ViewBag.length = JObject.FromObject(new { items = CartridgeList })["items"][0].ToString();
        }

        public int PagesCount()
        {

            var outer = GetHtmlString();
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(outer);

            var wrap = html.GetElementbyId("breadcrumb_js");
            int result = 0;
            result = Int32.Parse(wrap.ParentNode.Elements("div").Where(x => x.Attributes["class"].Value == "pagination").First().ChildNodes[1].Elements("span").Last().PreviousSibling.PreviousSibling.ChildNodes[1].InnerText);
            return result;
        }

        public void GetBrand(WebParserModel md)
        {
            WebRequest wr = WebRequest.Create("http://www.komus.ru/product/" + md.Referense.Substring(5) + "/#features");
            wr.Proxy = null;
            var resp = wr.GetResponse();
            var result = "";
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(result);

            var outer = html.GetElementbyId("breadcrumb_js");
            var wrapper = outer.NextSibling.NextSibling.NextSibling.NextSibling;
            var bigImg = wrapper.ChildNodes.FindFirst("img").Attributes["src"].Value;
            var cat = "";
            md.Title = wrapper.ChildNodes[3].ChildNodes.FindFirst("h1").InnerText.Trim(); // title
            md.ImageUrl = bigImg; // img url
            outer = html.GetElementbyId("tabs--content-item-features");
            var tableLinesCount = outer.ChildNodes.FindFirst("table").Elements("tr").Last().ChildNodes[3].InnerText.Trim();
            if (tableLinesCount == "&nbsp;")
            {
                cat = outer.ChildNodes.FindFirst("table").Elements("tr").Last().PreviousSibling.PreviousSibling.ChildNodes[3].InnerHtml;
                md.Brand = Regex.Match(cat, @"\w+(?=(</span>))").Value;
            }
            else
            {
                cat = tableLinesCount;
                md.Brand = cat;
            }

            //ViewBag.outer = bigImg;

        }

        public void XmlSerializeObject()
        {
            var path = "~/report.xml";
            using (TextWriter output = new StreamWriter(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<WebParserModel>));
                serializer.Serialize(output, CartridgeList);
            }
        }

        //public List<WebParserModel> CartridgeList { get { return _CartridgeList; } set { _CartridgeList = value; } }



    }
}