using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace _2fpro.ViewModel
{

    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "заполните имя")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "заполните фамилию")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "заполните адрес доставки")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "заполните номер телефона")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "заполните почту")]
        [EmailAddress(ErrorMessage = "введите правильный формат почты")]
        public string Email { get; set; }
        public string OrderId { get; set; }
        public int OrderIdInt { get; set; }
        [Required(ErrorMessage = "способ оплаты")]
        public string Payment { get; set; }
        [Required(ErrorMessage = "тип доставки")]
        public string Delivery { get; set; }


        public string Day { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Comment { get; set; }

        public string Name { get; set; }

        public IEnumerable<SelectListItem> DeliveryList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem{Text="В радиусе МКАД",Value="В радиусе МКАД"},
                    new SelectListItem{Text="За МКАД",Value="За МКАД"},
                    new SelectListItem{Text="Иногородняя",Value="Иногородняя"}
                };
            }
        }
        public IEnumerable<SelectListItem> PaymentList
        {
            get
            {
                return new List<SelectListItem>
                {
                    //new SelectListItem{Text="Яндекс.Деньги",Value="Яндекс.Деньги"},
                    //new SelectListItem{Text="WebMoney",Value="WebMoney"},
                   
                    new SelectListItem{Text="Оплата наличными",Value="Оплата наличными"},
                    new SelectListItem{Text="По договору",Value="По договору"}
                };
            }
        }

        public class Checkout_Delivery
        {
            public string Delivery { get; set; }
        }

        public class Checkout_Payment
        {
            public string Payment { get; set; }
        }
    }


}