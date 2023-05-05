using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.ViewModel
{
    public class FeedbackViewModel
    {
        [Required(ErrorMessage = "Обязательное поле к заполнению")]
        [Display(Name = "Имя пользователя")]
        [StringLength(100)]
        public string Name { get; set; }


        [Display(Name = "Адрес электронной почты")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",
                    ErrorMessage = "Неверный формат электронной почты")]
        [StringLength(100)]
        [Required(ErrorMessage = "Обязательное поле к заполнению")]
        public string Email { get; set; }
        [StringLength(100)]
        public string PhoneNumber { get; set; }
        public string StringField { get; set; }

        [StringLength(250)]
        [Display(Name = "Сообщение")]
        [Required(ErrorMessage = "Обязательное поле к заполнению")]
        public string Text { get; set; }

        [Remote("ValidCaptcha", "Feedback", HttpMethod = "POST",
            ErrorMessage = "Ошибка ! Введите код проверки снова.")]
        public string Captcha { get; set; }
    }
}
