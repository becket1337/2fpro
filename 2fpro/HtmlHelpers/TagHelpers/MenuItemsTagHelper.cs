using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.HtmlHelpers.TagHelpers
{

    [HtmlTargetElement("navigation", TagStructure = TagStructure.WithoutEndTag)]
    public class MenuItemsTagHelper : TagHelper
    {

        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public MenuViewModel Info { get; set; }

        private HttpRequest Request => ViewContext.HttpContext.Request;
        private IMenuRepository _menuRep;

        public MenuItemsTagHelper(IMenuRepository menuRep)
        {
            _menuRep = menuRep;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "ul";
            var content = await output.GetChildContentAsync();
            var target = content.GetContent();
            output.Attributes.SetAttribute("data-uniqueid", context.UniqueId);
            output.Attributes.SetAttribute("id", "section-navigation__top");
            //output.Content.SetHtmlContent("dd");



            foreach (var menuItem in Info.Menues.Where(x => x.MenuSection == Info.MenuSection).OrderBy(x => x.SortOrder))
            {
                output.Content.AppendHtml($@"<li>{menuItem.Text}</li>");


            }

            //output.PreContent.SetHtmlContent("<section>");
            //output.PostContent.SetHtmlContent("</section>");
            //output.TagMode = TagMode.SelfClosing;
        }
    }
}
