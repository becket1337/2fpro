using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Threading.Tasks;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using _2fpro.Extension;
using Microsoft.EntityFrameworkCore;

namespace _2fpro.Areas.Admin.Controllers
{

    public class OrderController : Controller
    {
        private IProductRepository _product;
        private IOrderRepository _orderRepo;
        private IEmailService _emailSender;
        private IDistributedCache _cache;
        private IOrderItemRepository _orderItem;
        private IDiagnosticService _diag;
        public OrderController(IOrderRepository orderRepo, IEmailService emailSender, IDistributedCache cache, IOrderItemRepository orderItem, IProductRepository product, IDiagnosticService diag)
        {
            _orderRepo = orderRepo;
            _emailSender = emailSender;
            _cache = cache;
            _product = product;
            _orderItem = orderItem;
            _diag = diag;
        }

        //
        // GET: /Admin/Order/
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int page = 1, string sort = null)
        {
            int pageSize = 20;
            var orderList = sort == null ? _orderRepo.Orders : _orderRepo.Orders.Where(x => x.OrderStatus == sort);
            OrderViewModel vm = new OrderViewModel
            {
                Orders = orderList
                .OrderBy(x => x.Sequance)
                .ThenByDescending(x => x.CreatedAt)
                .ToList()

                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = page,
                    TotalItems = orderList.Count(),
                    Sort = sort,
                    Condition = false,
                    Service = "order"

                }
            };
            return View(vm);
        }
        public async Task<IActionResult> Makeorder(string id, int quan = 1)
        {
            var idCheckSpace = id.Replace(" ", "+");
            var deId = Int32.Parse(EncryptHelper.Decrypt(idCheckSpace));
            var checkProd = await _product.Products.FirstOrDefaultAsync(x => x.ID == deId);
            if (quan >= 10001 || quan < 1 || checkProd == null) throw new ArgumentException();
            return ViewComponent("MakeOrder", new { Quan = quan, Id = deId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Makeorder(OrderViewModel _order)
        {
            //System.Threading.Thread.Sleep(500);
            if (ModelState.IsValid)
            {
                if (Int32.Parse(_order.Quan) >= 10001 || Int32.Parse(_order.Quan) < 1) throw new ArgumentException();
                var diag = await _diag.GetAsync(User.Identity.Name);
                var locInfo = diag != null ? diag.City + " / " + diag.Country : "";
                var prod = await _product.Products.FirstOrDefaultAsync(x => x.ID == _order.prodId);
                var order = new Order();
                order.Address = _order.Address;
                order.Comment = _order.Desc;
                order.Name = _order.Name + ", " + locInfo;

                prod.Packaging = prod.Packaging == null ? "1" : (int.Parse(prod.Packaging) + 1).ToString();
                order.EmailAddress = _order.Email;
                order.Phone = _order.Phone;
                order.OrderStatus = "Не просмотрено";
                order.Payment = _order.Service;
                order.Sequance = 1;
                order.OrderSum = (float)prod.DiscountedPrice() * Int32.Parse(_order.Quan);
                order.CreatedAt = DateTime.Now;
                await _orderRepo.Create(order);
                await _orderItem.Create(prod, Int32.Parse(_order.Quan), order.ID, (float)prod.DiscountedPrice());
                var bodyMessage =
                    $@"
                         <h3>Время заказа: {order.CreatedAt}<br />
                            Заказчик: {order.Name} </h3>
                        <div>
<p>Телефон : {order.Phone}</p>
<p>Адрес : {order.Address}</p>
<p>Почта : {_order.Email}</p>
                           
                        </div>
                        <p> <strong>Заказ:</strong> {_order.ProdName} x {_order.Quan}  = {_order.TotalSum}</p>";
                //                    <dt>Телефон: </dt>
                //    <dd>{order.Phone}</dd>
                //    <dt>Адрес: </dt>
                //    <dd>{order.Address}</dd>
                //    <dt>Почта: </dt>
                //    <dd>{_order.Email}</dd>

                //</ dl >
                await _emailSender.SendEmailAsync(order.Name, bodyMessage, "Заказ с сайта!", _order.Email, true);
                var cart = GetCart();
                cart.Clear();
                //await _emailSender.SendEmailAsync();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {

                    return Json("Заказ принят!");
                }
                throw new ArgumentException();
                //return Redirect("/");
            }
            throw new ArgumentException();
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
        //
        // GET: /Admin/Order/Details/5
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Order order = _orderRepo.Get(id);

            if (order == null)
            {
                return BadRequest("404");
            }
            return View(new OrderViewModel
            {
                Order = order,
                OrderStatus = order.OrderStatus


            });
        }

        //
        // POST: /Admin/Order/Delete/5
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAll()
        {

            await _orderRepo.DeleteAll();
            return LocalRedirect("/Admin/Order/Index?page=1");
        }

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, int Npage = 1)
        {
            Order order = _orderRepo.Get(id);
            _orderRepo.Delete(order);
            return LocalRedirect("/Admin/Order/Index?page=" + Npage);
        }

        [HttpPost]
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public JsonResult ChangeStatus([FromBody]StatusTypes model)
        {

            var order = _orderRepo.Get(model.orderid);
            order.OrderStatus = model.orderstatus;
            switch (model.orderstatus)
            {
                case ("Не просмотрено"):
                    order.Sequance = 1; break;
                case ("Отменено"):
                    order.Sequance = 4; break;
                case ("В процессе"):
                    order.Sequance = 3; break;
                case ("Завершен"):
                    order.Sequance = 2; break;
                    //case ("Отложено"):
                    //    order.Sequance = 5; break;

            }
            _orderRepo.Update();
            return Json(new { Name = model.orderstatus, Seq = order.Sequance });
        }
        public class StatusTypes
        {
            public int orderid { get; set; }
            public string orderstatus { get; set; }
        }
        public enum statusList
        {
            nepros,
            otlog,
            narasm,
            otmen,
            ogidaet
        }
    }
}