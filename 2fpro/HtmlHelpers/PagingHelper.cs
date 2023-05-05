using System;

using System.Text;

using _2fpro.ViewModel;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace _2fpro.HtmlHelpers
{
    public static class PagingHelper
    {
        public static IHtmlContent PageLinks(this IHtmlHelper html, PagingInfo pageingInfo, Func<int, string> pageUrl)
        {
            //StringBuilder str = new StringBuilder();
            var str1 = new TagBuilder("div");

            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = pageingInfo.CurrentPage;
            var totalPages = pageingInfo.TotalPage;


            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            for (int i = 1; i <= totalPages; i++)
            {
                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {
                    TagBuilder tag = new TagBuilder("a");

                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml.AppendHtml(i.ToString());


                    if (i == currentPage)
                        tag.AddCssClass("current-page k-primary k-button");
                    else
                        tag.AddCssClass("k-button");

                    str1.InnerHtml.AppendHtml(tag.ToString());
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            str1.InnerHtml.AppendHtml("...");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            str1.InnerHtml.AppendHtml("...");
                            bThreeDots2 = true;
                        }
                    }
                }
            }
            if (pageingInfo.TotalPage > 1)
            {

                return str1;
            }
            else return str1;
        }

        public static IHtmlContent AjaxPageLinks(this IHtmlHelper html, PagingInfo pageingInfo)
        {
            StringBuilder str = new StringBuilder();
            var str1 = new HtmlContentBuilder();

            bool bThreeDots1 = false;
            bool bThreeDots2 = false;
            int links_visible = 2;
            var currentPage = pageingInfo.CurrentPage;
            var totalPages = pageingInfo.TotalPage;


            int links_visible_head = links_visible;
            if (links_visible >= (currentPage - links_visible)) links_visible_head = 3;
            int links_visible_tail = links_visible;
            if ((currentPage + links_visible) >= (totalPages - links_visible))
                links_visible_tail = links_visible * 2 + 1;

            for (int i = 1; i <= totalPages; i++)
            {
                if (i <= links_visible_head
                    || i > (totalPages - links_visible_tail)
                    || (i <= currentPage && i >= (currentPage - links_visible))
                    || (i >= currentPage && i <= (currentPage + links_visible)))
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", "javascript:void()");
                    tag.InnerHtml.SetContent(i.ToString());
                    tag.MergeAttribute("data-service", pageingInfo.Service);
                    tag.MergeAttribute("data-page", i.ToString());
                    if (pageingInfo.CatId != null)
                    {
                        tag.MergeAttribute("data-catid", pageingInfo.CatId.ToString());
                    }
                    tag.AddCssClass("page-link k-button");

                    if (i == currentPage)
                        tag.AddCssClass("current-page k-button k-primary");

                    str.Append(tag.ToString());
                }
                else
                {
                    if (i < currentPage)
                    {
                        if (!bThreeDots1)
                        {
                            str.Append("...");
                            bThreeDots1 = true;
                        }
                    }
                    else
                    {
                        if (!bThreeDots2)
                        {
                            str.Append("...");
                            bThreeDots2 = true;
                        }
                    }
                }
            }
            if (pageingInfo.TotalPage > 1)
            {
                return str1.SetContent(str.ToString());
            }
            else return str1;
        }
    }



}