using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _2fpro.HtmlHelpers
{
    public static class CategoryHelper
    {
        public static IHtmlContent GetCats(this IHtmlHelper helper, List<Category> catsList)
        {

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("catlist");
            int count = 0;
            foreach (var item in catsList)
            {
                count++;
                string catName = item.CategoryName.IndexOf("&") != 0 ?
                     item.CategoryName.Replace(" & ", "-") :
                     item.CategoryName.Replace(" ", "-");
                TagBuilder liTag = new TagBuilder("li");
                if (count == catsList.Count()) { liTag.AddCssClass("last"); }
                TagBuilder aTag = new TagBuilder("a");
                aTag.MergeAttribute("href", "/" + catName);

                aTag.AddCssClass("cat-link");
                aTag.InnerHtml.Append(item.CategoryName);
                liTag.InnerHtml.SetContent(aTag.ToString());
                ulTag.InnerHtml.SetContent(liTag.ToString());

            }

            //if (catsList.Count() != 0)
            //{
            return ulTag.RenderBody();
            //}
            //else return MvcHtmlString.Empty;

        }

    }
}