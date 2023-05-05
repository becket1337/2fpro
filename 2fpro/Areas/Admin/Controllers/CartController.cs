using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using _2fpro.Extension;
using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.Service.Repository;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace _2fpro.Areas.Admin.Controllers
{

    public class CartController : Controller
    {
        //
        // GET: /Admin/Cart/

        IProductRepository _repo;
        private IDistributedCache _cache;
        public CartController(IProductRepository repo, IDistributedCache cache)
        {

            _repo = repo;
            _cache = cache;
        }


        public async Task<IActionResult> Index()
        {
            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });
        }

        public ActionResult CartSummary()
        {
            return PartialView(GetCart());
        }


        public ActionResult CartPartial()
        {

            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });

        }
        public ActionResult ViewCart(int prodId = 0)
        {

            return PartialView(new CartViewModel
            {
                Cart = GetCart()
            });

        }
        [HttpPost]
        public JsonResult Clear(Cart cart)
        {

            cart.Clear();
            UpdateCart(cart);
            return Json("");
        }
        public JsonResult UpdateItem(int pid, int type)
        {

            var prod = _repo.Get(pid);
            var cart = GetCart();
            var newQ = type == 0 ? --cart.GetLine(pid).Quantity : ++cart.GetLine(pid).Quantity;
            if (cart.GetLine(pid).Quantity == 0)
                newQ = ++cart.GetLine(pid).Quantity;
            if (cart.GetLine(pid).Quantity == 20)
                newQ = --cart.GetLine(pid).Quantity;
            //GetCart().ChangeQuantity(prod, newQ);
            UpdateCart(cart);
            return Json(new { s = GetCart().TotalValue().ToString("000"), t = newQ, quant = GetCart().GetLine(pid).Quantity, price = GetCart().GetLine(pid).Product.DiscountedPrice() });
        }
        public ActionResult NewItem(int prodId)
        {
            var prod = _repo.Get(prodId);
            return PartialView(prod);
        }
        [HttpPost]
        public JsonResult AddToCart(string prodId, int quan = 1, string t = "c")
        {
            int id = Int32.Parse(EncryptHelper.Decrypt(prodId));
            var prod = _repo.Products.Include("ProdImages").FirstOrDefault(x => x.ID == id);

            //if (lastPrice - prod.Price > 10000||lastPrice<prod.Price||lastPrice<4500) throw new HttpException();
            //if (cloth==null || color == null) throw new HttpException();
            if (prod == null) throw new ArgumentException();
            var cart = GetCart();
            if (prod != null && t == "d")
            {

                cart.AddItem(prod, quan);
            }
            else if (prod != null && t == "c")
            {
                cart.AddItem(prod, quan);
            }
            UpdateCart(cart);
            //Guid.NewGuid().ToString();
            return Json(new
            {

                total = GetCart().TotalValue(),
                count = GetCart().SumItems,//GetCart().Lines.Count(),
                price = prod.DiscountedPrice(),
                title = prod.ProductName,
                cid = Guid.NewGuid().ToString()
            });
        }
        [HttpPost]
        public JsonResult ChangeQuantity(int prodId, int quant)
        {
            var prod = _repo.Get(prodId);
            var cart = GetCart();
            cart.ChangeQuantity(prod, quant);
            UpdateCart(cart);
            return Json(new { itemTotal = (prod.Price * quant).ToString("0 000") });
        }
        public JsonResult GetCartSummary()
        {
            var val = string.Format("{0}", GetCart().TotalValue().ToString("0 000"));
            return Json(new
            {
                count = GetCart().Lines.Count(),
                total = (GetCart().Lines.Count() == 0 ? "0" : val)
            });
        }
        public ActionResult GetPartialCartRow(int prodId)
        {
            var prod = _repo.Get(prodId);
            return PartialView(prod);

        }
        [HttpPost]
        public JsonResult RemoveFromCart(int prodId)
        {

            var prod = _repo.Products.FirstOrDefault(x => x.ID == prodId);
            var cart = GetCart();
            prod = prod == null ? new Product() { ID = prodId } : prod;

            cart.RemoveLine(prod);
            UpdateCart(cart);

            return Json(new
            {
                id = prodId,
                total = GetCart().TotalValue(),
                count = GetCart().Lines.Count(),
            });


        }

        private Cart GetCart()
        {
            var cart = _cache.Get<SessionStorage>(HttpContext.User.Identity.Name);
            if (cart == null)
            {
                cart = new SessionStorage();
                _cache.Set<SessionStorage>(HttpContext.User.Identity.Name, cart);
            }
            return cart.Cart;
        }
        private void UpdateCart(Cart cart)
        {
            var session = _cache.Get<SessionStorage>(HttpContext.User.Identity.Name);
            session.Cart = cart;
            _cache.Set<SessionStorage>(HttpContext.User.Identity.Name, session);

        }
        public bool AddedProdId(int id)
        {


            var added = GetCart().Lines.Select(x => x.Product.ID).Contains(id);
            return added;
        }

    }
}
