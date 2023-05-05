using System;
using System.Threading.Tasks;
using _2fpro.Models;
using _2fpro.Service.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using _2fpro.ViewModel;

namespace _2fpro.Areas.Admin.Controllers
{

    public class FeedbackController : Controller
    {
        //
        // GET: /Admin/Feedback/

        IEmailService _emailSender;

        public FeedbackController(IEmailService emailSender)
        {
            _emailSender = emailSender;
        }
        //[HttpPost]
        //public JsonResult ValidCaptcha(string Captcha)
        //{

        //    var isValidCaptcha = DataCaptchaController.IsValidCaptchaValue(Captcha,HttpContext);



        //    if (!isValidCaptcha)
        //    {
        //        return Json(Captcha == null);

        //    }
        //    return Json(true);
        //}

        public ActionResult GetIndex()
        {


            return ViewComponent("Feedback");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetIndex(FeedbackViewModel feedback)
        {

            System.Threading.Thread.Sleep(500);
            if (ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(feedback.Name + " - " + feedback.PhoneNumber, feedback.Text, "Вопрос с сайта 2fpro.ru!", "", false);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {

                    return Json("Сообщение отправлено!");
                }
                throw new ArgumentException();
                //return Redirect("/");
            }
            throw new ArgumentException();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Send(Feedback feed)
        //{
        //    //throw new HttpException();
        //    System.Threading.Thread.Sleep(1500);
        //    /*var isValidCaptcha = DataCaptchaController.IsValidCaptchaValue(feed.Captcha);


        //    if (!isValidCaptcha)
        //    {
        //        throw new HttpException();
        //    }*/

        //    if (ModelState.IsValid)
        //    {
        //        await _emailSender.SendEmailAsync(feed.Email,"Письмо с сайта!", feed.Text);

        //        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        //        {

        //            return PartialView("Feedbacksend");
        //        }
        //        return Redirect("/");
        //    }
        //    throw new Exception();
        //}


    }
}
