using _2fpro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace _2fpro.ViewModel
{
    public class MenuViewModel
    {
        public MenuViewModel() { ParentId = 0; }
        private List<MenuSelectListItems> _menuSectionList;// коллекция выбора для категории меню
        private List<MenuViewModel> MenuParentList { get; set; } // коллекция выбора для Родительских разделов

        public IEnumerable<Menu> Menues { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int ctype { get; set; }


        public int Id { get; set; }

        public int ParentId { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните название")]
        public string Text { get; set; }
        [StringLength(200)]
        [Required(ErrorMessage = "Заполните Url")]
        [RegularExpression(@"([a-zA-Z0-9]+(-|_)[-a-zA-Z0-9]+|\w+)", ErrorMessage = "допустимый формат ввода = o_kompanii или o-kompanii)")]
        public string Url { get; set; }


        public string Body { get; set; }
        public string HiddenBody { get; set; }
        public string BodyEng { get; set; }

        public bool IsPublish { get; set; }
        public string CustomField { get; set; }
        public string SeoDescription { get; set; }

        public DateTime? LastModifiedDate { get; set; }


        public string SeoKeywords { get; set; }


        public int SortOrder { get; set; }

        public int MenuSection { get; set; }

        public bool HasChildMenu { get; set; }


        public List<MenuSelectListItems> MenuSelectList
        {
            get
            {
                _menuSectionList = new List<MenuSelectListItems>();
                _menuSectionList.Add(new MenuSelectListItems { ID = 0, Text = "Навигация первая 1-категория" });
                _menuSectionList.Add(new MenuSelectListItems { ID = 1, Text = "Навигация первая 2-категория" });
                return _menuSectionList;
            }
        }


    }

    public class MenuSelectListItems
    {

        public int ID { get; set; }
        public string Text { get; set; }


    }
}