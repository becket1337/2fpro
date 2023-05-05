using _2fpro.Models;
using _2fpro.Service.Interface;
using _2fpro.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _2fpro.Extension;

namespace _2fpro.ViewComponents
{
    public class ProdListViewComponent : ViewComponent
    {

        private IProductRepository _prodRep;
        private ICategoryRepository _catRep;
        IDistributedCache _cache;
        public int PageSize = 16;

        public ProdListViewComponent(IProductRepository prodRep, ICategoryRepository catRep, IDistributedCache cache)
        {
            _prodRep = prodRep;
            _catRep = catRep;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string jsonData)
        {
            // if (jsonData == null && filteredData == null) throw new Exception();
            var lastLogged = await _cache.GetAsync("lastLogged");
            var encLogged = System.Text.Encoding.UTF8.GetString(lastLogged);
            SessionStorage session = null;
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                session = await _cache.GetAsync<SessionStorage>(HttpContext.User.Identity.Name);
            }
            if (session == null)
            {


                session = await _cache.GetAsync<SessionStorage>(encLogged);
                if (session == null)
                {
                    session = new SessionStorage();
                    await _cache.SetAsync<SessionStorage>(encLogged, session);
                }
                ViewBag.GetCart = session;
            }
            else
            {
                ViewBag.GetCart = session;
            }


            //var filterData = JsonConvert.DeserializeObject<PagingInfo>(filteredData);

            PagingInfo listInfo = JsonConvert.DeserializeObject<PagingInfo>(jsonData);


            //JObject obj2 = JObject.Parse(jsonData);
            //var listInfo = jsonData;
            //obj2.SelectToken("catId").Value<int>();


            string cattype = listInfo.cattype;
            int num = listInfo.page;//obj2.SelectToken("page").Value<int>();
            int[] modelsArr = listInfo.models == null ? new int[0] : listInfo.models;
            int[] partsArr = listInfo.parts == null ? new int[0] : listInfo.parts;

            List<Product> list = await _prodRep.GetAsync();
            List<Category> cats = await _catRep.GetAsync();
            int[] str = null;
            if (partsArr.Length == 0 && modelsArr.Length > 0)
            {

                var subCats = cats.Where(x => modelsArr.Contains(x.ParentCategoryId)).Select(x => x.ID).ToArray();
                str = subCats;
                list = list.Where(x => subCats.Contains(x.CategoryID)).ToList();
            }
            else if (partsArr.Length > 0 && modelsArr.Length == 0)
            {
                list = list.Where(x => partsArr.Contains(x.CategoryID)).ToList();
            }
            else if (partsArr.Length > 0 && modelsArr.Length > 0)
            {
                // 
                if (cattype == "models")
                {
                    //var filterModels = cats.Where(x => modelsArr.Contains(x.ID)).Select(x => x.ID).ToArray();
                    //var filterParts = cats.Where(x => partsArr.Contains(x.ID)).ToList();


                    //if (filterParts.Any(x => filterModels.Contains(x.ParentCategoryId)))
                    //{

                    //}

                    var subCats = cats.Where(x => modelsArr.Contains(x.ParentCategoryId)).Select(x => x.ID).ToArray();
                    str = subCats;
                    list = list.Where(x => subCats.Contains(x.CategoryID)).ToList();
                }
                else list = list.Where(x => partsArr.Contains(x.CategoryID)).ToList();


            }
            else
            {

            }
            ProductListViewModel model2 = null;
            PagingInfo info = null;
            if (partsArr.Length == 0 && modelsArr.Length == 0)
            {
                //Весь Каталог
                model2 = new ProductListViewModel
                {
                    Products = list.OrderBy(x => x.Sortindex).Skip<Product>(((num - 1) * this.PageSize)).Take<Product>(this.PageSize),


                };
                info = new PagingInfo
                {
                    Service = "prod",
                    CurrentPage = num,
                    Sort = this.PageSize.ToString(),
                    ItemsPerPage = this.PageSize,
                    TotalItems = list.Count(),
                    CatId = 0,
                    Condition = true,
                    catname = "Каталог"
                };
            }
            else
            {

                //Каталог под категорию  
                model2 = new ProductListViewModel
                {
                    Products = list.OrderBy(x => x.Sortindex).Skip<Product>(((num - 1) * this.PageSize)).Take<Product>(this.PageSize),
                    Models = listInfo.models,
                    Parts = listInfo.parts

                };
                info = new PagingInfo
                {
                    Service = "prod",
                    CurrentPage = num,
                    Sort = this.PageSize.ToString(),
                    ItemsPerPage = this.PageSize,
                    TotalItems = list.Count(),

                    Condition = true

                };
            }

            model2.PagingInfo = info;
            ProductListViewModel model = model2;
            return View(model);

        }

    }
}
