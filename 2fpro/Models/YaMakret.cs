using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _2fpro.Models
{
    public class YaMakret
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string UrlSite { get; set; }

        public string Currency { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public string SiteName { get; set; }
        public string Delivery { get; set; }
        public string Pickup { get; set; }

        [StringLength(50, ErrorMessage = "Длина не должна превышать 50 символов")]
        public string SalesNotes { get; set; }
        public string Condition { get; set; }

        public string Gifts { get; set; }
        public string Promos { get; set; }

        public int SomeValue { get; set; }
        public int SomeValueTwo { get; set; }

        public string SomeStrValue { get; set; }
        public string SomeStrValueTwo { get; set; }

        public decimal SomeDecValue { get; set; }

        public bool SomeBoolValue { get; set; }
        public bool SomeBoolValueTwo { get; set; }

    }
}
