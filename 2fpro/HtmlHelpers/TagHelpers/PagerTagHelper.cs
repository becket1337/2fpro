using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unidecode.NET;

namespace _2fpro.HtmlHelpers.TagHelpers
{


    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        //private IFileManager _manager;
        //public PagerTagHelper(IFileManager manager) {
        //    _manager = manager;
        //}


        public PagingInfo Info { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = Info.CurrentPage;
            var totalPages = Info.TotalPage;

            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            //var content = await output.GetChildContentAsync();
            //var target = content.GetContent();
            var infoService = Info.Service;

            output.TagName = "section";
            output.Attributes.SetAttribute("data-uniqueid", context.UniqueId);
            output.Attributes.SetAttribute("id", "section-pager");

            for (int i = 1; i <= totalPages; i++)
            {

                string resultUrl = null;
                if (infoService == "prod") resultUrl = Info.PageUrl == null ? Info.PageUrlWithParam2(i, Info.CatId, Info.catname.Unidecode()) : Info.PageUrl(i); // href
                else if (infoService == "order") resultUrl = Info.PageUrl == null ? Info.PageUrlWithParam(i, Info.Sort) : Info.PageUrl(i); // href
                else resultUrl = Info.PageUrl == null ? Info.PageUrlWithParam(i, Info.Dir.Name) : Info.PageUrl(i);                             //

                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {

                    if (Info.Condition)
                    { // ajax attributes
                        if (i == currentPage)
                        {
                            if (infoService == "prod") output.Content.AppendHtml(
                                     $@"<a href=""javascript: void(0);"" class=""current-page""  data-catname=""{Info.catname.Unidecode()}"" data-catId=""{ Info.CatId}"" data-page=""{i}"" data-service=""{Info.Service}"">{i.ToString()}</a>");
                            else
                                output.Content.AppendHtml(
                                 $@"<a href=""javascript: void(0);""  class=""current-page k-primary k-button"" data-folder=""{Info.Dir.Name}"" data-sort=""{Info.Sort}"" data-page=""{i}"" data-service=""{Info.Service}"">{i.ToString()}</a>");
                        }
                        else
                        {
                            if (infoService == "prod") output.Content.AppendHtml(
                                    $@"<a href=""javascript: void(0);""  class=""""  data-catname=""{Info.catname.Unidecode()}"" data-catId=""{ Info.CatId}"" data-page=""{i}"" data-service=""{Info.Service}"">{i.ToString()}</a>");
                            else
                                output.Content.AppendHtml(
                                 $@"<a  href=""javascript: void(0);"" class="" k-button"" data-folder=""{Info.Dir.Name}""  data-sort=""{Info.Sort}"" data-page=""{i}"" data-service=""{Info.Service}"">{i.ToString()}</a>");
                        }

                    }
                    else // no ajax attributes
                    {

                        if (i == currentPage)
                        {
                            output.Content.AppendHtml(
                                 $@"<a class=""current-page k-primary k-button"" href=""{resultUrl}"">{i.ToString()}</a>"
                                 );
                        }
                        else
                        {
                            output.Content.AppendHtml(
                                $@"<a  class=""k-button"" href=""{resultUrl}"">{i.ToString()}</a>"
                                );
                        }

                    }
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            output.Content.AppendHtml("<span>...</span>");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            output.Content.AppendHtml("<span>...</span>");
                            bThreeDots2 = true;
                        }
                    }
                }
            }

            if (totalPages <= 1)
            {
                output.SuppressOutput();

            }

        }
    }
}
