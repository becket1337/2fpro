
using System;
using System.Collections.Generic;
using System.Linq;

using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;

using System.Threading.Tasks;
using _2fpro.Extension;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using _2fpro.Service.Repository;

namespace _2fpro.Areas.Admin.Controllers
{

    public class CheckoutController : Controller
    {
        //
        // GET: /Admin/Checkout/
        private IProductRepository _product;
        private IOrderRepository _order;
        private IOrderStatusRepository _orderStatus;
        private IOrderItemRepository _orderItem;
        private IDistributedCache _cache;
        private IEmailService _mail;
        private IDiagnosticService _diag;
        public CheckoutController
            (
                IProductRepository product,
                IOrderRepository order,
                IOrderStatusRepository orderStatus,
                IOrderItemRepository orderItem,
                IDistributedCache cache,
                IEmailService mail,
                 IDiagnosticService diag
            )
        {
            _cache = cache;
            _product = product;
            _order = order;
            _orderStatus = orderStatus;
            _orderItem = orderItem;
            _mail = mail;
            _diag = diag;
        }

        //[_2fpro.Filters.RequireHttps(RequireSecure = true)]
        //[ResponseCache(Duration = 60, VaryByQueryKeys = new string[] { "step", "orderid" })]

        public async Task<ActionResult> Index(int step, string orderid = null)
        {
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            //var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //var user = await userManager.FindByNameAsync(User.Identity.Name);

            var cart = GetCart();
            var step2CheckInputs = cart.ClientDetails.HasEmptyProperties();

            ViewBag.Step1 = GetCart().Step1;
            ViewBag.Step2 = GetCart().Step2;
            ViewBag.Step3 = GetCart().Step3;
            ViewBag.Step4 = false;

            if (cart.Lines.Count() == 0 && orderid == null)
            {
                ViewBag.Step = 1;
                return View();
            }
            // Process for Authenticated
            //
            //if (User.Identity.IsAuthenticated)
            //{


            //    if (step == 1)
            //    {
            //        ViewBag.Step = 1;

            //        cart.Step2 = true;
            //        return View();
            //    }
            //    else if (step == 2 && cart.Step2)
            //    {

            //        ViewBag.Step = 2;
            //        //if (!step2CheckInputs) ModelState.AddModelError("", "Заполните все данные в вашей профиле");
            //        cart.UpdateClientDetails(user);
            //        return View(new CheckoutViewModel
            //        {
            //            Address = cart.ClientDetails.Address,
            //            FirstName = cart.ClientDetails.FirstName,
            //            LastName = cart.ClientDetails.LastName,
            //            Email = cart.ClientDetails.Email,
            //            Phone = cart.ClientDetails.Phone,
            //            Delivery = cart.ClientDetails.Delivery,
            //            Payment = cart.ClientDetails.Payment
            //        });



            //    }
            //    else if (step == 3 && cart.Step3)
            //    {
            //        ViewBag.Step = 3;
            //        cart.UpdateDelivery(user);
            //        return View(new CheckoutViewModel { Delivery = user.Delivery });
            //    }
            //    else if (step == 4 && cart.Step4)
            //    {
            //        ViewBag.Step = 4;
            //        cart.UpdatePayment(user);
            //        return View(new CheckoutViewModel { Payment = user.Payment });
            //    }
            //    else { return RedirectToAction("Index", new { step = 1 }); }
            //}
            // Process for Anonymous
            //
            if (step == 1)
            {
                ViewBag.Step = 1;
                //cart.Step2 = true;
                return View();
            }
            else if (step == 2 && cart.Step2)
            {
                ViewBag.Step = 2;


                //if (cart.ClientDetails.FirstName != null)
                //{
                return View(new CheckoutViewModel
                {
                    Address = cart.ClientDetails.Address,
                    FirstName = cart.ClientDetails.FirstName,
                    LastName = cart.ClientDetails.LastName,
                    Email = cart.ClientDetails.Email,
                    Phone = cart.ClientDetails.Phone,
                    Delivery = cart.ClientDetails.Delivery,
                    Payment = cart.ClientDetails.Payment
                });
                //}
                //return View(new CheckoutViewModel());
            }
            else if (orderid != null)
            {
                if (!_order.Orders.Any(x => x.ID == Int32.Parse(EncryptHelper.Decrypt(orderid)))) { ViewBag.Step = 1; return RedirectToAction("Index", new { step = 1 }); }
                ViewBag.Step = 3;
                ViewBag.Step4 = true;
                var orderID = Int32.Parse(EncryptHelper.Decrypt(orderid));
                if (!_order.Orders.Any(x => x.ID == orderID)) return NotFound();
                return View(new CheckoutViewModel { OrderIdInt = orderID });
            }

            else { ViewBag.Step = 1; return RedirectToAction("Index", new { step = 1 }); }
        }
        [HttpPost]
        public JsonResult Processing()
        {
            System.Threading.Thread.Sleep(1000);
            var t = (User.Identity.IsAuthenticated ? 1 : 0);
            var cart = GetCart();
            cart.Step2 = true;
            UpdateCart(cart);
            return Json(new { type = t, step = cart.Step2 });
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ProceedCheckout(CheckoutViewModel vm)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        //var user = await userManager.FindByNameAsync(User.Identity.Name);

        //        var cart = GetCart();
        //        cart.Step3 = true;
        //        cart.UpdateClientDetails(vm);

        //        //user.Address = vm.Address;
        //        //user.Firstname = vm.FirstName;
        //        //user.Sirname = vm.LastName;
        //        //user.Phone = vm.Phone;
        //        //user.Delivery = vm.Delivery;
        //        //user.Payment = vm.Payment;
        //        //await userManager.UpdateAsync(user);

        //        return RedirectToAction("Index", new { step = 3 });
        //    }
        //    return RedirectToAction("Index", new { step = 2 });
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async   Task<ActionResult> ProceedDelivery(Checkout_Delivery vm)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //        //var user = await userManager.FindByNameAsync(User.Identity.Name);
        //        var cart = GetCart();
        //        cart.Step4 = true;
        //        cart.UpdateDelivery(vm);
        //        //user.Delivery = vm.Delivery;
        //        //await userManager.UpdateAsync(user);

        //        return RedirectToAction("Index", new { step = 4 });
        //    }
        //    return RedirectToAction("Index", new { step = 3 });
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ProceedPayment(CheckoutViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var cart = GetCart();
                //var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var user = await userManager.FindByNameAsync(User.Identity.Name);

                //DELIVERY
                //
                var diag = await _diag.GetAsync(User.Identity.Name);
                var locInfo = diag != null ? diag.City + " / " + diag.Country : "";
                cart.UpdateClientDetails(vm);
                UpdateCart(cart);
                var clientInfo = cart.ClientDetails;
                var step2CheckInputs = cart.ClientDetails.HasEmptyProperties();
                if (!step2CheckInputs)
                {

                    ModelState.AddModelError("", "не все поля заполнены");
                    RedirectToAction("Index", new { step = 2 });

                }
                cart.Step3 = true;
                UpdateCart(cart);
                //update user payment details

                //user.Payment = vm.Payment;
                //await userManager.UpdateAsync(user);

                //make order
                var order = new Order();
                order.Address = cart.ClientDetails.Address;
                order.Name = string.Format("{0} {1}", clientInfo.FirstName, clientInfo.LastName);
                order.Phone = clientInfo.Phone;
                order.OrderStatus = "Не просмотрено";
                order.Payment = clientInfo.Payment;
                order.CreatedAt = DateTime.Now;
                order.EmailAddress = cart.ClientDetails.Email;
                order.Delivery = clientInfo.Delivery;
                order.OrderSum = cart.TotalValue();
                order.Sequance = 1;
                await _order.Create(order);

                string orderItemMess = "";
                foreach (var item in cart.Lines)
                {

                    await _orderItem.Create(item.Product, item.Quantity, order.ID, (float)item.Product.DiscountedPrice());
                    orderItemMess += $@"<div> <strong>Заказ:</strong> {item.Product.ProductName} x {item.Quantity}  = {item.Quantity * (int)item.Product.DiscountedPrice()}</div>";
                }
                ViewBag.loc = diag.City + " / " + diag.Country;
                var bodyMessage =
                  $@"
                        <h3>Время заказа: {order.CreatedAt}<br />
                            Заказчик: {order.Name}   -   {locInfo} </h3>
                        <div>
                            <table>
                                <tr>
                                <td>Телефон:</td>
                                <td>Адрес:</td>
                                <td>Почта:</td>
                                <td>

                                </tr>
                                <tr>
                                <td>{order.Phone}</td>
                                <td>{order.Address}</td>
                                <td>{order.EmailAddress}</td>


                                </tr>
                              </table>
                        </div>
                        {orderItemMess}
                        <div>Итог:{order.OrderSum.ToString("N0")} руб</div>
                    ";
                /*
                YaMoney ya = new YaMoney();
                string url = ya.GetTokenRequestURL();
                Response.Redirect(url);
                */
                cart.Clear();
                UpdateCart(cart);

                await _mail.SendEmailAsync(order.Name, bodyMessage, "Заказ с сайта!", order.EmailAddress, true);
                return RedirectToAction("Index", new { orderid = EncryptHelper.Encrypt(order.ID.ToString()) });
            }
            return RedirectToAction("Index", new { step = 2 });
        }

