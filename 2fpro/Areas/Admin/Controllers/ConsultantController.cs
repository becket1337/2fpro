//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Web;
//using System.Web.Mvc;
//using _2fpro.Data;
//using _2fpro.Models;
//using _2fpro.Service.Interface;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;

//namespace _2fpro.Areas.Admin
//{
//    public class ConsultantController : Controller
//    {
//        //
//        // GET: /Admin/LiveChat/
//        private readonly ApplicationDbContext _db;
//        IEmailSender _emailSender;
//        public ConsultantController(IEmailSender emailSender, ApplicationDbContext db)
//        {
//            _emailSender = emailSender;
//            _db = db;
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public JsonResult AllowUser(LiveUser user, string connId)
//        {



//            if (User.IsInRole("admin"))
//            {
//                var user2 = _db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
//                user2.ConnId = connId;
//                user2.IsOnline = true;
//                _db.SaveChanges();
//                return Json(new { usersOnline = db.LiveUsers.Count() - 1, id = connId });
//            }

//            if (ModelState.IsValid)
//            {
//                if (user.UserName == null || user.Email == null)
//                {
//                    throw new Exception();
//                }
//                System.Threading.Thread.Sleep(2500);

//                LiveUser user1 = null;

//                user1 = new LiveUser();
//                user1.FeedMessage = "txt";
//                user1.UserName = user.UserName;
//                user1.Email = user.Email;
//                user1.IsAdmin = false;
//                user1.IsOnline = true;
//                user1.ConnId = connId;
//                //user1.LiveMessages = new List<Message>();
//                _db.LiveUsers.Add(user1);
//                _db.SaveChanges();


//                HttpCookie cookie = Request.Cookies["_LiveChat-username"];
//                HttpCookie cookie1 = Request.Cookies["_LiveChat-email"];

//                if (cookie != null && cookie1 != null)
//                {
//                    cookie.Value = user.UserName;
//                    cookie1.Value = user.Email;
//                    Response.Cookies.Add(cookie);
//                    Response.Cookies.Add(cookie1);
//                }
//                else
//                {
//                    cookie = new HttpCookie("_LiveChat-username");
//                    cookie.Value = user.UserName;
//                    cookie.Expires = DateTime.Now.AddYears(1);
//                    cookie1 = new HttpCookie("_LiveChat-email");
//                    cookie1.Value = user.Email;
//                    cookie1.Expires = DateTime.Now.AddYears(1);

//                    Response.Cookies.Add(cookie);
//                    Response.Cookies.Add(cookie1);
//                }
//                return Json(new { Name = user.UserName, Email = user.Email });
//            }
//            throw new HttpException();
//        }

//        public JsonResult GetUserRoom(string conn)
//        {

//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var user = db.LiveUsers.SingleOrDefault(x => x.ConnId == conn);
//                var user1 = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
//                user.LiveMessages.ToList().ForEach(x => x.Visited = true);
//                user1.GroupId = conn;
//                db.SaveChanges();
//                var newQuestions = db.LiveUsers.Where(x => x.LiveMessages.Any(y => !y.Visited)).Count();

//                var userMessages = user.LiveMessages.Select(x => new
//                {
//                    id = x.MessID,
//                    mess = x.TextMess,
//                    date = x.DateAdded.ToString("dd MMM, hh:mm")
//                }).ToArray();
//                return Json(new
//                {
//                    totalM = userMessages,
//                    name = user.UserName,
//                    connid = conn,
//                    totalQ = newQuestions,
//                    groupId = user1.GroupId
//                }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        public static bool ConsultIsOnline()
//        {

//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var user = db.LiveUsers.SingleOrDefault(x => (bool)x.IsAdmin);
//                if (user.IsOnline)
//                    return true;
//            }
//            return false;

//        }
//        public static void SetConsultOffline()
//        {
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                db.LiveUsers.Where(x => (bool)x.IsAdmin).ToList().ForEach(x => x.IsOnline = false);
//                //user.IsOnline = false;
//                db.SaveChanges();
//            }
//        }
//        public static string ConsultConnId()
//        {
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var user = db.LiveUsers.SingleOrDefault(x => (bool)x.IsAdmin);
//                return user.ConnId;
//            }
//        }
//        public static string GetUserNameByConnId(string connId)
//        {
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var user = db.LiveUsers.SingleOrDefault(x => x.ConnId == connId);
//                return user.UserName;
//            }
//        }
//        public static string ConsultGroupId()
//        {
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var user = db.LiveUsers.SingleOrDefault(x => (bool)x.IsAdmin);
//                return user.GroupId;
//            }
//        }
//        public static int OnlineUsersCount()
//        {
//            using (ApplicationDbContext db = new ApplicationDbContext())
//            {
//                var users = db.LiveUsers.Where(x => x.IsOnline == true && !x.IsAdmin).Count();
//                return users;
//            }
//        }
//        public ActionResult GetChatRoom()
//        {
//            ViewBag.AdmIsOnline = ConsultIsOnline();
//            ViewBag.OnlineUsers = OnlineUsersCount();
//            //Thread.Sleep(1000);
//            //List<LiveUser> items = null;
//            //using (ApplicationDbContext db = new ApplicationDbContext())
//            //{

//            //    items = db.LiveUsers.Include("LiveMessages").ToList();
//            //    ViewBag.Users = items;
//            //}

//            return PartialView();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public JsonResult SendFeed(LiveUser user)
//        {
//            Thread.Sleep(2000);
//            if (ModelState.IsValid)
//            {
//                _emailSender.SendMail(user.UserName, user.FeedMessage, "Консультант:Сообщение с сайта!", user.Email);
//                return Json("Ok");
//            }
//            throw new HttpException();
//        }
//        public ActionResult FormFeed()
//        {
//            return PartialView();
//        }
//    }
//}
