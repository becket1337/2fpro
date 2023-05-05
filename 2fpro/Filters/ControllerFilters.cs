using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace _2fpro.Filters
{
    public class ControllerFilters
    {


    }

    public class DisableController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            throw new System.Exception();
        }
    }
}