        //[_2fpro.Filters.RequireHttps(RequireSecure = true)]

        public ActionResult Finished(bool transactionOk = false, string orderId = null)
        {
            /* YaMoney ya = new YaMoney();
             if (transactionOk&&(string)Session["token"]!=null) {

                
                 string tokenId = (string)Session["token"];
                 ya.AccessToken = tokenId;

                 if (tokenId != null)
                 {
                  
                    ViewBag.AccInfo =  ya.ProcessPayment();
               
                 }


                // ViewBag.YaMessage = "Транзакция окончена успешно !";
             }*/


            var order = _order.Get(Int32.Parse(EncryptHelper.Decrypt(orderId)));
            return View(order);
        }
        private Cart GetCart()
        {
            var session = _cache.Get<SessionStorage>(HttpContext.User.Identity.Name);
            if (session == null)
            {
                session = new SessionStorage();
                _cache.Set<SessionStorage>(HttpContext.User.Identity.Name, session);
            }
            return session.Cart;
        }
        private void UpdateCart(Cart cart)
        {
            var session = _cache.Get<SessionStorage>(HttpContext.User.Identity.Name);
            session.Cart = cart;
            _cache.Set<SessionStorage>(HttpContext.User.Identity.Name, session);

        }

        #region Single Order

        public IEnumerable<SelectListItem> GetYears()
        {
            List<SelectListItem> years = new List<SelectListItem>();
            for (var i = 1930; i <= 2000; i++)
            {
                years.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            IEnumerable<SelectListItem> formattedYears = years.GroupBy(x => x.Text).Select(x => x.FirstOrDefault()).ToList<SelectListItem>().OrderBy(x => x.Text);
            return formattedYears;
        }
        public IEnumerable<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>{

                new SelectListItem {Text="Январь",Value="1"},
                new SelectListItem {Text="Февраль",Value="2"},
                new SelectListItem {Text="Март",Value="3"},
                new SelectListItem {Text="Апрель",Value="4"},
                new SelectListItem {Text="Май",Value="5"},
                new SelectListItem {Text="Июнь",Value="6"},
                new SelectListItem {Text="Июль",Value="7"},
                new SelectListItem {Text="Август",Value="8"},
                new SelectListItem {Text="Сентябрь",Value="9"},
                new SelectListItem {Text="Октябрь",Value="10"},
                new SelectListItem {Text="Ноябрь",Value="11"},
                new SelectListItem {Text="Декабрь",Value="12"}
            };
            return months;


        }

        public IEnumerable<SelectListItem> GetDays()
        {
            List<SelectListItem> days = new List<SelectListItem>();

            for (var i = 1; i <= 31; i++)
            {
                days.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return days;
        }

        public ActionResult OrderPartial()
        {
            ViewBag.Year = GetYears();
            ViewBag.Month = GetMonths();
            ViewBag.Day = GetDays();
            return PartialView(new CheckoutViewModel());
        }

        [HttpPost]
        public ActionResult OrderPartial(CheckoutViewModel vm)
        {

            System.Threading.Thread.Sleep(1000);

            if (ModelState.IsValid)
            {
                var order = new Order();
                order.EmailAddress = vm.Email;
                order.Name = vm.Name;
                order.Phone = vm.Phone;
                order.OrderStatus = "Не просмотрено";
                order.CreatedAt = DateTime.Now;

                _order.Create(order);
                return PartialView("OrderOk");
            }
            return StatusCode(404);
        }

        #endregion
        public StatusCodeResult StatusCodeActionResult()
        {
            return StatusCode(404);
        }

    }
}
