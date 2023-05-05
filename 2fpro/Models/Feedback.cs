using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;


namespace _2fpro.Models
{
    public class Feedback
    {
        [Required(ErrorMessage = "<span class='entypo-alert'></span>Обязательное поле к заполнению")]
        [Display(Name = "Имя пользователя")]
        [StringLength(100)]
        public string Name { get; set; }


        [Display(Name = "Адрес электронной почты")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
                    ErrorMessage = "<span class='entypo-alert'></span>Неверный формат электронной почты")]
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        public string StringField { get; set; }

        [StringLength(250)]
        [Display(Name = "Сообщение")]
        public string Text { get; set; }

        [Remote("ValidCaptcha", "Feedback", HttpMethod = "POST",
            ErrorMessage = "<span class='entypo-alert'></span> Ошибка ! Введите код проверки снова.")]
        public string Captcha { get; set; }
    }
}