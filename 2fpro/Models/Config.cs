using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace _2fpro.Models
{
    public class Config
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public int ConfigID { get; set; }


        [Display(Name = "Название сайта")]
        [StringLength(300)]
        public string SiteName { get; set; }

        [Display(Name = "Robots.txt")]
        public string Robots { get; set; }

        [Display(Name = "Адрес сайта")]
        [StringLength(300)]
        [Url(ErrorMessage = "введите полный адрес страницы")]
        public string SiteAddress { get; set; }

        [Display(Name = "Описание сайта")]
        public string Description { get; set; }

        [Display(Name = "Ключевые слова")]
        public string Keywords { get; set; }

        [Display(Name = "Email")]
        [StringLength(300)]
        [EmailAddress(ErrorMessage = "некоректная почта")]
        public string Email { get; set; }

        [Display(Name = "Отключить сайт")]
        public bool SelectedIsOnlineID { get; set; }

        [Display(Name = "Сообщение при неработающем сайте")]
        public string OfflineMessage { get; set; }



    }


}