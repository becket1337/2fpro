using _2fpro.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Wangkanai.Detection;

namespace _2fpro.Controllers
{

    public class ViewSwitcherController : Controller
    {
        private readonly IDetection _detection;
        public ViewSwitcherController(IDetection detection)
        {
            _detection = detection;
        }
        public RedirectResult SwitchView(bool mobile, string returnUrl)
        {
            //if (SiteHelpFunctions.IsMobileBrowser(Request))
            //    HttpContext.();
            //else
            //    HttpContext.SetOverriddenBrowser(mobile ? BrowserOverride.Mobile : BrowserOverride.Desktop);

            return Redirect(returnUrl);
        }
    }
}
