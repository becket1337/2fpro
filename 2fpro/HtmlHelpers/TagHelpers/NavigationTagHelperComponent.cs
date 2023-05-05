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

    public class NavigationTagHelperComponent : TagHelperComponent
    {

        //private IMenuRepository _menuRep;
        //private HttpRequest Request => ViewContext.HttpContext.Request;
        //private int _viewModel;
        //[ViewContext]
        //public ViewContext ViewContext { get; set; }
        //public override int Order => 1;



        //public NavigationTagHelperComponent(IMenuRepository menuRep)
        //{
        //    _menuRep = menuRep;
        //   // _viewModel = viewModel;
        //    //Order = order;
        //}

        //public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    if (string.Equals(context.TagName, "website",
        //       StringComparison.OrdinalIgnoreCase))
        //    {

        //        output.TagName = "ol";

        //        output.Attributes.SetAttribute("data-uniqueid", context.UniqueId);
        //        output.Attributes.SetAttribute("id", "section-navigation__top");
        //        output.Content.SetContent(_menuRep.Menues.Count().ToString());

        //        foreach (var menuItem in _menuRep.Menues.Where(x => x.MenuSection == _viewModel).OrderBy(x => x.SortOrder))
        //        {
        //            output.Content.AppendHtml($@"<li>{menuItem.Text}</li>");


        //        }

        //        output.PreContent.SetHtmlContent("<section>");
        //        output.PostContent.SetHtmlContent("</section>");
        //        output.TagMode = TagMode.SelfClosing;
        //    }

        //}

    }
}
