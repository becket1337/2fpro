using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.Areas.Admin.Controllers;
using _2fpro.Models;

namespace _2fpro.HtmlHelpers
{
    public static class animatedMenuHelper
    {
        /* public static MvcHtmlString SiteMenuAsUnorderedList(this HtmlHelper helper, List<Menu> siteLinks, bool isFooter = false)
         {
             var CurrentController = (string)helper.ViewContext.RouteData.Values["Controller"];
             var CurrentAction = (string)helper.ViewContext.RouteData.Values["Action"];
             if (siteLinks == null)
                 return MvcHtmlString.Empty;
             var topLevelParentId = SiteLinkListHelper.GetTopLevelParentId(siteLinks);
             return MvcHtmlString.Create(buildMenuItems(siteLinks, topLevelParentId, CurrentController, isFooter));
         }

         private static string buildMenuItems(List<Menu> siteLinks, int parentId, string controllerName, bool isFooter = false)
         {
             if (isFooter)
             {

                 var ptag = new TagBuilder("ul");
                 ptag.MergeAttribute("class", "foot-nav");

                 var links = SiteLinkListHelper.GetChildSiteLinks(siteLinks, 0);
                 foreach (var siteLink in links)
                 {
                     var itag = new TagBuilder("li");
                     var ancht = new TagBuilder("a");
                     ancht.MergeAttribute("href", "/" + siteLink.Url);
                     ancht.SetInnerText(siteLink.Text);
                     itag.InnerHtml = ancht.ToString();
                     ptag.InnerHtml += itag;
                 }

                 return ptag.ToString();
             }

             var currentUrl = string.Format("{0}", controllerName);
             var parentTag = new TagBuilder("ul");
             if (parentId == 0 && siteLinks.Any(x => x.MenuSection == 1))
             {
                 parentTag.MergeAttribute("class", "treeview");
                 parentTag.MergeAttribute("id", "tree");
             }
             else
             {
                 parentTag.MergeAttribute("class", "nav full height reverse");
             }
             var childSiteLinks = SiteLinkListHelper.GetChildSiteLinks(siteLinks, parentId);
             foreach (var siteLink in childSiteLinks)
             {
                 var itemTag = new TagBuilder("li");
                 itemTag.AddCssClass("m-item");
                 var spanTag = new TagBuilder("span");
                 spanTag.MergeAttribute("data-title", siteLink.Text);
                 var anchorTag = new TagBuilder("a");
                 anchorTag.AddCssClass("i-link");
                 if (currentUrl == siteLink.Url)
                 {
                     anchorTag.MergeAttribute("class", "current");
                 }

                 var newUrl = "/" + siteLink.Url;
                 string concatUrl = null;
                 if (siteLink.ParentId > 0)
                 {
                     //concatUrl = MenuController.GetMenuUrl(siteLink.ParentId) + newUrl;
                     //anchorTag.MergeAttribute("href", concatUrl);
                 }
                 else
                 {
                     if (siteLink.Url == "Home") anchorTag.MergeAttribute("href", "/");
                     else anchorTag.MergeAttribute("href", newUrl);
                 }

                 spanTag.SetInnerText(siteLink.Text);
                 anchorTag.InnerHtml = spanTag.ToString();
                 itemTag.InnerHtml = anchorTag.ToString();
                 TagBuilder subMenu = new TagBuilder("div"); ;
                 if (SiteLinkListHelper.SiteLinkHasChildren(siteLinks, siteLink.Id))
                 {
                     //itemTag.InnerHtml += buildMenuItems(siteLinks, siteLink.Id, controllerName);

                     var subOuter = new TagBuilder("div");
                     var triang = new TagBuilder("div");
                     var subLinks = new TagBuilder("div");
                     subLinks.AddCssClass("sub-links-wrap");
                     subOuter.AddCssClass("sub-outer");
                     triang.AddCssClass("triangle");
                     subMenu.AddCssClass("sub-menu");
                     foreach (var subLink in siteLinks.Where(x => x.ParentId == siteLink.Id))
                     {
                         var subItem = new TagBuilder("div");
                         var subAnch = new TagBuilder("a");

                         var subUrl = "/" + subLink.Url;
                         string subConcatUrl = null;
                         subConcatUrl = "/" + MenuController.GetMenuUrl(subLink.ParentId) + subUrl;
                         subAnch.MergeAttribute("href", subConcatUrl);
                         subAnch.SetInnerText(subLink.Text);
                         subItem.AddCssClass("sub-item");
                         subItem.InnerHtml = subAnch.ToString();
                         subLinks.InnerHtml += subItem.ToString();

                     }

                     subMenu.InnerHtml = subOuter.ToString() + triang.ToString() + subLinks.ToString();
                 }



                 if (siteLink.Url == "catalog") subMenu.MergeAttribute("id", "toleft");
                 if (subMenu.InnerHtml != "") parentTag.InnerHtml += itemTag.ToString() + subMenu.ToString();
                 else parentTag.InnerHtml += itemTag;
             }
             return parentTag.ToString();
         }*/
    }
}